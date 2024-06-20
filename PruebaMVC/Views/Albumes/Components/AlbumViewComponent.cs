using Microsoft.AspNetCore.Mvc;
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;
using PruebaMVC.Services.Specification;

namespace PruebaMVC.Views.Albumes.Component
{
    public class AlbumViewComponent(IGenericRepositorio<Albume> albumes) : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int AlbumId)
        {
            var items = await albumes.DameTodos();
            IAlbumSpecification especificacion = new AlbumSpecification(AlbumId);
            var itemsFiltrados = items.Where(especificacion.IsValid);
            return View(itemsFiltrados);
        }
    }
}
