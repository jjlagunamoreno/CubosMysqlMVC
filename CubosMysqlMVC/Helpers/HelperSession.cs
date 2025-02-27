using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PracticaCubosMVC.Models;
using System.Collections.Generic;
using System.Linq;

namespace PracticaCubosMVC.Helpers
{
    public class HelperSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CART_KEY = "CART";

        // *****************************
        // CONSTRUCTOR: INICIALIZA LA SESIÓN
        // *****************************
        public HelperSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // *****************************
        // OBTENER LISTA DEL CARRITO DESDE LA SESIÓN
        // *****************************
        public List<Compra> GetCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var data = session.GetString(CART_KEY);
            return data != null ? JsonConvert.DeserializeObject<List<Compra>>(data) : new List<Compra>();
        }

        // *****************************
        // GUARDAR EL CARRITO EN LA SESIÓN
        // *****************************
        private void SaveCart(List<Compra> cart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString(CART_KEY, JsonConvert.SerializeObject(cart));
        }

        // *****************************
        // AGREGAR O QUITAR CUBO DEL CARRITO (TOGGLE)
        // *****************************
        public void ToggleCartItem(Cubo cubo)
        {
            List<Compra> cart = GetCart();
            var item = cart.FirstOrDefault(c => c.IdCubo == cubo.IdCubo);

            if (item != null)
            {
                // SI YA EXISTE, LO QUITAMOS DEL CARRITO
                cart.Remove(item);
            }
            else
            {
                // SI NO EXISTE, LO AGREGAMOS CON CANTIDAD 1
                cart.Add(new Compra
                {
                    IdCubo = cubo.IdCubo,
                    Cantidad = 1,
                    Precio = cubo.Precio,
                    Cubo = cubo
                });
            }

            SaveCart(cart);
        }

        // *****************************
        // ACTUALIZAR LA CANTIDAD DE UN CUBO EN EL CARRITO
        // *****************************
        public void UpdateCartQuantity(int id, int cantidad)
        {
            List<Compra> cart = GetCart();
            var item = cart.FirstOrDefault(c => c.IdCubo == id);

            if (item != null && cantidad > 0)
            {
                // ACTUALIZA LA CANTIDAD
                item.Cantidad = cantidad;
            }
            else
            {
                // SI LA CANTIDAD ES 0, ELIMINA EL CUBO
                cart.RemoveAll(c => c.IdCubo == id);
            }

            SaveCart(cart);
        }

        // *****************************
        // ELIMINAR UN CUBO ESPECÍFICO DEL CARRITO
        // *****************************
        public void RemoveFromCart(int id)
        {
            List<Compra> cart = GetCart();
            cart.RemoveAll(c => c.IdCubo == id);
            SaveCart(cart);
        }

        // *****************************
        // VACIAR COMPLETAMENTE EL CARRITO
        // *****************************
        public void ClearCart()
        {
            _httpContextAccessor.HttpContext.Session.Remove(CART_KEY);
        }
    }
}
