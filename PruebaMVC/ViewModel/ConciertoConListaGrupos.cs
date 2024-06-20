namespace PruebaMVC.ViewModel
{
    public class ConciertoConListaGrupos
    {
        public List<ConciertoConGruposcs> listaGrupo { get; set; }
        public string? Genero { get; set; }

        public string? Lugar { get; set; }

        public string? Titulo { get; set; }

        public decimal? Precio { get; set; }

        public DateTime? Fecha { get; set; }
    }
}
