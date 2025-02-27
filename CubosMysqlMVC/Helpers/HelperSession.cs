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

        public HelperSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void ToggleCartItem(Cubo cubo)
        {
            List<Compra> cart = GetCart();
            var item = cart.FirstOrDefault(c => c.IdCubo == cubo.IdCubo);

            if (item != null)
            {
                cart.Remove(item);
            }
            else
            {
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


        public void UpdateCartQuantity(int id, int cantidad)
        {
            List<Compra> cart = GetCart();
            var item = cart.FirstOrDefault(c => c.IdCubo == id);

            if (item != null && cantidad > 0)
            {
                item.Cantidad = cantidad;
            }
            else
            {
                cart.RemoveAll(c => c.IdCubo == id);
            }

            SaveCart(cart);
        }

        public List<Compra> GetCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            var data = session.GetString(CART_KEY);
            return data != null ? JsonConvert.DeserializeObject<List<Compra>>(data) : new List<Compra>();
        }

        public void SaveCart(List<Compra> cart)
        {
            var session = _httpContextAccessor.HttpContext.Session;
            session.SetString(CART_KEY, JsonConvert.SerializeObject(cart));
        }

        public void ClearCart()
        {
            _httpContextAccessor.HttpContext.Session.Remove(CART_KEY);
        }

        public void RemoveFromCart(int id)
        {
            List<Compra> cart = GetCart();
            cart.RemoveAll(c => c.IdCubo == id);
            SaveCart(cart);
        }

    }
}
