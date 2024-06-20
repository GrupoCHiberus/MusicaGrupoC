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
        public async Task<IActionResult> Index()
        {
            var grupoCContext = await contextVista.DameTodos();
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
            ViewData[DataCanciones] = new SelectList(await contextCancione.DameTodos(), "Id", DataComboTitulo);
            ViewData[DataConciertos] = new SelectList(await contextConcierto.DameTodos(), "Id", DataComboTitulo);
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
            ViewData[DataCanciones] = new SelectList(await contextCancione.DameTodos(), "Id", DataComboTitulo, cancionesConcierto.CancionesId);
            ViewData[DataConciertos] = new SelectList(await contextConcierto.DameTodos(), "Id", DataComboTitulo, cancionesConcierto.ConciertosId);
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
            ViewData[DataCanciones] = new SelectList(await contextCancione.DameTodos(), "Id", DataComboTitulo, cancionesConcierto.CancionesId);
            ViewData[DataConciertos] = new SelectList(await contextConcierto.DameTodos(), "Id", DataComboTitulo, cancionesConcierto.ConciertosId);
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
            ViewData[DataCanciones] = new SelectList(await contextCancione.DameTodos(), "Id", DataComboTitulo, cancionesConcierto.CancionesId);
            ViewData[DataConciertos] = new SelectList(await contextConcierto.DameTodos(), "Id", DataComboTitulo, cancionesConcierto.ConciertosId);
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