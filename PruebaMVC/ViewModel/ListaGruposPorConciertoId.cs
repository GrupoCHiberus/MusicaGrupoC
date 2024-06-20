using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.ViewModel
{
    public class ListaGruposPorConciertoId(IGenericRepositorio<Concierto> contextConcierto,
        IGenericRepositorio<ConciertosGrupo> contextConciertoGrupo, IGenericRepositorio<Grupo> contextGrupo) : IListaGruposPorConciertoId
    {
        public async Task<List<ConciertoConGruposcs>> DameGrupos(int conciertoId)
        {
            var datosComponente = from c in (await contextConcierto.DameTodos()).AsParallel()
                join gc in (await contextConciertoGrupo.DameTodos()).AsParallel() on c.Id equals gc.ConciertosId
                join g in (await contextGrupo.DameTodos()).AsParallel() on gc.GruposId equals g.Id
                where c.Id == conciertoId
                select new ConciertoConGruposcs()
                {
                    Fecha = c.Fecha,
                    Genero = c.Genero,
                    Lugar = c.Lugar,
                    Titulo = c.Titulo,
                    Precio = c.Precio,
                    GruposId = gc.GruposId,
                    ConciertosId = gc.ConciertosId,
                    Nombre = g.Nombre
                };

            return datosComponente.ToList();
        }
    }
}
