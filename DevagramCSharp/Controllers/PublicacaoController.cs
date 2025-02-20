using DevagramCSharp.Dtos;
using DevagramCSharp.Models;
using DevagramCSharp.Repository;
using DevagramCSharp.Services;
using Microsoft.AspNetCore.Mvc;

namespace DevagramCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PublicacaoController : BaseController   
    {
        private readonly ILogger<PublicacaoController> _logger;
        private readonly IPublicacaoRepository _publicacaoRepository;
        public PublicacaoController(ILogger<PublicacaoController> logger, 
            IPublicacaoRepository publicacaoRepository, IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _publicacaoRepository = publicacaoRepository;
        }

        [HttpPost]
        public IActionResult Publicar([FromForm] PublicacaoRequisicaoDto publicacaodto) 
        {
            try
            {
                CosmicService cosmicService = new CosmicService();

                if(publicacaodto != null)
                {
                    if(string.IsNullOrEmpty(publicacaodto.Descricao) && string.IsNullOrWhiteSpace(publicacaodto.Descricao))
                    {
                        _logger.LogError("A descrição está inválida.");
                        return BadRequest("É obrigatorio a descrição na publicação.");
                    }
                    if(publicacaodto.Foto == null)
                    {
                        _logger.LogError("A foto está inválida.");
                        return BadRequest("É obrigatorio a foto na publicação.");
                    }

                    Publicacao publicacao = new Publicacao()
                    {
                        Descricao = publicacaodto.Descricao,
                        IdUsuario = ReadToken().Id,
                        Foto = cosmicService.EnviarImagem(new ImagemDto { Imagem = publicacaodto.Foto, Nome = " publicacao" })
                    };
                    _publicacaoRepository.publicar(publicacao);
                }

                return Ok("Publicação salva com sucesso.");
                
            }
            catch(Exception ex)
            {
                _logger.LogError("Erro na publicação.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Descricao = "Ocorreu o seguinte erro:" + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
