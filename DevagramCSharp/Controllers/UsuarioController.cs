	using DevagramCSharp.Dtos;
	using DevagramCSharp.Models;
using DevagramCSharp.Repository;
using DevagramCSharp.Repository.Impl;
using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Mvc;

	namespace DevagramCSharp.Controllers
	{

		[ApiController]
		[Route("api/[controller]")]
		public class UsuarioController : BaseController
		{
			public readonly ILogger<UsuarioController> _logger;
			public readonly IUsuarioRepository _usuarioRepository;

			public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository)
			{
				_logger = logger;
				_usuarioRepository = usuarioRepository;
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

			[HttpPost]
			public IActionResult SalvarUsuario([FromBody] Usuario usuario)
			{
				try
				{

					if(usuario != null)
					{
						var erros = new List<string>();
						if(string.IsNullOrEmpty(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Nome))
						{
							erros.Add("Nome inválido");
						}                                       
						if (string.IsNullOrEmpty(usuario.Email) || string.IsNullOrWhiteSpace(usuario.Email) || !usuario.Email.Contains("@"))
						{
							erros.Add("Email inválido");
						}                        
						if (string.IsNullOrEmpty(usuario.Nome) || string.IsNullOrWhiteSpace(usuario.Nome))
						{
							erros.Add("Nome inválido");
						}

						if(erros.Count > 0)
						{
							return BadRequest(new ErrorResponseDto()
							{
								Status = StatusCodes.Status400BadRequest,
								Erros = erros
							});
						}

						 _usuarioRepository.Salvar(usuario);

					}   

					return Ok(usuario);
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

		}
	}
