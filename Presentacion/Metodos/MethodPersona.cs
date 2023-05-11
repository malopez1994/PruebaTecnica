using Presentacion.Models;
using System.Text.Json;
using System.Text;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace Presentacion.Metodos
{
    public class MethodPersona
    {

        public IConfiguration _Config;
        public MethodPersona(IConfiguration config)
        {
            _Config = config;
        }
        /// <summary>
        /// Realiza el acceso a las credenciales del token para la utilizacion de la funciones
        /// </summary>
        /// <returns></returns>
        public dynamic AccesoToken()
        {
			try
			{
				var client = new HttpClient();
                client.BaseAddress = new Uri(_Config.GetSection("URLServicio").Value);
                var json = System.Text.Json.JsonSerializer.Serialize( new { usuario ="Admin", password="123456789" });
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var request = new HttpRequestMessage(HttpMethod.Post, "Access");
                request.Content = new StringContent(json,Encoding.UTF8, "application/json");
				var result = client.Send(request);
                    string data = result.Content.ReadAsStringAsync().Result;
                    var informacion = JsonConvert.DeserializeObject<dynamic>(data);
                    return informacion;
			}
			catch (Exception ex)
			{

				throw ex;
			}
        }

        /// <summary>
        /// Realiza la consulta de todos los usuarios registrados 
        /// </summary>
        /// <returns></returns>
        public List<UsuarioModel> ListaUsuario(string token) 
        {
            List<UsuarioModel> listUsuario = new List<UsuarioModel>();
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(_Config.GetSection("URLServicio").Value);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization =new AuthenticationHeaderValue("Bearer", token);
                var request = new HttpRequestMessage(HttpMethod.Get, "ConsultarPersonas");
                var result = client.Send(request);
                if (result.StatusCode == System.Net.HttpStatusCode.OK) {
                    string data = result.Content.ReadAsStringAsync().Result;
                    listUsuario = JsonConvert.DeserializeObject<List<UsuarioModel>>(data);
                }
                return listUsuario;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///  Realiza la consulta usuario por el id registrado
        /// </summary>
        /// <returns></returns>
        public UsuarioModel UsuarioxId(string token,int id)
        {
            UsuarioModel usuario = new UsuarioModel();
            try
            {
                var client = new HttpClient();        
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                UriBuilder builder = new UriBuilder(_Config.GetSection("URLServicio").Value + "ConsultarPersonaxId");

                client.BaseAddress = new Uri(_Config.GetSection("URLServicio").Value);
                builder.Query = "Id=" + id;
                var result = client.GetAsync(builder.Uri).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string data = result.Content.ReadAsStringAsync().Result;
                    usuario = JsonConvert.DeserializeObject<UsuarioModel>(data);
                }
                return usuario;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///  Realiza la creacion del usuario
        /// </summary>
        /// <returns></returns>
        public bool CrearUsuario(string token,UsuarioModel _usuario)
        {
            bool valid = false;
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(_Config.GetSection("URLServicio").Value);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = System.Text.Json.JsonSerializer.Serialize(_usuario);
                var request = new HttpRequestMessage(HttpMethod.Post, "Creacion");
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = client.Send(request);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string data = result.Content.ReadAsStringAsync().Result;
                    valid = Convert.ToBoolean(data);
                }
                return valid;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///  Realiza la modificacion del usuario
        /// </summary>
        /// <returns></returns>
        public bool ActualizarUsuario(string token, UsuarioModel _usuario)
        {
            bool valid = false;
            try
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(_Config.GetSection("URLServicio").Value);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var json = System.Text.Json.JsonSerializer.Serialize(_usuario);
                var request = new HttpRequestMessage(HttpMethod.Put, "Actualizar");
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = client.Send(request);
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string data = result.Content.ReadAsStringAsync().Result;
                    valid = Convert.ToBoolean(data);
                }
                return valid;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        ///  Realiza la eliminacion del usuario
        /// </summary>
        /// <returns></returns>
        public bool EliminarUsuario(string token, int Id)
        {
            bool valid = false;
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                UriBuilder builder = new UriBuilder(_Config.GetSection("URLServicio").Value + "Eliminar");
                builder.Query = "id=" + Id;
                var result = client.DeleteAsync(builder.Uri).Result;
                if (result.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string data = result.Content.ReadAsStringAsync().Result;
                    valid = Convert.ToBoolean(data);
                }
                return valid;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
