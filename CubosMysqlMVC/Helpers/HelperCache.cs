using Microsoft.Extensions.Caching.Memory;
using PracticaCubosMVC.Models;
using System;
using System.Collections.Generic;

namespace PracticaCubosMVC.Helpers
{
    public class HelperCache
    {
        private readonly IMemoryCache _cache;
        private const string FAVORITOS_KEY = "FAVORITOS";

        public HelperCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        public void AddCuboToFavorites(Cubo cubo)
        {
            List<Cubo> favoritos = GetFavorites() ?? new List<Cubo>();
            favoritos.Add(cubo);
            _cache.Set(FAVORITOS_KEY, favoritos, TimeSpan.FromMinutes(60));
        }

        public List<Cubo> GetFavorites()
        {
            return _cache.TryGetValue(FAVORITOS_KEY, out List<Cubo> favoritos) ? favoritos : new List<Cubo>();
        }

        public void ClearFavorites()
        {
            _cache.Remove(FAVORITOS_KEY);
        }
    }
}
