using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;
using System.Collections.Generic;

namespace PruebaMVC.Controllers
{
    public class ConciertoesController(IGenericRepositorio<Concierto> context) : Controller
    {
        // GET: Conciertoes
        public async Task<IActionResult> Index(string sortOrder,string searchString)
        {
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "titulo_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["FechaSortParm"] = sortOrder == "Fecha" ? "fecha-desc" : "Fecha";
            ViewData["LugarSortParm"] = sortOrder == "Lugar" ? "lugar_desc" : "Lugar";
            ViewData["PrecioSortParm"] = sortOrder == "Precio" ? "precio_desc" : "Precio";

            var vista = await context.DameTodos();
            var conciertos = vista.Select(x=>x);

            if (!String.IsNullOrEmpty(searchString))
            {
                conciertos = conciertos.Where(s => s.Titulo!.Contains(searchString));
            }

            conciertos = sortOrder switch
            {
                "titulo_desc" => conciertos.OrderByDescending(s => s.Titulo),
                "Lugar" => conciertos.OrderBy(s => s.Lugar),
                "lugar_desc" => conciertos.OrderByDescending(s => s.Lugar),
                "Genero" => conciertos.OrderBy(s => s.Genero),
                "genero_desc" => conciertos.OrderByDescending(s => s.Genero),
                "Fecha" => conciertos.OrderBy(s => s.Fecha),
                "fecha-desc" => conciertos.OrderByDescending(s => s.Fecha),
                "Precio" => conciertos.OrderBy(s => s.Precio),
                "precio_desc" => conciertos.OrderByDescending(s => s.Precio),
                _ => conciertos.OrderBy(s => s.Titulo)
            };
            return View(conciertos);
        }

        public async Task<IActionResult> IndexConsulta(string sortOrder)
        {
            var vista = await context.DameTodos();
            var conciertos = vista.Where(x => x is { Fecha.Year: > 2015, Precio: > 30 });
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "titulo_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["FechaSortParm"] = sortOrder == "Fecha" ? "fecha-desc" : "Fecha";
            ViewData["LugarSortParm"] = sortOrder == "Lugar" ? "lugar_desc" : "Lugar";
            ViewData["PrecioSortParm"] = sortOrder == "Precio" ? "precio_desc" : "Precio";
            conciertos = sortOrder switch
            {
                "titulo_desc" => conciertos.OrderByDescending(s => s.Titulo),
                "Lugar" => conciertos.OrderBy(s => s.Lugar),
                "lugar_desc" => conciertos.OrderByDescending(s => s.Lugar),
                "Genero" => conciertos.OrderBy(s => s.Genero),
                "genero_desc" => conciertos.OrderByDescending(s => s.Genero),
                "Fecha" => conciertos.OrderBy(s => s.Fecha),
                "fecha-desc" => conciertos.OrderByDescending(s => s.Fecha),
                "Precio" => conciertos.OrderBy(s => s.Precio),
                "precio_desc" => conciertos.OrderByDescending(s => s.Precio),
                _ => conciertos.OrderBy(s => s.Titulo)
            };
            return View(conciertos);
        }

        // GET: Conciertoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {

            var vista = await context.DameTodos();
            var context1 = vista.Select(x=>x);
            var concierto = context1
                .FirstOrDefault(m => m.Id == id);
            if (concierto == null)
            {
                return NotFound();
            }

            return View(concierto);
        }

        // GET: Conciertoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Conciertoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Titulo,Id,Fecha,Genero,Lugar,Precio")] Concierto concierto)
        {
            if (ModelState.IsValid)
            {
                await context.Agregar(concierto);
                return RedirectToAction(nameof(Index));
            }
            return View(concierto);
        }

        // GET: Conciertoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concierto = await context.DameUno((int)id);
            if (concierto == null)
            {
                return NotFound();
            }
            return View(concierto);
        }

        // POST: Conciertoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Titulo,Id,Fecha,Genero,Lugar,Precio")] Concierto concierto)
        {
            if (id != concierto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await context.Modificar(id,concierto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConciertoExists(concierto.Id).Result)
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
            return View(concierto);
        }

        // GET: Conciertoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await context.DameTodos();
            var context1 = vista.Select(x => x);
            var concierto = context1.FirstOrDefault(m => m.Id == id);
            if (concierto == null)
            {
                return NotFound();
            }

            return View(concierto);
        }

        // POST: Conciertoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concierto = await context.DameUno(id);
            if (concierto != null)
            {
                await context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ConciertoExists(int id)
        {
            var vista = await context.DameTodos();
            return vista.Exists(e => e.Id == id);
        }
    }
}
