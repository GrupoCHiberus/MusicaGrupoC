using Microsoft.AspNetCore.Mvc;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.Views.Shared.Components
{
    public class ConciertoCarouselViewComponent(IGenericRepositorio<Concierto> contextConcierto) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var concierto = await contextConcierto.DameTodos();
            
            return View(concierto);
        }
    }
}
