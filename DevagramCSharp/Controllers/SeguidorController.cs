using DevagramCSharp.Dtos;
using DevagramCSharp.Models;
using DevagramCSharp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SeguidorController : BaseController
    {
        private readonly ILogger<SeguidorController> _logger;
        private readonly ISeguidorRepository _seguidorRepository;

        public SeguidorController(ILogger<SeguidorController> logger,
            ISeguidorRepository seguidorRepository, IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _seguidorRepository = seguidorRepository;
        }

        [HttpPut]

        public IActionResult Seguir(int idSeguido)
        {
            try
            {
                Usuario usuarioseguido = _usuarioRepository.GetUsuarioPorId(idSeguido);
                Usuario usuarioseguidor = ReadToken();

                if (usuarioseguidor != null)
                {
                    Seguidor seguidor = _seguidorRepository.GetSeguidor(usuarioseguidor.Id, usuarioseguido.Id);
                    if(seguidor != null)
                    {
                        _seguidorRepository.Desseguir(seguidor);
                    }
                    else
                    {
                        Seguidor seguidornovo = new Seguidor()
                        {
                            IdUsuarioSeguido = usuarioseguido.Id,
                            IdUsuarioSeguidor = usuarioseguidor.Id
                        };
                        _seguidorRepository.Seguir(seguidornovo);
                    }
                                       
                }
                return Ok("Ação realizada com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao seguir/deixar de seguir usuário");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
