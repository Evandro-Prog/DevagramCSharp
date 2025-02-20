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

        public void publicar(Publicacao publicacao)
        {
            _context.Add(publicacao);
            _context.SaveChanges();
        }
    }
}
