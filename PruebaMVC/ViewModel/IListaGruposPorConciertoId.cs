namespace PruebaMVC.ViewModel
{
    public interface IListaGruposPorConciertoId
    {
        public Task<List<ConciertoConGruposcs>> DameGrupos(int conciertoId);
    }
}
