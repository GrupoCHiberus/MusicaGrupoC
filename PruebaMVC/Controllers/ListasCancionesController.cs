using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class ListasCancionesController(
        IGenericRepositorio<ListasCancione> context,
        IGenericRepositorio<Cancione> contextCanciones,
        IGenericRepositorio<Lista> contextListas,
        IGenericRepositorio<VistaListaCancione> contextVista)
        : Controller
    {
        private const string DataCanciones = "CancionesId"; 
        private const string DataLista = "ListasId"; 
        private const string DataComboTitulo = "Titulo"; 
        private const string DataComboNombre = "Nombre";

        // GET: ListasCanciones
        public async Task<IActionResult> Index()
        {
            var grupoCContext = await contextVista.DameTodos();
            return View(grupoCContext);
        }

        // GET: ListasCanciones/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await contextVista.DameTodos();
            var listasCancione = vista.AsParallel()
                .FirstOrDefault(m => m.Id == id);


            return View(listasCancione);
        }

        // GET: ListasCanciones/Create
        public async Task<IActionResult> Create()
        {
            var contextoCan = await contextCanciones.DameTodos();
            var contextoLis = await contextListas.DameTodos();
            ViewData[DataCanciones] = new SelectList(contextoCan.OrderBy(x=>x.Titulo), "Id", DataComboTitulo);
            ViewData[DataLista] = new SelectList(contextoLis.OrderBy(x=>x.Nombre), "Id", DataComboNombre);
            return View();
        }

        // POST: ListasCanciones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ListasId,CancionesId")] ListasCancione listasCancione)
        {
            if (ModelState.IsValid)
            {
                await context.Agregar(listasCancione);
                return RedirectToAction(nameof(Index));
            }
            var contextoCan = await contextCanciones.DameTodos();
            var contextoLis = await contextListas.DameTodos();
            ViewData[DataCanciones] = new SelectList(contextoCan.OrderBy(x => x.Titulo), "Id", DataComboTitulo, listasCancione.CancionesId);
            ViewData[DataLista] = new SelectList(contextoLis.OrderBy(x => x.Nombre), "Id", DataComboNombre, listasCancione.ListasId);
            return View(listasCancione);
        }

        // GET: ListasCanciones/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listasCancione = await context.DameUno((int)id);

            var vista = await contextVista.DameTodos();
            var conjunto = vista.AsParallel().FirstOrDefault(x => x.Id == id);
            var contextoCan = await contextCanciones.DameTodos();
            var contextoLis = await contextListas.DameTodos();
            ViewData[DataCanciones] = new SelectList(contextoCan.OrderBy(x => x.Titulo), "Id", DataComboTitulo, listasCancione.CancionesId);
            ViewData[DataLista] = new SelectList(contextoLis.OrderBy(x => x.Nombre), "Id", DataComboNombre, listasCancione.ListasId);
            return View(conjunto);
        }

        // POST: ListasCanciones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ListasId,CancionesId")] ListasCancione listasCancione)
        {
            if (id != listasCancione.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await context.Modificar(id, listasCancione);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListasCancioneExists(listasCancione.Id).Result)
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
            var contextoCan = await contextCanciones.DameTodos();
            var contextoLis = await contextListas.DameTodos();
            ViewData[DataCanciones] = new SelectList(contextoCan.OrderBy(x => x.Titulo), "Id", DataComboTitulo, listasCancione.CancionesId);
            ViewData[DataLista] = new SelectList(contextoLis.OrderBy(x => x.Nombre), "Id", DataComboNombre, listasCancione.ListasId);
            return View(conjunto);
        }

        // GET: ListasCanciones/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await contextVista.DameTodos();
            var listasCanciones = vista.AsParallel().FirstOrDefault(m => m.Id == id);


            return View(listasCanciones);
        }

        // POST: ListasCanciones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await context.Borrar(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ListasCancioneExists(int id)
        {
            var vista = await context.DameTodos();
            return vista.AsParallel().Any(e => e.Id == id);
        }
    }
}