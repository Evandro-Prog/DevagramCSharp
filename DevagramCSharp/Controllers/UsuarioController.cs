using DevagramCSharp.Dtos;
using DevagramCSharp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCSharp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : BaseController
    {
        public readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        [HttpGet]        
        public IActionResult ObterUsuario()
        {
            try
            {
                Usuario usuario = new Usuario()
                {
                    Email = "evandro@evandro.com.br",
                    Nome = "Evandro",
                    Id = 12
                };

                return Ok(usuario);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao obter usuário");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }           

    }
}
