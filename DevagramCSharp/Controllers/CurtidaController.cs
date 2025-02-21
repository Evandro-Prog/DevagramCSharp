using DevagramCSharp.Dtos;
using DevagramCSharp.Models;
using DevagramCSharp.Repository;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CurtidaController : BaseController
    {
        private readonly ILogger<CurtidaController> _logger;
        private readonly ICurtidaRepository _curtidaRepository;
        public CurtidaController(ILogger<CurtidaController> logger,
            ICurtidaRepository curtidaRepository, IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _curtidaRepository = curtidaRepository;
        }

        [HttpPut]  
        public IActionResult Curtir ([FromBody] CurtidaRequisicaoDto curtidadto)
        {
            try
            {
                if(curtidadto != null)
                {
                    Curtida curtida = _curtidaRepository.GetCurtida(curtidadto.IdPublicacao, ReadToken().Id);

                    if (curtida != null)
                    {
                        _curtidaRepository.Descurtir(curtida);
                    }
                    else
                    {
                        Curtida curtidanova = new Curtida()
                        {
                            IdPublicacao = curtidadto.IdPublicacao,
                            IdUsuario = ReadToken().Id
                        };
                        _curtidaRepository.Curtir(curtidanova);
                    }                   

                }
                else
                {
                    _logger.LogError("Requisição de curtir está vazia.");
                    return BadRequest("Requisição de curtir está vazia.");
                }

                return Ok("Curtida realizada com sucesso");
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao curtir/descurtir.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Descricao = "Erro ao curtir/descurtir." + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
