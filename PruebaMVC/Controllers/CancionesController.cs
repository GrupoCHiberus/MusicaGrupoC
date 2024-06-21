using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class CancionesController(
        IGenericRepositorio<Cancione> context,
        IGenericRepositorio<Albume> contextAlbume,
        IGenericRepositorio<VistaCancione> contextVista)
        : Controller
    {
        public const string AlbumId = "AlbumesId";
        // GET: Canciones
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            var grupoCContext = await contextVista.DameTodos();
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "titulo_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["FechaSortParm"] = sortOrder == "Fecha" ? "fecha-desc" : "Fecha";
            ViewData["AlbumesSortParm"] = sortOrder == "Albumes" ? "albumes_desc" : "Albumes";
            ViewData["FechaSortParm"] = sortOrder == "Fecha" ? "fecha_desc" : "Fecha";

            if (grupoCContext == null)
            {
                return Problem("Es nulo");
            }
            var canciones = from m in grupoCContext
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                canciones = canciones.Where(s => s.Titulo!.Contains(searchString));
            }

            canciones = sortOrder switch
            {
                "titulo_desc" => canciones.OrderByDescending(s => s.Titulo),
                "Genero" => canciones.OrderBy(s => s.Genero),
                "genero_desc" => canciones.OrderByDescending(s => s.Genero),
                "Fecha" => canciones.OrderBy(s => s.Fecha),
                "fecha-desc" => canciones.OrderByDescending(s => s.Fecha),
                "Albumes" => canciones.OrderBy(s => s.TituloAlbum),
                "albumes_desc" => canciones.OrderByDescending(s => s.TituloAlbum),
                _ => canciones.OrderBy(s => s.Titulo)
            };
            return View(canciones);
            
        }

        // GET: Canciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await contextVista.DameTodos();
            var cancione = vista
                .Find(m => m.Id == id);
            if (cancione == null)
            {
                return NotFound();
            }

            return View(cancione);
        }

        // GET: Canciones/Create
        public async Task<IActionResult> Create()
        {
            var contextAlb = await contextAlbume.DameTodos();
            ViewData[AlbumId] = new SelectList(contextAlb.OrderBy(x=>x.Titulo), "Id", "Titulo");
            return View();
        }

        // POST: Canciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Titulo,Genero,Fecha,AlbumesId")] Cancione cancione)
        {
            if (ModelState.IsValid)
            {
                await context.Agregar(cancione);
                return RedirectToAction(nameof(Index));
            }
            var contextAlb = await contextAlbume.DameTodos();
            ViewData[AlbumId] = new SelectList(contextAlb.OrderBy(x => x.Titulo), "Id", "Titulo", cancione.AlbumesId);
            return View(cancione);
        }

        // GET: Canciones/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var conjunto = await contextVista.DameTodos();
            var cancione = conjunto.Find(x => x.Id == id);
            if (cancione == null)
            {
                return NotFound();
            }
            var contextAlb = await contextAlbume.DameTodos();
            ViewData[AlbumId] = new SelectList(contextAlb.OrderBy(x => x.Titulo), "Id", "Titulo", cancione.AlbumesId);
            return View(cancione);
        }

        // POST: Canciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Titulo,Genero,Fecha,AlbumesId")] Cancione cancione)
        {
            if (id != cancione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await context.Modificar(id,cancione);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CancioneExists(cancione.Id).Result)
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
            var vista = await contextVista.DameTodos();
            var conjunto = vista.Find(x => x.Id == id);
            var contextAlb = await contextAlbume.DameTodos();
            ViewData[AlbumId] = new SelectList(contextAlb.OrderBy(x => x.Titulo), "Id", "Titulo", cancione.AlbumesId);
            return View(conjunto);
        }

        // GET: Canciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await contextVista.DameTodos();
            var cancione = vista.Find(m => m.Id == id);
            if (cancione == null)
            {
                return NotFound();
            }

            return View(cancione);
        }

        // POST: Canciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cancione = await context.DameUno(id);
            if (cancione != null)
            {
               await context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CancioneExists(int id)
        {
            var vista = await context.DameTodos();
            return vista.Exists(e => e.Id == id);
        }
    }
}
