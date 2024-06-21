using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class AlbumesController(
        IGenericRepositorio<Albume> context,
        IGenericRepositorio<Grupo> contextGrupo,
        IGenericRepositorio<VistaAlbume> contextVista)
        : Controller
    {
        // GET: Albumes
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["IDSortParm"] = sortOrder == "Grupos" ? "grupos_desc" : "Grupos";
            ViewData["FechaSortParm"] = sortOrder == "Fecha" ? "fecha_desc" : "Fecha";

            var vista = await contextVista.DameTodos();
            var conjunto = vista.Select(x => x);
            if (!String.IsNullOrEmpty(searchString))
            {
                conjunto = conjunto.Where(s => s.Titulo!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    conjunto = conjunto.OrderByDescending(s => s.Titulo);
                    break;
                case "Genero":
                    conjunto = conjunto.OrderBy(s => s.Genero);
                    break;
                case "genero_desc":
                    conjunto = conjunto.OrderByDescending(s => s.Genero);
                    break;
                case "Grupos":
                    conjunto = conjunto.OrderBy(s => s.NombreGrupo);
                    break;
                case "grupos_desc":
                    conjunto = conjunto.OrderByDescending(s => s.NombreGrupo);
                    break;
                case "Fecha":
                    conjunto = conjunto.OrderBy(s => s.Fecha);
                    break;
                case "fecha_desc":
                    conjunto = conjunto.OrderByDescending(s => s.Fecha);
                    break;
                default:
                    conjunto = conjunto.OrderBy(s => s.Titulo);
                    break;
            }

            return View(conjunto);
        }

        public async Task<IActionResult> IndexConsulta(string sortOrder, string searchString)
        {
           
           
            ViewData["TituloSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["GeneroSortParm"] = sortOrder == "Genero" ? "genero_desc" : "Genero";
            ViewData["IDSortParm"] = sortOrder == "Grupos" ? "grupos_desc" : "Grupos";
            ViewData["FechaSortParm"] = sortOrder == "Fecha" ? "fecha_desc" : "Fecha";
            var letra = 'u';
            var vista = await contextVista.DameTodos();
            var conjunto = vista.Select(x => x).
                Where(x => x.Genero == "Heavy Metal" && x.Titulo != null && x.Titulo.Contains(letra));
            if (!String.IsNullOrEmpty(searchString))
            {
                conjunto = conjunto.Where(s => s.Titulo!.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    conjunto = conjunto.OrderByDescending(s => s.Titulo);
                    break;
                case "Genero":
                    conjunto = conjunto.OrderBy(s => s.Genero);
                    break;
                case "genero_desc":
                    conjunto = conjunto.OrderByDescending(s => s.Genero);
                    break;
                case "Grupos":
                    conjunto = conjunto.OrderBy(s => s.NombreGrupo);
                    break;
                case "grupos_desc":
                    conjunto = conjunto.OrderByDescending(s => s.NombreGrupo);
                    break;
                case "Fecha":
                    conjunto = conjunto.OrderBy(s => s.Fecha);
                    break;
                case "fecha_desc":
                    conjunto = conjunto.OrderByDescending(s => s.Fecha);
                    break;
                default:
                    conjunto = conjunto.OrderBy(s => s.Titulo);
                    break;
            }

            return View(conjunto);
        }

        // GET: Albumes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var conjunto = await contextVista.DameTodos();
            var albume = conjunto.FirstOrDefault(x => x.Id == id);
            if (albume == null)
            {
                return NotFound();
            }

            return View(albume);
        }

        // GET: Albumes/Create
        public async Task<IActionResult> Create()
        {
            var contextGru = await contextGrupo.DameTodos();
            ViewData["GruposId"] = new SelectList(contextGru.OrderBy(x=>x.Nombre), "Id", "Nombre");
            return View();
        }

        // POST: Albumes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Fecha,Genero,Titulo,GruposId")] Albume albume)
        {
            if (ModelState.IsValid)
            {
                await context.Agregar(albume);
                return RedirectToAction(nameof(Index));
            }
            var contextGru = await contextGrupo.DameTodos();
            ViewData["GruposId"] = new SelectList(contextGru.OrderBy(x => x.Nombre), "Id", "Nombre", albume.GruposId);
            return View(albume);
        }

        // GET: Albumes/Edit/5
        public async Task<IActionResult> Edit(int id)
        {

            var albume = await context.DameUno(id);
            var vista = await contextVista.DameTodos();
            var conjunto = vista.FirstOrDefault(x => x.Id == id);
            var contextGru = await contextGrupo.DameTodos();
            ViewData["GruposId"] = new SelectList(contextGru.OrderBy(x => x.Nombre), "Id", "Nombre", albume.GruposId);
            return View(conjunto);
        }

        // POST: Albumes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Fecha,Genero,Titulo,GruposId")] Albume albume)
        {
            if (id != albume.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                   await context.Modificar(id, albume);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AlbumeExists(albume.Id).Result)
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
            var contextGru = await contextGrupo.DameTodos();
            ViewData["GruposId"] = new SelectList(contextGru.OrderBy(x => x.Nombre), "Id", "Nombre", albume.GruposId);
            return View(conjunto);
        }

        // GET: Albumes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
         
            var vista = await contextVista.DameTodos();
            var albume =vista.FirstOrDefault(x => x.Id == id);
            if (albume == null)
            {
                return NotFound();
            }

            return View(albume);
        }

        // POST: Albumes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var albume = await context.DameUno(id);
            if (albume != null)
            {
               await context.Borrar(id);
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> AlbumeExists(int id)
        {
            var vista = await context.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
