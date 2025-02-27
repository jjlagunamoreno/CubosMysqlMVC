using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using PracticaCubosMVC.Models;
using System.Collections.Generic;

namespace PracticaCubosMVC.Helpers
{
    public class HelperSession
    {
        private readonly ISession _session;

        public HelperSession(IHttpContextAccessor httpContextAccessor)
        {
            _session = httpContextAccessor.HttpContext.Session;
        }

        public void AddCuboToCart(Cubo cubo)
        {
            List<Cubo> cubos = GetCart() ?? new List<Cubo>();
            cubos.Add(cubo);
            _session.SetString("CART", JsonConvert.SerializeObject(cubos));
        }

        public List<Cubo> GetCart()
        {
            string data = _session.GetString("CART");
            return data == null ? new List<Cubo>() : JsonConvert.DeserializeObject<List<Cubo>>(data);
        }

        public void ClearCart()
        {
            _session.Remove("CART");
        }
    }
}
