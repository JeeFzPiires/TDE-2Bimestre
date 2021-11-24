using ApiProduto.Context;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using ApiProduto.models;
using ApiProduto.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace ApiProduto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly ProdutoContext Context;
        public UsuarioController()
        {
            Context = new ProdutoContext();
        }


        /// <summary>
        /// Método faz autenticação usuário e senha.
        /// </summary>
        /// <param name="Usuario"></param>
        /// <param name="Senha"></param>
        /// <returns>A newly created TodoItem</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        
        ///        "UserName": "H1",
        ///        "Password": "H1"
        ///     }
        ///
        /// </remarks>
        /// <response code="201">Returns o token do usuário autenticado</response>
        /// <response code="400">Usuario não existe</response>
        /// <response code="500">Senha incorreta</response>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate([FromBody] Usuario model)
        {
            // Recupera o usuário
            var user = await Context._usuarios.Find<Usuario>
                (p => p.UserName == model.UserName).FirstOrDefaultAsync();

            // Verifica se o usuário existe
            if (user == null)
                return NotFound(new { message = "Usuário não existe" });

            if (user.Password != model.Password)
                return BadRequest(new { message = "Senha inválida" });

            // Gera o Token
            var token = TokenService.GenerateToken(user);

            // Oculta a senha
            user.Password = "";

            // Retorna os dados
            return new
            {
                user = user,
                token = token
            };
        }

    }
}