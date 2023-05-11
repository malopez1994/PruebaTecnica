using Negocio.Entidad;

namespace Negocio.Interfaces
{
    public interface ILog
    {
        public void CrearLogPersona(int Idpersona, string Metodo, string Respuesta, string Descripcion);
    }
}
