using Google.Cloud.Firestore;

namespace JobFinder.Core.Models
{
    [FirestoreData]
    public class Oferta
    {
        [FirestoreDocumentId]
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string Titulo { get; set; }

        [FirestoreProperty]
        public required string Descripcion { get; set; }

        [FirestoreProperty]
        public required string Localidad { get; set; }

        [FirestoreProperty]
        public Timestamp FechaPublicacion { get; set; }

        [FirestoreProperty]
        public required string IdEmpresa { get; set; } // Referencia al ID del usuario empresa

        [FirestoreProperty]
        public String[]? candidatos { get; set; }
    }
}
