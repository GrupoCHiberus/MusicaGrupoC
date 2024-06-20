using PruebaMVC.Models;

namespace PruebaMVC.Services.Specification
{
    public class AlbumSpecification (int AlbumId) : IAlbumSpecification
    {
        public bool IsValid(Albume elemento)
        {
            return elemento.Id == AlbumId;
        }
    }
}
