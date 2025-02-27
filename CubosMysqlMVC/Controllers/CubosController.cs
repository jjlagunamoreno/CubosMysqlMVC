using Microsoft.AspNetCore.Mvc;
using PracticaCubosMVC.Helpers;
using PracticaCubosMVC.Models;
using PracticaCubosMVC.Repositories;
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

        public async Task<IActionResult> Index()
        {
            var cubos = await _repo.GetCubosAsync();
            return View(cubos);
        }

        public async Task<IActionResult> AddToCart(int id)
        {
            var cubo = await _repo.GetCuboByIdAsync(id);
            if (cubo != null)
            {
                _helperSession.AddCuboToCart(cubo);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ViewCart()
        {
            var cart = _helperSession.GetCart();
            return View(cart);
        }

        public IActionResult ClearCart()
        {
            _helperSession.ClearCart();
            return RedirectToAction(nameof(ViewCart));
        }

        public async Task<IActionResult> AddToFavorites(int id)
        {
            var cubo = await _repo.GetCuboByIdAsync(id);
            if (cubo != null)
            {
                _helperCache.AddCuboToFavorites(cubo);
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ViewFavorites()
        {
            var favoritos = _helperCache.GetFavorites();
            return View(favoritos);
        }

        public IActionResult ClearFavorites()
        {
            _helperCache.ClearFavorites();
            return RedirectToAction(nameof(ViewFavorites));
        }

        public IActionResult Create()
        {
            return View();
        }

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

        public async Task<IActionResult> Details(int id)
        {
            var cubo = await _repo.GetCuboByIdAsync(id);
            if (cubo == null)
            {
                return NotFound();
            }
            return View(cubo);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var cubo = await _repo.GetCuboByIdAsync(id);
            if (cubo == null)
            {
                return NotFound();
            }
            return View(cubo);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteCuboAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
