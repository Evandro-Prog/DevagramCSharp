using DevagramCSharp.Dtos;
using DevagramCSharp.Models;

namespace DevagramCSharp.Repository.Impl
{
    public class PublicacaorepositoryImpl : IPublicacaoRepository
    {
        private readonly DevagramContext _context;

        public PublicacaorepositoryImpl(DevagramContext context)
        {
            _context = context;
        }

        public List<PublicacaoFeedRespostaDto> GetPublicacoesFeed(int idUsuario) // querry para inner join
        {
            var feed =
                from publicacoes in _context.Publicacoes
                join seguidores in _context.Seguidores on publicacoes.IdUsuario equals seguidores.IdUsuarioSeguido
                where seguidores.IdUsuarioSeguidor == idUsuario
                select new PublicacaoFeedRespostaDto
                {
                    IdPublicacao = publicacoes.Id,
                    Descricao = publicacoes.Descricao,
                    Foto = publicacoes.Foto,
                    IdUsuario = publicacoes.IdUsuario,
                };

            return feed.ToList();
        }

        public void publicar(Publicacao publicacao)
        {
            _context.Add(publicacao);
            _context.SaveChanges();
        }
    }
}
