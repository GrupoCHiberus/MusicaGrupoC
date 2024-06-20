using Microsoft.AspNetCore.Mvc;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Views.Shared.Components
{
    public class ComboBoxGruposViewComponent(IGenericRepositorio<Grupo> contextGrupos) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await contextGrupos.DameTodos());
        }
    }
}
