using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class GruposArtistasController(
        IGenericRepositorio<GruposArtista> context,
        IGenericRepositorio<Artista> contextArtista,
        IGenericRepositorio<Grupo> contextGrupo,
        IGenericRepositorio<VistaGruposArtista> contextVista)
        : Controller
    {
        // GET: GruposArtistas
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["ArtistaSortParm"] = sortOrder == "NombreArtista" ? "artista_desc" : "NombreArtista";
            ViewData["GrupoSortParm"] = sortOrder == "NombreGrupo" ? "grupo_desc" : "NombreGrupo";

            var grupoCContext = await contextVista.DameTodos();

            switch (sortOrder)
            {
                case "artista_desc":
                    grupoCContext = grupoCContext.OrderByDescending(s => s.NombreArtista).ToList();
                    break;
                case "NombreArtista":
                    grupoCContext = grupoCContext.OrderBy(s => s.NombreArtista).ToList();
                    break;
                case "grupo_desc":
                    grupoCContext = grupoCContext.OrderByDescending(s => s.NombreGrupo).ToList();
                    break;
                case "NombreGrupo":
                    grupoCContext = grupoCContext.OrderBy(s => s.NombreGrupo).ToList();
                    break;
            }

            return View(grupoCContext);
        }

        // GET: GruposArtistas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await contextVista.DameTodos();
            var gruposArtista =vista.FirstOrDefault(m => m.Id == id);
            if (gruposArtista == null)
            {
                return NotFound();
            }

            return View(gruposArtista);
        }

        // GET: GruposArtistas/Create
        public async Task<IActionResult> Create()
        {
            var contextArt = await contextArtista.DameTodos();
            var contextGru = await contextGrupo.DameTodos();
            ViewData["ArtistasId"] = new SelectList(contextArt.OrderBy(x=>x.Nombre), "Id", "Nombre");
            ViewData["GruposId"] = new SelectList(contextGru.OrderBy(x=>x.Nombre), "Id", "Nombre");
            return View();
        }

        // POST: GruposArtistas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ArtistasId,GruposId")] GruposArtista gruposArtista)
        {
            if (ModelState.IsValid)
            {
               await context.Agregar(gruposArtista);
                return RedirectToAction(nameof(Index));
            }
            var contextArt = await contextArtista.DameTodos();
            var contextGru = await contextGrupo.DameTodos();
            ViewData["ArtistasId"] = new SelectList(contextArt.OrderBy(x => x.Nombre), "Id", "Nombre", gruposArtista.ArtistasId);
            ViewData["GruposId"] = new SelectList(contextGru.OrderBy(x => x.Nombre), "Id", "Nombre", gruposArtista.GruposId);
            return View(gruposArtista);
        }

        // GET: GruposArtistas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var gruposArtista = await context.DameUno((int)id);
            if (gruposArtista == null)
            {
                return NotFound();
            }
            var vista = await contextVista.DameTodos();
            var conjunto = vista.FirstOrDefault(x => x.Id == id);
            var contextArt = await contextArtista.DameTodos();
            var contextGru = await contextGrupo.DameTodos();
            ViewData["ArtistasId"] = new SelectList(contextArt.OrderBy(x => x.Nombre), "Id", "Nombre", gruposArtista.ArtistasId);
            ViewData["GruposId"] = new SelectList(contextGru.OrderBy(x => x.Nombre), "Id", "Nombre", gruposArtista.GruposId);
            return View(conjunto);
        }

        // POST: GruposArtistas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ArtistasId,GruposId")] GruposArtista gruposArtista)
        {
            if (id != gruposArtista.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await context.Modificar(id,gruposArtista);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GruposArtistaExists(gruposArtista.Id).Result)
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
            var conjunto = vista.FirstOrDefault(x => x.Id == id);
            var contextArt = await contextArtista.DameTodos();
            var contextGru = await contextGrupo.DameTodos();
            ViewData["ArtistasId"] = new SelectList(contextArt.OrderBy(x => x.Nombre), "Id", "Nombre", gruposArtista.ArtistasId);
            ViewData["GruposId"] = new SelectList(contextGru.OrderBy(x => x.Nombre), "Id", "Nombre", gruposArtista.GruposId);
            return View(conjunto);
        }

        // GET: GruposArtistas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await contextVista.DameTodos();
            var gruposArtista = vista.FirstOrDefault(m => m.Id == id);
            if (gruposArtista == null)
            {
                return NotFound();
            }

            return View(gruposArtista);
        }

        // POST: GruposArtistas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var gruposArtista = await context.DameUno(id);
            if (gruposArtista != null)
            {
               await context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> GruposArtistaExists(int id)
        {
            var vista =await contextArtista.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
