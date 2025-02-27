using Microsoft.EntityFrameworkCore;
using PracticaCubosMVC.Data;
using PracticaCubosMVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PracticaCubosMVC.Repositories
{
    public class RepositoryCubos
    {
        private readonly CubosContext _context;

        // **********************
        // CONSTRUCTOR
        // **********************
        public RepositoryCubos(CubosContext context)
        {
            _context = context;
        }

        // **********************
        // OBTENER TODOS LOS CUBOS
        // **********************
        public async Task<List<Cubo>> GetCubosAsync()
        {
            return await _context.Cubos.ToListAsync();
        }

        // **********************
        // OBTENER TODAS LAS COMPRAS DE LA BD
        // **********************
        public async Task<List<Compra>> GetAllPurchasesAsync()
        {
            return await _context.Compras
                .Include(c => c.Cubo) // Incluir los detalles del cubo en la consulta
                .OrderByDescending(c => c.FechaPedido)
                .ToListAsync();
        }

        // **********************
        // OBTENER VARIOS CUBOS POR LISTA DE IDS
        // **********************
        public async Task<List<Cubo>> GetCubosByIdsAsync(List<int> ids)
        {
            return await _context.Cubos
                .Where(c => ids.Contains(c.IdCubo))
                .ToListAsync();
        }

        // **********************
        // OBTENER UN CUBO POR ID
        // **********************
        public async Task<Cubo> GetCuboByIdAsync(int id)
        {
            return await _context.Cubos.FindAsync(id);
        }

        // **********************
        // AGREGAR UN NUEVO CUBO A LA BASE DE DATOS
        // **********************
        public async Task AddCuboAsync(Cubo cubo)
        {
            try
            {
                // OBTENER EL ÚLTIMO ID Y GENERAR EL SIGUIENTE
                int maxId = await _context.Cubos.AnyAsync() ? await _context.Cubos.MaxAsync(c => c.IdCubo) : 0;
                cubo.IdCubo = maxId + 1;

                _context.Cubos.Add(cubo);
                await _context.SaveChangesAsync();
                Console.WriteLine($"Cubo {cubo.Nombre} agregado con ID {cubo.IdCubo}.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR AL AGREGAR EL CUBO: {ex.Message}");
            }
        }

        // **********************
        // ELIMINAR UN CUBO POR ID
        // **********************
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
