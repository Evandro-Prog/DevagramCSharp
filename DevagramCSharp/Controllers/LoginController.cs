using DevagramCSharp.Dtos;
using DevagramCSharp.Models;
using DevagramCSharp.Services;
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

        public LoginController (ILogger<LoginController> logger)
        {
            _logger = logger;
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
                    string email = "evandro@evandro.com.br";
                    string senha = "Senha1234@";

                    if (loginrequisicao.Email == email && loginrequisicao.Senha == senha)
                    {

                        Usuario usuario = new Usuario()
                        {
                            Email = loginrequisicao.Email,
                            Id = 12,
                            Nome = "Evandro Marques"
                        };

                        return Ok(new LoginRespostaDto()
                        {
                            Email = usuario.Email = email,
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
                return StatusCode(StatusCodes.Status500InternalServerError,  new ErrorResponseDto()
                {
                    Descricao = "Ocorreu um erro ao realizar login.",
                    Status = StatusCodes.Status500InternalServerError
                });
            }
        }
    }
}

