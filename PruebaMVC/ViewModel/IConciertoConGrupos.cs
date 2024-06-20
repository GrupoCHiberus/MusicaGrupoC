namespace PruebaMVC.ViewModel
{
    public interface IConciertoConGrupos
    {
        public Task<List<ConciertoConListaGrupos>> dameListaDeConciertoConGrupos(int conciertoId);
    }
}
