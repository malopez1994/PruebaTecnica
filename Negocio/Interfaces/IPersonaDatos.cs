using Negocio.Entidad;
namespace Negocio.Interfaces
{
    public interface IPersonaDatos
    {
        public bool CrearPersona(Persona persona);
        public bool ActualizarPersona(Persona persona);
        public bool EliminarPersona(int Idpersona);
        public List<Persona> ConsultarPersona();
    }
}
