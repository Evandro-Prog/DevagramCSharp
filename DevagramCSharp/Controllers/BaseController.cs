using DevagramCSharp.Models;
using DevagramCSharp.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevagramCSharp.Controllers
{
    [Authorize]
    public class BaseController : ControllerBase
    {       
        protected readonly IUsuarioRepository _usuarioRepository;

        public BaseController(IUsuarioRepository usuarioRepository)
        {           
            _usuarioRepository = usuarioRepository;
        }

        protected Usuario ReadToken()
        {
            //Busca o Id do usuario logado
            var idUser = User.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(c => c.Value).FirstOrDefault();

            if(string.IsNullOrEmpty(idUser))
            {
                return null;
            }
            else
            {
                return _usuarioRepository.GetUsuarioPorId(int.Parse(idUser));
            }
        }
    }
}
