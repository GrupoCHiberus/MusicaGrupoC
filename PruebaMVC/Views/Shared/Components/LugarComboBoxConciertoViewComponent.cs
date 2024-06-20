using Microsoft.AspNetCore.Mvc;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Views.Shared.Components
{
    public class LugarComboBoxConciertoViewComponent(IGenericRepositorio<Concierto> contextConcierto) :ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var concierto = await contextConcierto.DameTodos();
            List<Concierto> lugaresDistintos = concierto.GroupBy(l => l.Lugar).Select(ld => ld.First()).ToList();

            return View(lugaresDistintos);
        }
    }
}
