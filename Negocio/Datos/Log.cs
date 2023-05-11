using Negocio.Entidad;
using Negocio.Interfaces;
using System.Data.SqlClient;

namespace Negocio.Datos
{
    public class Log : ILog
    {
        public IConfiguration _Config;
        public Log(IConfiguration config)
        {
            _Config = config;
        }
        private SqlConnection conn;
        private SqlCommand cmd;
        public void CrearLogPersona(int Idpersona, string Metodo, string Respuesta, string Descripcion)
        {
            try
            {
                conn = new SqlConnection(_Config.GetSection("ConnectionStrings:Connection").Value);
                conn.Open();
                cmd = new SqlCommand("SP_LOG_PERSONA", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tipo", 1);
                cmd.Parameters.AddWithValue("@IdPersona", Idpersona);
                cmd.Parameters.AddWithValue("@Metodo", Metodo);
                cmd.Parameters.AddWithValue("@Respuesta_Log", Respuesta);
                cmd.Parameters.AddWithValue("@Descripcion_Log", Descripcion);
                var result = cmd.ExecuteNonQuery(); 
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
