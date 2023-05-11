using Negocio.Entidad;
using Negocio.Interfaces;
using System.Data;
using System.Data.SqlClient;

namespace Negocio.Datos
{
    public class PersonaDatos : IPersonaDatos
    {
        public IConfiguration _Config;
        public PersonaDatos(IConfiguration config)
        {
            _Config = config;
        }
        private SqlConnection conn;
        private SqlCommand cmd;
        private Log registroLog;
        public bool ActualizarPersona(Persona persona)
        {
            registroLog = new Log(_Config);
            try
            {
                conn = new SqlConnection(_Config.GetSection("ConnectionStrings:Connection").Value);
                conn.Open();
                cmd = new SqlCommand("SP_CRUD_PERSONA", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tipo", 2);
                cmd.Parameters.AddWithValue("@IdPersona", persona.Id);
                cmd.Parameters.AddWithValue("@NombrePersona", persona.Nombre);
                cmd.Parameters.AddWithValue("@FechaNacimientoPersona", persona.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Genero", persona.Genero);
                var result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    conn.Close();
                    registroLog.CrearLogPersona(persona.Id, "Update","Correcto","Se ha actualizado correctamente");
                    return true;
                }
                else
                {
                    conn.Close();
                    registroLog.CrearLogPersona(persona.Id, "Update", "Correcto", "Se ha actualizado correctamente");
                    return false;
                }
            }
            catch (SqlException e)
            {
                conn.Close();
                registroLog.CrearLogPersona(persona.Id, "Update", "Correcto", "Se ha actualizado correctamente");
                throw;
            }
            catch (Exception)
            {
                conn.Close();
                registroLog.CrearLogPersona(persona.Id, "Update", "Correcto", "Se ha actualizado correctamente");
                throw;
            }
        }

        public List<Persona> ConsultarPersona()
        {
            registroLog = new Log(_Config);
            List<Persona> listadoPersona = new List<Persona>();
            try
            {
                //conn = new SqlConnection("Server=MALOPEZ;Database=PruebaTecnica;Trusted_Connection=True;MultipleActiveResultSets=true");
                
                conn = new SqlConnection(_Config.GetSection("ConnectionStrings:Connection").Value);
                conn.Open();
                cmd = new SqlCommand("SP_CRUD_PERSONA", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tipo", 0);
                cmd.Parameters.AddWithValue("@FechaNacimientoPersona", DateTime.Now);
                Persona i;
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    registroLog.CrearLogPersona(0,"Query","Correcto","La consulta se ejecuto correctamente");
                    while (reader.Read())
                    {
                        i = new Persona
                        {
                            Id = int.Parse(reader.GetValue("Id_Persona").ToString()),
                            Nombre = reader.GetValue("Nombre_Persona").ToString(),
                            FechaNacimiento = Convert.ToDateTime(reader.GetValue("FechaNacimiento_Persona").ToString()),
                            Genero = reader.GetValue("Genero_Persona").ToString()
                        };
                        listadoPersona.Add(i);
                    }
                }
                conn.Close();
                return listadoPersona;
            }
            catch (SqlException e)
            {
                conn.Close();
                registroLog.CrearLogPersona(0, "Query", "Error", "Se genero error en el SQL " + e.Message);
                throw;
            }
            catch (Exception ex)
            {
                conn.Close();
                registroLog.CrearLogPersona(0, "Query", "Error", "Hubo error en la consulta " + ex.Message);
                throw;
            }
        }

        public Persona ConsultarPersonaxId(int id)
        {
            registroLog = new Log(_Config);
            Persona persona = new Persona();
            try
            {
                conn = new SqlConnection(_Config.GetSection("ConnectionStrings:Connection").Value);
                conn.Open();
                cmd = new SqlCommand("SP_CRUD_PERSONA", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tipo", 4);
                cmd.Parameters.AddWithValue("@IdPersona", id);
                cmd.Parameters.AddWithValue("@FechaNacimientoPersona", DateTime.Now);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    registroLog.CrearLogPersona(0, "Query", "Correcto", "La consulta se ejecuto correctamente");
                    if (reader.Read())
                    {
                        persona = new Persona
                        {
                            Id = int.Parse(reader.GetValue("Id_Persona").ToString()),
                            Nombre = reader.GetValue("Nombre_Persona").ToString(),
                            FechaNacimiento = Convert.ToDateTime(reader.GetValue("FechaNacimiento_Persona").ToString()),
                            Genero = reader.GetValue("Genero_Persona").ToString()
                        };
                    }
                }
                conn.Close();
                return persona;
            }
            catch (SqlException e)
            {
                conn.Close();
                registroLog.CrearLogPersona(0, "Query", "Error", "Se genero error en el SQL " + e.Message);
                throw;
            }
            catch (Exception ex)
            {
                conn.Close();
                registroLog.CrearLogPersona(0, "Query", "Error", "Hubo error en la consulta " + ex.Message);
                throw;
            }
        }
        public bool CrearPersona(Persona persona)
        {
            registroLog = new Log(_Config);
            try
            {
                conn = new SqlConnection(_Config.GetSection("ConnectionStrings:Connection").Value);
                conn.Open();
                cmd = new SqlCommand("SP_CRUD_PERSONA", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tipo", 1);
                cmd.Parameters.AddWithValue("@NombrePersona", persona.Nombre);
                cmd.Parameters.AddWithValue("@FechaNacimientoPersona", persona.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Genero", persona.Genero);
                var result = cmd.ExecuteScalar();
                if (result != null)
                {
                    conn.Close();
                    registroLog.CrearLogPersona(Convert.ToInt32(result), "Create", "Correcto", "Se ha creado correctamente");
                    return true;
                }
                else
                {
                    conn.Close();
                    registroLog.CrearLogPersona(0, "Create", "Error", "No se registro la informacion");
                    return false;
                }
            }
            catch (SqlException e)
            {
                conn.Close();
                registroLog.CrearLogPersona(0, "Create", "Error", "Se genero error en el SQL " + e.Message);
                throw;
            }
            catch (Exception ex)
            {
                conn.Close();
                registroLog.CrearLogPersona(0, "Create", "Error", "Hubo error en la creacion " + ex.Message);
                throw;
            }
        }

        public bool EliminarPersona(int Idpersona)
        {
            registroLog = new Log(_Config);
            try
            {
                conn = new SqlConnection(_Config.GetSection("ConnectionStrings:Connection").Value);
                conn.Open();
                cmd = new SqlCommand("SP_CRUD_PERSONA", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Tipo", 3);
                cmd.Parameters.AddWithValue("@IdPersona", Idpersona);
                cmd.Parameters.AddWithValue("@FechaNacimientoPersona", DateTime.Now);
                var result = cmd.ExecuteNonQuery();
                if (result == 1)
                {
                    conn.Close();
                    registroLog.CrearLogPersona(Idpersona, "Delete", "Correcto", "Se ha creado correctamente");
                    return true;
                }
                else 
                {
                    conn.Close();
                    registroLog.CrearLogPersona(Idpersona, "Delete", "Error", "No se puede eliminar la persona");
                    return false;
                }
            }
            catch (SqlException e)
            {
                conn.Close();
                registroLog.CrearLogPersona(Idpersona, "Delete", "Error", "Se genero error en el SQL " + e.Message);
                return false;
            }
            catch (Exception ex)
            {
                conn.Close();
                registroLog.CrearLogPersona(Idpersona, "Delete", "Error", "Hubo error en la eliminacion " + ex.Message);
                return false;
            }
        }
    }
}
