using JobFinder.Core.Models;

namespace JobFinder.Core.Services
{
    public interface IFirebaseService
    {
        // Operaciones para Usuarios
        Task<string> CrearUsuario(Usuario usuario);
        Task<Usuario> ObtenerUsuario(string id);
        Task ActualizarUsuario(Usuario usuario);
        Task EliminarUsuario(string id);

        // Operaciones para Ofertas
        Task<string> CrearOferta(Oferta oferta);
        Task<Oferta> ObtenerOferta(string id);
        Task<List<Oferta>> ObtenerTodasOfertas();
        Task ActualizarOferta(Oferta oferta);
        Task EliminarOferta(string id);

        // Operaciones específicas de negocio
        Task AplicarAOferta(string ofertaId, string usuarioId);
        Task<List<Usuario>> ObtenerAplicantes(string ofertaId);
    }
}
