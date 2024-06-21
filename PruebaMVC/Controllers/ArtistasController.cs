using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class ArtistasController(IGenericRepositorio<Artista> context) : Controller
    {
        // GET: Artistas
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["FechaSortParm"] = sortOrder == "Fecha de Nacimiento" ? "fecha_desc" : "Fecha de Nacimiento";
            if (await context.DameTodos() == null)
            {
                return Problem("Es nulo");
            }

            var vista = await context.DameTodos();
            var artistas = from m in vista
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                artistas = artistas.Where(s => s.Nombre!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "nombre_desc":
                    artistas = artistas.OrderByDescending(s => s.Nombre);
                    break;
                case "Genero":
                    artistas = artistas.OrderBy(s => s.Genero);
                    break;
                case "genero_desc":
                    artistas = artistas.OrderByDescending(s => s.Genero);
                    break;
                case "Fecha de Nacimiento":
                    artistas = artistas.OrderBy(s => s.FechaNac);
                    break;
                case "fecha_desc":
                    artistas = artistas.OrderByDescending(s => s.FechaNac);
                    break;
                default:
                    artistas = artistas.OrderBy(s => s.Nombre);
                    break;
            }
            return View(artistas);
        }

        public async Task<IActionResult> IndexConsulta(string sortOrder)
        {
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["FechaSortParm"] = sortOrder == "Fecha de Nacimiento" ? "fecha_desc" : "Fecha de Nacimiento";
            var vista = await context.DameTodos();
            var artistas = vista.Where(x => x.FechaNac !=null && x.FechaNac.Value.Year >1950);

            switch (sortOrder)
            {
                case "nombre_desc":
                    artistas = artistas.OrderByDescending(s => s.Nombre);
                    break;
                case "Genero":
                    artistas = artistas.OrderBy(s => s.Genero);
                    break;
                case "genero_desc":
                    artistas = artistas.OrderByDescending(s => s.Genero);
                    break;
                case "Fecha de Nacimiento":
                    artistas = artistas.OrderBy(s => s.FechaNac);
                    break;
                case "fecha_desc":
                    artistas = artistas.OrderByDescending(s => s.FechaNac);
                    break;
                default:
                    artistas = artistas.OrderBy(s => s.Nombre);
                    break;
            }

            return View(artistas);
        }

        // GET: Artistas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await context.DameTodos();
            var artista = vista
                .FirstOrDefault(m => m.Id == id);
            if (artista == null)
            {
                return NotFound();
            }

            return View(artista);
        }

        // GET: Artistas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Artistas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Genero,FechaNac")] Artista artista, IFormFile foto)
        {
            if (ModelState.IsValid)
            {
                if (foto != null && foto.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await foto.CopyToAsync(memoryStream);
                        artista.Foto = memoryStream.ToArray();
                    }
                }
                await context.Agregar(artista);
                return RedirectToAction(nameof(Index));
            }
            return View(artista);
        }

        // GET: Artistas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
         
            var artista = await context.DameUno(id);
            
            return View(artista);
        }

        // POST: Artistas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Genero,FechaNac")] Artista artista, IFormFile foto)
        {
            if (id != artista.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var artistaToUpdate = await context.DameUno(id);
                    if (foto != null && foto.Length > 0)
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            await foto.CopyToAsync(memoryStream);
                            artistaToUpdate.Foto = memoryStream.ToArray();
                        }
                    }

                    artistaToUpdate.Nombre = artista.Nombre;
                    artistaToUpdate.Genero = artista.Genero;
                    artistaToUpdate.FechaNac = artista.FechaNac;
                    await context.Modificar(id, artistaToUpdate);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistaExists(artista.Id).Result)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(artista);
        }

        // GET: Artistas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await context.DameTodos();
            var artista = vista
                .FirstOrDefault(m => m.Id == id);
            if (artista == null)
            {
                return NotFound();
            }

            return View(artista);
        }

        // POST: Artistas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artista = await context.DameUno(id);
            if (artista != null)
            {
               await context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ArtistaExists(int id)
        {
            var vista = await context.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
