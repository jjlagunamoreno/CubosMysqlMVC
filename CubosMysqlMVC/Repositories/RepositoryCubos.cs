using Microsoft.EntityFrameworkCore;
using PracticaCubosMVC.Data;
using PracticaCubosMVC.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PracticaCubosMVC.Repositories
{
    public class RepositoryCubos
    {
        private readonly CubosContext _context;

        public RepositoryCubos(CubosContext context)
        {
            _context = context;
        }

        public async Task<List<Cubo>> GetCubosAsync()
        {
            return await _context.Cubos.ToListAsync();
        }

        public async Task<Cubo> GetCuboByIdAsync(int id)
        {
            return await _context.Cubos.FindAsync(id);
        }
        public async Task AddCuboAsync(Cubo cubo)
        {
            try
            {
                // Buscar el último ID en la tabla CUBOS y generar el siguiente
                int maxId = await _context.Cubos.AnyAsync() ? await _context.Cubos.MaxAsync(c => c.IdCubo) : 0;
                cubo.IdCubo = maxId + 1;

                _context.Cubos.Add(cubo);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Cubo {cubo.Nombre} agregado con ID {cubo.IdCubo}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar el cubo: {ex.Message}");
            }
        }

        public async Task DeleteCuboAsync(int id)
        {
            var cubo = await _context.Cubos.FindAsync(id);
            if (cubo != null)
            {
                _context.Cubos.Remove(cubo);
                await _context.SaveChangesAsync();
            }
        }
    }
}
