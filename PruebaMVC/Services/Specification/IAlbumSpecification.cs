using PruebaMVC.Models;

namespace PruebaMVC.Services.Specification
{
    public interface IAlbumSpecification
    {
        bool IsValid(Albume elemento);
    }
}
