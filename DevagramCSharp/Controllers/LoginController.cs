using DevagramCSharp.Dtos;
using DevagramCSharp.Models;
using DevagramCSharp.Repository;
using DevagramCSharp.Services;
using DevagramCSharp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevagramCSharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;
        private readonly IUsuarioRepository _usuarioRepository;

        public LoginController (ILogger<LoginController> logger, IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult EfetuarLogin([FromBody] LoginRequisicaoDto loginrequisicao)
        {
            try
            {
                if (!string.IsNullOrEmpty(loginrequisicao.Senha) && !string.IsNullOrEmpty(loginrequisicao.Email) &&
                     !string.IsNullOrWhiteSpace(loginrequisicao.Senha) && !string.IsNullOrWhiteSpace(loginrequisicao.Email))
                {

                    Usuario usuario = _usuarioRepository.GetUsuarioPorLoginSenha(loginrequisicao.Email.ToLower(), MD5Utils.GerarHashMD5(loginrequisicao.Senha));

                    if(usuario != null)
                    {
                        return Ok(new LoginRespostaDto()
                        {
                            Email = usuario.Email,
                            Nome = usuario.Nome,
                            Token = TokenService.CriarToken(usuario)
                        });
                    }
                    else
                    {
                        return BadRequest(new ErrorResponseDto()
                        {
                            Descricao = "Email ou senha inválido",
                            Status = StatusCodes.Status400BadRequest
                        });
                    }
                    
                }
                else
                {

                    return BadRequest(new ErrorResponseDto()
                    {
                        Descricao = "Usuario não preencheu os dados corretamente",
                        Status = StatusCodes.Status400BadRequest
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Ocorreu um erro no login: " + ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Descricao = "Ocorreu um erro ao realizar login.",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}

