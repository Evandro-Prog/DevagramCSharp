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
        private readonly IComentarioRepository _comentarioRepository;
        private readonly ICurtidaRepository _curtidaRepository;
        public PublicacaoController(ILogger<PublicacaoController> logger, 
            IPublicacaoRepository publicacaoRepository, IUsuarioRepository usuarioRepository,
            IComentarioRepository comentarioRepository, ICurtidaRepository curtidaRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _publicacaoRepository = publicacaoRepository;
            _comentarioRepository = comentarioRepository;
            _curtidaRepository = curtidaRepository;
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

        [HttpGet]
        [Route("feed")]
        public IActionResult FeedHome()
        {
            try
            {
                List<PublicacaoFeedRespostaDto> feed = _publicacaoRepository.GetPublicacoesFeed(ReadToken().Id);

                foreach (PublicacaoFeedRespostaDto feedResposta in feed)
                {
                    Usuario usuario = _usuarioRepository.GetUsuarioPorId(feedResposta.IdUsuario);
                    UsuarioRespostaDto usuarioRespostaDto = new UsuarioRespostaDto()
                    {
                        Nome = usuario.Nome,
                        Avatar = usuario.FotoPerfil,
                        IdUsuario = usuario.Id
                    };

                    feedResposta.Usuario = usuarioRespostaDto;

                    List<Comentario> comentarios = _comentarioRepository.GetCOmentarioPorPublicacao(feedResposta.IdPublicacao);
                    feedResposta.Comentarios = comentarios;

                    List<Curtida> curtidas = _curtidaRepository.GetCurtidaPorPublicacao(feedResposta.IdPublicacao);
                    feedResposta.Curtidas = curtidas;

                }

                return Ok(feed);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao carregar Feed de Notícias");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Descricao = "Erro ao carregar Feed de Notícias" + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}
