using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class ConciertosGrupoesController(
        IGenericRepositorio<ConciertosGrupo> context,
        IGenericRepositorio<Concierto> contextConcierto,
        IGenericRepositorio<Grupo> contextGrupo,
        IGenericRepositorio<VistaConciertosGrupo> contextVista)
        : Controller
    {
        private const string DataConciertos = "ConciertosId";
        private const string DataGrupos = "GruposId";
        private const string DataComboTitulo = "Titulo";
        private const string DataComboNombre = "Nombre";

        // GET: ConciertosGrupoes
        public async Task<IActionResult> Index()
        {
            var grupoCContext = await contextVista.DameTodos();
            return View(grupoCContext);
        }

        // GET: ConciertosGrupoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await contextVista.DameTodos();
            var conciertosGrupo = vista.AsParallel().FirstOrDefault(m => m.Id == id);
            if (conciertosGrupo == null)
            {
                return NotFound();
            }

            return View(conciertosGrupo);
        }

        // GET: ConciertosGrupoes/Create
        public async Task<IActionResult> Create()
        {
            ViewData[DataConciertos] = new SelectList(await contextConcierto.DameTodos(), "Id", DataComboTitulo);
            ViewData[DataGrupos] = new SelectList(await contextGrupo.DameTodos(), "Id", DataComboNombre);
            return View();
        }

        // POST: ConciertosGrupoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,GruposId,ConciertosId")] ConciertosGrupo conciertosGrupo)
        {
            if (ModelState.IsValid)
            {
                await context.Agregar(conciertosGrupo);
                return RedirectToAction(nameof(Index));
            }
            ViewData[DataConciertos] = new SelectList(await contextConcierto.DameTodos(), "Id", DataComboTitulo, conciertosGrupo.ConciertosId);
            ViewData[DataGrupos] = new SelectList(await contextGrupo.DameTodos(), "Id", DataComboNombre, conciertosGrupo.GruposId);
            return View(conciertosGrupo);
        }

        // GET: ConciertosGrupoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var conciertosGrupo = await context.DameUno((int)id);

            var vista = await contextVista.DameTodos();
            var conjunto = vista.AsParallel().FirstOrDefault(x => x.Id == id);
            ViewData[DataConciertos] = new SelectList(await contextConcierto.DameTodos(), "Id", DataComboTitulo, conciertosGrupo.ConciertosId);
            ViewData[DataGrupos] = new SelectList(await contextGrupo.DameTodos(), "Id", DataComboNombre, conciertosGrupo.GruposId);
            return View(conjunto);
        }

        // POST: ConciertosGrupoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,GruposId,ConciertosId")] ConciertosGrupo conciertosGrupo)
        {
            if (id != conciertosGrupo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await context.Modificar(id, conciertosGrupo);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConciertosGrupoExists(conciertosGrupo.Id).Result)
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
            ViewData[DataConciertos] = new SelectList(await contextConcierto.DameTodos(), "Id", DataComboTitulo, conciertosGrupo.ConciertosId);
            ViewData[DataGrupos] = new SelectList(await contextGrupo.DameTodos(), "Id", DataComboNombre, conciertosGrupo.GruposId);
            return View(conjunto);
        }

        // GET: ConciertosGrupoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vista = await contextVista.DameTodos();
            var conciertosGrupo = vista.AsParallel()
                .FirstOrDefault(m => m.Id == id);


            return View(conciertosGrupo);
        }

        // POST: ConciertosGrupoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

                await context.Borrar(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ConciertosGrupoExists(int id)
        {
            var vista = await context.DameTodos();
            return vista.AsParallel().Any(e => e.Id == id);
        }
    }
}