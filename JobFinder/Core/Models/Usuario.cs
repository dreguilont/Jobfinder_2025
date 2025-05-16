using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Google.Cloud.Firestore;
using NuGet.Packaging.Signing;


namespace JobFinder.Core.Models
{
    [FirestoreData]
    public class Usuario
    {
        [FirestoreDocumentId] // Indica que este campo será el ID del documento en Firestore
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string Nombre { get; set; }
        [FirestoreProperty]
        public required string Password { get; set; }

        [FirestoreProperty]
        public required string Email { get; set; }

        [FirestoreProperty]
        public int Telefono { get; set; }

        [FirestoreProperty]
        public Google.Cloud.Firestore.Timestamp FechaNacimiento { get; set; } // Firestore usa Timestamp para fechas

        [FirestoreProperty]
        public string? Localidad { get; set; }

        [FirestoreProperty]
        public bool? TieneTransporte { get; set; }

        [FirestoreProperty]
        public bool EsEmpresa { get; set; } // Rol clave: true = empresa, false = candidato

        public String[]? ofertas { get; set; }
    }
}