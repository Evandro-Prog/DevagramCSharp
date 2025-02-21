using DevagramCSharp.Models;

namespace DevagramCSharp.Repository.Impl
{
    public class UsuarioRepositoryImpl : IUsuarioRepository
    {

        private readonly DevagramContext _context;

        public UsuarioRepositoryImpl(DevagramContext context)
        {
            _context = context;
        }

        public void AtualizarUsuario(Usuario usuario) // Atualiza dados do usuario logado
        {
            _context.Update(usuario);
            _context.SaveChanges();
        }

        public Usuario GetUsuarioPorId(int id) // busca usuario por ID
        {
            return _context.Usuarios.FirstOrDefault(u => u.Id == id);
        }

        public Usuario GetUsuarioPorLoginSenha(string email, string senha) // procura usuario no banco e realiza o login
        {
            return _context.Usuarios.FirstOrDefault(u => u.Email == email && u.Senha == senha);
        }

        public void Salvar(Usuario usuario) // cria usuario no sistema
        {
            _context.Add(usuario);
            _context.SaveChanges();
        }

        public bool VerificarEmail(string email) // verifica se já existe email cadastrado no sistema
        {
            return _context.Usuarios.Any(u => u.Email == email);
        }
    }
}
