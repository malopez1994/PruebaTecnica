using Microsoft.AspNetCore.Mvc;
using Negocio.Datos;
using Negocio.Entidad;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Negocio.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonaController : ControllerBase
    {
        public IConfiguration _Config;
        public PersonaController(IConfiguration config) 
        { 
        _Config = config;
        }
        private PersonaDatos PD;
        [Authorize]
        [HttpGet]
        [Route("ConsultarPersonas")]
        public List<Persona> ListaPersonas()
        {
            PD = new PersonaDatos(_Config);
            return PD.ConsultarPersona();
        }

        [Authorize]
        [HttpGet]
        [Route("ConsultarPersonaxId")]
        public Persona ConsultarPersonaxId(int Id)
        {
            PD = new PersonaDatos(_Config);
            return PD.ConsultarPersonaxId(Id);
        }

        [Authorize]
        [HttpPost]
        [Route("Creacion")]
        public bool Create(Persona persona)
        {
            PD = new PersonaDatos(_Config);
            return PD.CrearPersona(persona);
        }

        // PUT api/<PersonaController>/5
        [Authorize]
        [HttpPut]
        [Route("Actualizar")]
        public bool Updte(Persona persona)
        {
            PD = new PersonaDatos(_Config);
            return PD.ActualizarPersona(persona);
        }

        // DELETE api/<PersonaController>/5
        [Authorize]
        [HttpDelete]
        [Route("Eliminar")]
        public bool Delete(int id)
        {
            PD = new PersonaDatos(_Config);
            return PD.EliminarPersona(id);
        }

        [HttpPost]
        [Route("Access")]
        public dynamic Acceso([FromBody] Object objData) 
        {
            var informacion = JsonConvert.DeserializeObject<dynamic>(objData.ToString());
            string usuario = informacion.usuario.ToString();
            string pass = informacion.password.ToString();
            if (usuario == _Config.GetSection("Credencial:Usuario").Value && pass == _Config.GetSection("Credencial:Password").Value)
            {
                var claims = new[]
                {
                new Claim(JwtRegisteredClaimNames.Sub, _Config.GetSection("JWT:Subject").Value),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("Usuario", usuario)
                };
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_Config.GetSection("JWT:Key").Value));
                var sigin = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    _Config.GetSection("JWT:Issuer").Value,
                    _Config.GetSection("JWT:Audience").Value,
                    claims,
                    expires: DateTime.Now.AddMinutes(15),
                    signingCredentials: sigin
                    );
                return new
                {
                    success = true,
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                };
            }
            else 
            {
                return new
                {
                    success = false,
                    Error = "Las credenciales estan incorrectas"
                };
            }

        }

    }
}
