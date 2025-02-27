using Microsoft.AspNetCore.Mvc;
using PracticaCubosMVC.Helpers;
using PracticaCubosMVC.Models;
using PracticaCubosMVC.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticaCubosMVC.Controllers
{
    public class CubosController : Controller
    {
        private readonly RepositoryCubos _repo;
        private readonly HelperPathProvider _helperPath;
        private readonly ILogger<CubosController> _logger;
        private readonly HelperSession _helperSession;
        private readonly HelperCache _helperCache;

        // *****************************
        // CONSTRUCTOR: INYECTA DEPENDENCIAS
        // *****************************
        public CubosController(RepositoryCubos repo, HelperPathProvider helperPath,
                ILogger<CubosController> logger, HelperSession helperSession,
                HelperCache helperCache)
        {
            _repo = repo;
            _helperPath = helperPath;
            _logger = logger;
            _helperSession = helperSession;
            _helperCache = helperCache;
        }

        // *****************************
        // LISTAR TODOS LOS CUBOS
        // *****************************
        public async Task<IActionResult> Index()
        {
            var cubos = await _repo.GetCubosAsync();
            return View(cubos);
        }

        // **********************
        // MOSTRAR HISTORIAL DE COMPRAS DESDE LA BD
        // **********************
        public async Task<IActionResult> ViewPurchases()
        {
            var purchases = await _repo.GetAllPurchasesAsync();
            return View(purchases);
        }

        // *****************************
        // AGREGAR O QUITAR UN CUBO DE FAVORITOS
        // *****************************
        public async Task<IActionResult> ToggleFavorite(int id)
        {
            var cubo = await _repo.GetCuboByIdAsync(id);
            if (cubo != null)
            {
                _helperCache.ToggleFavorite(cubo);
            }
            return RedirectToAction(nameof(Index));
        }

        // *****************************
        // AGREGAR O QUITAR UN CUBO DEL CARRITO
        // *****************************
        public async Task<IActionResult> ToggleCart(int id)
        {
            var cubo = await _repo.GetCuboByIdAsync(id);
            if (cubo != null)
            {
                _helperSession.ToggleCartItem(cubo);
            }
            return RedirectToAction(nameof(Index));
        }

        // *****************************
        // ACTUALIZAR LA CANTIDAD DE UN CUBO EN EL CARRITO
        // *****************************
        public async Task<IActionResult> UpdateCartQuantity(int id, int cantidad)
        {
            _helperSession.UpdateCartQuantity(id, cantidad);
            return RedirectToAction(nameof(ViewCart));
        }

        // *****************************
        // MOSTRAR CARRITO DE COMPRAS
        // *****************************
        public async Task<IActionResult> ViewCart()
        {
            var cart = _helperSession.GetCart();

            // OBTENER SOLO LOS ID DE LOS CUBOS PARA CONSULTA ÚNICA
            var cuboIds = cart.Select(c => c.IdCubo).Distinct().ToList();
            var cubos = await _repo.GetCubosByIdsAsync(cuboIds);

            // ASIGNAR CUBOS A LAS COMPRAS DEL CARRITO
            foreach (var compra in cart)
            {
                compra.Cubo = cubos.FirstOrDefault(c => c.IdCubo == compra.IdCubo);
            }

            return View(cart);
        }

        // *****************************
        // ELIMINAR UN CUBO DEL CARRITO SIN RECARGAR LA PÁGINA (AJAX)
        // *****************************
        [HttpDelete]
        public IActionResult RemoveFromCart(int id)
        {
            _helperSession.RemoveFromCart(id);
            return Ok(); // RETORNA 200 OK SIN REDIRIGIR
        }

        // *****************************
        // VACIAR COMPLETAMENTE EL CARRITO
        // *****************************
        public IActionResult ClearCart()
        {
            _helperSession.ClearCart();
            return RedirectToAction(nameof(ViewCart));
        }

        // *****************************
        // MOSTRAR LISTA DE FAVORITOS
        // *****************************
        public IActionResult ViewFavorites()
        {
            var favoritos = _helperCache.GetFavorites();
            return View(favoritos);
        }

        // *****************************
        // ELIMINAR TODOS LOS FAVORITOS
        // *****************************
        public IActionResult ClearFavorites()
        {
            _helperCache.ClearFavorites();
            return RedirectToAction(nameof(ViewFavorites));
        }

        // *****************************
        // FORMULARIO PARA CREAR UN NUEVO CUBO
        // *****************************
        public IActionResult Create()
        {
            return View();
        }

        // *****************************
        // CREAR UN NUEVO CUBO Y GUARDARLO EN LA BASE DE DATOS
        // *****************************
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cubo cubo, IFormFile imagen)
        {
            if (imagen != null)
            {
                string fileName = Path.GetFileName(imagen.FileName);
                string path = _helperPath.MapPath(fileName, Folders.Images);

                try
                {
                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        await imagen.CopyToAsync(stream);
                    }
                    cubo.Imagen = fileName;
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al guardar la imagen: {ex.Message}");
                    return View(cubo);
                }
            }
            else
            {
                cubo.Imagen = cubo.Imagen ?? "default.png";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _repo.AddCuboAsync(cubo);
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error al guardar el cubo: {ex.Message}");
                }
            }

            return View(cubo);
        }

        // *****************************
        // MOSTRAR DETALLES DE UN CUBO
        // *****************************
        public async Task<IActionResult> Details(int id)
        {
            var cubo = await _repo.GetCuboByIdAsync(id);
            if (cubo == null)
            {
                return NotFound();
            }
            return View(cubo);
        }

        // *****************************
        // CONFIRMAR ELIMINACIÓN DE UN CUBO
        // *****************************
        public async Task<IActionResult> Delete(int id)
        {
            var cubo = await _repo.GetCuboByIdAsync(id);
            if (cubo == null)
            {
                return NotFound();
            }
            return View(cubo);
        }

        // *****************************
        // ELIMINAR UN CUBO DE LA BASE DE DATOS
        // *****************************
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteCuboAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
