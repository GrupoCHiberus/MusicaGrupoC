using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class GrupoesController(IGenericRepositorio<Grupo> context) : Controller
    {
        // GET: Grupoes
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
            var vista = await context.DameTodos();
            var grupos = vista.Select(x=>x);
            if (!String.IsNullOrEmpty(searchString))
            {
                grupos = grupos.Where(s => s.Nombre!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "nombre_desc":
                    grupos = grupos.OrderByDescending(s => s.Nombre);
                    break;
                default:
                    grupos = grupos.OrderBy(s => s.Nombre);
                    break;
            }
            return View(grupos);
        }

        public async Task<IActionResult> IndexConArtistas(string sortOrder, string searchString)
        {
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";

            var vista = await context.DameTodos();
            var grupos = vista.Select(x => x);
            if (!String.IsNullOrEmpty(searchString))
            {
                grupos = grupos.Where(s => s.Nombre!.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "nombre_desc":
                    grupos = grupos.OrderByDescending(s => s.Nombre);
                    break;
                default:
                    grupos = grupos.OrderBy(s => s.Nombre);
                    break;
            }

            return View(grupos);
        }

        // GET: Grupoes/Details/5
            public async Task<IActionResult> Details(int? id)
        {
            

            var vista = await context.DameTodos();
            var grupo = vista
                .FirstOrDefault(m => m.Id == id);

            return View(grupo);
        }

        // GET: Grupoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Grupoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre")] Grupo grupo)
        {
            if (ModelState.IsValid)
            {
                await context.Agregar(grupo);
                return RedirectToAction(nameof(Index));
            }
            return View(grupo);
        }

        // GET: Grupoes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
           

            var grupo = await context.DameUno(id);
            
            return View(grupo);
        }

        // POST: Grupoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre")] Grupo grupo)
        {
            if (id != grupo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await context.Modificar(id,grupo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GrupoExists(grupo.Id).Result)
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
            return View(grupo);
        }

        // GET: Grupoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await context.DameTodos();
            var grupo = vista
                .FirstOrDefault(m => m.Id == id);
            if (grupo == null)
            {
                return NotFound();
            }

            return View(grupo);
        }

        // POST: Grupoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var grupo = await context.DameUno(id);
            if (grupo != null)
            {
                await context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> GrupoExists(int id)
        {
            var vista = await context.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
