	using DevagramCSharp.Dtos;
	using DevagramCSharp.Models;
using DevagramCSharp.Repository;
using DevagramCSharp.Repository.Impl;
using DevagramCSharp.Services;
using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	namespace DevagramCSharp.Controllers
	{

		[ApiController]
		[Route("api/[controller]")]
		public class UsuarioController : BaseController
		{
			private readonly ILogger<UsuarioController> _logger;	
			private readonly IPublicacaoRepository _publicacaoRepository;
			private readonly ISeguidorRepository _seguidorRepository;

			public UsuarioController(ILogger<UsuarioController> logger, 
				IUsuarioRepository usuarioRepository, IPublicacaoRepository publicacaoRepository,
                ISeguidorRepository seguidorRepository) : base(usuarioRepository)
			{
				_logger = logger;
				_publicacaoRepository = publicacaoRepository;
				_seguidorRepository = seguidorRepository;

            }

			[HttpGet]        
			public IActionResult ObterUsuario()
			{
				try
				{

					Usuario usuario = ReadToken();

					return Ok(new UsuarioRespostaDto
					{
						Nome = usuario.Nome,
						Email = usuario.Email
					});
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

		[HttpPut]
		public IActionResult AtualizarUsuario([FromForm] UsuarioRequisicaoDto usuariodto) 
		{
			try
			{
				Usuario usuario = ReadToken(); 

				if (usuariodto != null)
				{
					var erros = new List<string>();
					if (string.IsNullOrEmpty(usuariodto.Nome) || string.IsNullOrWhiteSpace(usuariodto.Nome))
					{
						erros.Add("Nome inválido");
					}
					if (erros.Count > 0)
					{
						return BadRequest(new ErrorResponseDto()
						{
							Status = StatusCodes.Status400BadRequest,
							Erros = erros
						});
					}
					else
					{
						CosmicService cosmicService = new CosmicService();

						usuario.FotoPerfil = cosmicService.EnviarImagem(new ImagemDto { Imagem = usuariodto.FotoPerfil, Nome = usuariodto.Nome.Replace(" ", "") });
						usuario.Nome = usuariodto.Nome;

						_usuarioRepository.AtualizarUsuario(usuario);
						
					}                    
				}

				return Ok("Infomações atualizadas.");
			}
			catch (Exception ex)
			{
				_logger.LogError("Erro ao salvar usuário");
				return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
				{
					Descricao = "Ocorreu o seguinte erro:" + ex.Message,
					Status = StatusCodes.Status500InternalServerError
				});
			}
		}
			[HttpPost]
			[AllowAnonymous]
			public IActionResult SalvarUsuario([FromForm] UsuarioRequisicaoDto usuariodto)
			{
				try
				{

					if (usuariodto != null)
					{
						var erros = new List<string>();
						if (string.IsNullOrEmpty(usuariodto.Nome) || string.IsNullOrWhiteSpace(usuariodto.Nome))
						{
							erros.Add("Nome inválido");
						}
						if (string.IsNullOrEmpty(usuariodto.Email) || string.IsNullOrWhiteSpace(usuariodto.Email) || !usuariodto.Email.Contains("@"))
						{
							erros.Add("Email inválido");
						}
						if (string.IsNullOrEmpty(usuariodto.Nome) || string.IsNullOrWhiteSpace(usuariodto.Nome))
						{
							erros.Add("Nome inválido");
						}

						if (erros.Count > 0)
						{
							return BadRequest(new ErrorResponseDto()
							{
								Status = StatusCodes.Status400BadRequest,
								Erros = erros
							});
						}

					CosmicService  cosmicservice = new CosmicService();

					Usuario usuario = new Usuario()
					{
						Email = usuariodto.Email,
						Senha = usuariodto.Senha,
						Nome = usuariodto.Nome,
						FotoPerfil = cosmicservice.EnviarImagem(new ImagemDto {Imagem = usuariodto.FotoPerfil, Nome = usuariodto.Nome.Replace(" ","")})
					};

						//criptografia de senha
						usuario.Senha = Utils.MD5Utils.GerarHashMD5(usuario.Senha);
						usuario.Email = usuario.Email.ToLower();

						if (!_usuarioRepository.VerificarEmail(usuario.Email))
						{
							_usuarioRepository.Salvar(usuario);
						}
						else
						{
							return BadRequest(new ErrorResponseDto()
							{
								Status = StatusCodes.Status400BadRequest,
								Descricao = "Usuário já cadastrado."
							});
						}
					}

					 return Ok("Usuário cadastrado!");
					}
					 catch (Exception ex)
					{
						_logger.LogError("Erro ao salvar usuário");
						return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
						{
							Descricao = "Ocorreu o seguinte erro:" + ex.Message,
							Status = StatusCodes.Status500InternalServerError
						});
				}				

			}
		[HttpGet]
		[Route("pesquisaid")]
		public IActionResult PesquisarUsaurioID(int idUsuario)
		{
			try
			{
				Usuario usuario = _usuarioRepository.GetUsuarioPorId(idUsuario);
				int qtdepublicacoes = _publicacaoRepository.GetQtdePublicacoes(idUsuario);
				int qtdeseguidores = _seguidorRepository.GetQtdeSeguidores(idUsuario);
				int qtdeseguindo = _seguidorRepository.GetQtdeSeguindo(idUsuario);

				return Ok(new UsuarioRespostaDto
				{
					Nome = usuario.Nome,
					Email = usuario.Email,
					Avatar = usuario.FotoPerfil,
					IdUsuario = usuario.Id,
					QtdePublicacoes = qtdepublicacoes,
					QtdeSeguidores = qtdeseguidores,
					QtdeSeguindo = qtdeseguindo
				});
			}
			catch(Exception ex)
			{
                _logger.LogError("Erro obter dados do usuário pesquisado.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Descricao = "Erro obter dados do usuário pesquisado:" + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
		}
        [HttpGet]
        [Route("pesquisanome")]
        public IActionResult PesquisarUsaurioNome(string nome)
        {
            try
            {
                List <Usuario> usuarios = _usuarioRepository.GetUsuarioNome(nome);
				List<UsuarioRespostaDto> usuariosresposta = new List<UsuarioRespostaDto>();

				foreach(Usuario usuario in usuarios)
				{
                    int qtdepublicacoes = _publicacaoRepository.GetQtdePublicacoes(usuario.Id);
                    int qtdeseguidores = _seguidorRepository.GetQtdeSeguidores(usuario.Id);
                    int qtdeseguindo = _seguidorRepository.GetQtdeSeguindo(usuario.Id);

					usuariosresposta.Add(new UsuarioRespostaDto
					{
						Nome = usuario.Nome,
						Email = usuario.Email,
						Avatar = usuario.FotoPerfil,
						IdUsuario = usuario.Id,
						QtdePublicacoes = qtdepublicacoes,
						QtdeSeguidores = qtdeseguidores,
						QtdeSeguindo = qtdeseguindo
					});
                    
                };				

                return Ok(usuariosresposta);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro obter dados do usuário pesquisado.");
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Descricao = "Erro obter dados do usuário pesquisado:" + ex.Message,
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
	}
