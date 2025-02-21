using DevagramCSharp.Dtos;
using DevagramCSharp.Models;

namespace DevagramCSharp.Repository
{
    public interface IPublicacaoRepository
    {
        List<PublicacaoFeedRespostaDto> GetPublicacoesFeed(int idUsuario);
        public void publicar(Publicacao publicacao);
    }
}
