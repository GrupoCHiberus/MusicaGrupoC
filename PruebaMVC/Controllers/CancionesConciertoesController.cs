using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class CancionesConciertoesController(
        IGenericRepositorio<CancionesConcierto> context,
        IGenericRepositorio<Cancione> contextCancione,
        IGenericRepositorio<Concierto> contextConcierto,
        IGenericRepositorio<VistaCancionConcierto> contextVista)
        : Controller
    {
        private const string DataCanciones = "CancionesId";
        private const string DataConciertos = "ConciertosId";
        private const string DataComboTitulo = "Titulo";

        // GET: CancionesConciertoes
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["CancionesSortParm"] = sortOrder == "TituloCanciones" ? "canciones_desc" : "TituloCanciones";
            ViewData["TituloSortParm"] = sortOrder == "Titulo" ? "titulo_desc" : "Titulo";
            var grupoCContext = await contextVista.DameTodos();

            switch (sortOrder)
            {
                case "canciones_desc":
                    grupoCContext = grupoCContext.OrderByDescending(s => s.TituloCanciones).ToList();
                    break;
                case "Titulo":
                    grupoCContext = grupoCContext.OrderBy(s => s.Titulo).ToList();
                    break;
                case "titulo_desc":
                    grupoCContext = grupoCContext.OrderByDescending(s => s.Titulo).ToList();
                    break;
                default:
                    grupoCContext = grupoCContext.OrderBy(s => s.TituloCanciones).ToList();
                    break;
            }

            return View(grupoCContext);
        }

        // GET: CancionesConciertoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await contextVista.DameTodos();
            var cancionesConcierto = vista.AsParallel().FirstOrDefault(m => m.Id == id);

            return View(cancionesConcierto);
        }

        // GET: CancionesConciertoes/Create
        public async Task<IActionResult> Create()
        {
            var contextCan = await contextCancione.DameTodos();
            var contextConci = await contextConcierto.DameTodos();
            ViewData[DataCanciones] = new SelectList(contextCan.OrderBy(x=>x.Titulo), "Id", DataComboTitulo);
            ViewData[DataConciertos] = new SelectList(contextConci.OrderBy(x=>x.Titulo), "Id", DataComboTitulo);
            return View();
        }

        // POST: CancionesConciertoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CancionesId,ConciertosId")] CancionesConcierto cancionesConcierto)
        {
            if (ModelState.IsValid)
            {
                await context.Agregar(cancionesConcierto);
                return RedirectToAction(nameof(Index));
            }
            var contextCan = await contextCancione.DameTodos();
            var contextConci = await contextConcierto.DameTodos();
            ViewData[DataCanciones] = new SelectList(contextCan.OrderBy(x => x.Titulo), "Id", DataComboTitulo, cancionesConcierto.CancionesId);
            ViewData[DataConciertos] = new SelectList(contextConci.OrderBy(x => x.Titulo), "Id", DataComboTitulo, cancionesConcierto.ConciertosId);
            return View(cancionesConcierto);
        }

        // GET: CancionesConciertoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cancionesConcierto = await context.DameUno((int)id);

            var vista = await contextVista.DameTodos();
            var conjunto = vista.AsParallel().FirstOrDefault(x => x.Id == id);
            var contextCan = await contextCancione.DameTodos();
            var contextConci = await contextConcierto.DameTodos();
            ViewData[DataCanciones] = new SelectList(contextCan.OrderBy(x => x.Titulo), "Id", DataComboTitulo, cancionesConcierto.CancionesId);
            ViewData[DataConciertos] = new SelectList(contextConci.OrderBy(x => x.Titulo), "Id", DataComboTitulo, cancionesConcierto.ConciertosId);
            return View(conjunto);
        }

        // POST: CancionesConciertoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CancionesId,ConciertosId")] CancionesConcierto cancionesConcierto)
        {
            if (id != cancionesConcierto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await context.Modificar(id, cancionesConcierto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CancionesConciertoExists(cancionesConcierto.Id).Result)
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
            var conjunto = vista.AsParallel().FirstOrDefault(x => x.Id == id);
            var contextCan = await contextCancione.DameTodos();
            var contextConci = await contextConcierto.DameTodos();
            ViewData[DataCanciones] = new SelectList(contextCan.OrderBy(x => x.Titulo), "Id", DataComboTitulo, cancionesConcierto.CancionesId);
            ViewData[DataConciertos] = new SelectList(contextConci.OrderBy(x => x.Titulo), "Id", DataComboTitulo, cancionesConcierto.ConciertosId);
            return View(conjunto);
        }

        // GET: CancionesConciertoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await contextVista.DameTodos();
            var cancionesConcierto = vista.AsParallel().FirstOrDefault(m => m.Id == id);
            if (cancionesConcierto == null)
            {
                return NotFound();
            }

            return View(cancionesConcierto);
        }

        // POST: CancionesConciertoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

                await context.Borrar(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CancionesConciertoExists(int id)
        {
            var vista = await context.DameTodos();
            return vista.AsParallel().Any(e => e.Id == id);
        }
    }
}