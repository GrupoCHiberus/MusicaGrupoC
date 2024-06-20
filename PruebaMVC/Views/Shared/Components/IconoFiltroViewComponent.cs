using Microsoft.AspNetCore.Mvc;

namespace PruebaMVC.Views.Shared.Components
{
    public class IconoFiltroViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync()
        {
            return Task.FromResult<IViewComponentResult>(View());
        }
    }
}
