
using PruebaMVC.Models;
using PruebaMVC.Services.Repositorio;

namespace PruebaMVC.ViewModel
{
    public class GruposPorConcierto (IGenericRepositorio<Concierto> _contextConcierto,
        IGenericRepositorio<ConciertosGrupo> _contextConciertoGrupo, IGenericRepositorio<Grupo> _contextGrupo) : IConciertoConGrupos
    {
        public async Task<List<ConciertoConListaGrupos>> dameListaDeConciertoConGrupos(int conciertoId)
        {
            var conciertos = from c in (await _contextConcierto.DameTodos()).AsParallel() where c.Id == conciertoId select c;
            ConciertoConListaGrupos listaDeConcierto = new ConciertoConListaGrupos();
            foreach (var concierto in conciertos)
            {
                listaDeConcierto = new ConciertoConListaGrupos()
                {
                    Titulo = concierto.Titulo,
                    Genero = concierto.Genero,
                    Lugar = concierto.Lugar,
                    Precio = concierto.Precio,
                    Fecha = concierto.Fecha,
                    listaGrupo = await new ListaGruposPorConciertoId(_contextConcierto, _contextConciertoGrupo, _contextGrupo)
                        .DameGrupos(conciertoId)
                };
            }

            List<ConciertoConListaGrupos> listaDeGruposPorConcierto = new ();
            listaDeGruposPorConcierto.Add(listaDeConcierto);
            return listaDeGruposPorConcierto;
        }
    }
}
