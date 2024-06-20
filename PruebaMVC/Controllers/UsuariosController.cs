using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Controllers
{
    public class UsuariosController(IGenericRepositorio<Usuario> context) : Controller
    {
        // GET: Usuarios
        public async Task<IActionResult> Index(string sortOrder, string searchString, string mostrar ="Ocultar")
        {
            ViewData["NombreSortParm"] = String.IsNullOrEmpty(sortOrder) ? "nombre_desc" : "";
            ViewData["EmailSortParm"] = sortOrder == "Email" ? "email_desc" : "Email";
            ViewData["ContraseñaSortParm"] = sortOrder == "Contraseña" ? "contraseña_desc" : "Contraseña";

            if (mostrar.Equals("Mostrar"))
            {
                ViewData["Esconder"] = "No";
            }
            if (mostrar.Equals("Ocultar"))
            {
                ViewData["Esconder"] = "Si";
            }
            var vista = await context.DameTodos();
            var usuarios = vista.Select(x => x);

            if (!String.IsNullOrEmpty(searchString))
            {
                usuarios = usuarios.Where(s => s.Nombre != null && s.Nombre.Contains(searchString));
            }

            usuarios = sortOrder switch
            {
                "nombre_desc" => usuarios.OrderByDescending(s => s.Nombre),
                "Email" => usuarios.OrderBy(s => s.Email),
                "email_desc" => usuarios.OrderByDescending(s => s.Email),
                "Contraseña" => usuarios.OrderBy(s =>s.Contraseña),
                "contraseña_desc" => usuarios.OrderByDescending(s =>s.Contraseña),
                 _ => usuarios.OrderBy(s => s.Nombre)

            };
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var usuario = context.DameUno(id);
          
            return View(await usuario);
        }

        // GET: Usuarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Usuarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Email,Contraseña")] Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                await context.Agregar(usuario);
                return RedirectToAction(nameof(Index));
            }
            return View(usuario);
        }

        // GET: Usuarios/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var usuario = await context.DameUno(id);

            return View(usuario);
        }

        // POST: Usuarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Email,Contraseña")] Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await context.Modificar(id, usuario);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(usuario.Id).Result)
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
            return View(usuario);
        }

        // GET: Usuarios/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var usuario = await context.DameUno(id);

            return View(usuario);
        }

        // POST: Usuarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await context.Borrar(id);
         return RedirectToAction(nameof(Index));
        }

        private async Task<bool> UsuarioExists(int id)
        {
            var vista = await context.DameTodos();
            return vista.Any(e => e.Id == id);
        }
    }
}
