using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class ListasController(
        IGenericRepositorio<Lista> context,
        IGenericRepositorio<Usuario> contextUsuario)
        : Controller
    {
        // GET: Listas
        public async Task<IActionResult> Index()
        {
            var grupoCContext = await context.DameTodos();
            foreach (var item in grupoCContext)
            {
                if (item.UsuarioId != null) item.Usuario = await contextUsuario.DameUno((int)item.UsuarioId);
            }

            return View(grupoCContext);
        }

        // GET: Listas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await context.DameTodos();
            foreach (var item in vista)
            {
                if (item.UsuarioId != null) item.Usuario = await contextUsuario.DameUno((int)item.UsuarioId);
            }

            var lista = vista.FirstOrDefault(m => m.Id == id);
            if (lista == null)
            {
                return NotFound();
            }
            return View(lista);
        }

        // GET: Listas/Create
        public async Task<IActionResult> Create()
        {
            var contextoUsu = await contextUsuario.DameTodos();
            ViewData["UsuarioId"] = new SelectList(contextoUsu.OrderBy(x=>x.Nombre), "Id", "Nombre");
            return View();
        }

        // POST: Listas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,UsuarioId")] Lista lista)
        {
            if (ModelState.IsValid)
            {
                await context.Agregar(lista);
                return RedirectToAction(nameof(Index));
            }
            var contextoUsu = await contextUsuario.DameTodos();
            ViewData["UsuarioId"] = new SelectList(contextoUsu.OrderBy(x => x.Nombre), "Id", "Nombre", lista.UsuarioId);
            return View(lista);
        }

        // GET: Listas/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var lista = await context.DameUno(id);
            var vista = await context.DameTodos();
            var conjunto = vista.FirstOrDefault(x => x.Id == id);
            var contextoUsu = await contextUsuario.DameTodos();
            ViewData["UsuarioId"] = new SelectList(contextoUsu.OrderBy(x => x.Nombre), "Id", "Nombre", lista.UsuarioId);
            return View(conjunto);
        }

        // POST: Listas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,UsuarioId")] Lista lista)
        {
            if (id != lista.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await context.Modificar(id,lista);

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListaExists(lista.Id).Result)
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
            var contextoUsu = await contextUsuario.DameTodos();
            ViewData["UsuarioId"] = new SelectList(contextoUsu.OrderBy(x => x.Nombre), "Id", "Nombre", lista.UsuarioId);
            return View(lista);
        }

        // GET: Listas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var vista = await context.DameTodos();

            var lista = vista
                .FirstOrDefault(m => m.Id == id);
            if (lista == null)
            {
                return NotFound();
            }

            return View(lista);
        }

        // POST: Listas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await context.Borrar(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ListaExists(int id)
        {
            var vista = await context.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
