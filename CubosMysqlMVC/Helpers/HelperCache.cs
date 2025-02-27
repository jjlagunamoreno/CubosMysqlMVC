using Microsoft.Extensions.Caching.Memory;
using PracticaCubosMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void ToggleFavorite(Cubo cubo)
        {
            List<Cubo> favoritos = GetFavorites();
            if (favoritos.Any(c => c.IdCubo == cubo.IdCubo))
            {
                favoritos.RemoveAll(c => c.IdCubo == cubo.IdCubo);
            }
            else
            {
                favoritos.Add(cubo);
            }
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
