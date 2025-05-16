using System.ComponentModel.DataAnnotations;

namespace JobFinder.Core.Models
{
    public class AuthModel
    {
        // Campos comunes
        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato inválido")]
        public string Correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        // Campos específicos de registro
        [Required(ErrorMessage = "Nombre de usuario requerido")]
        public string Usuario { get; set; }

        [StringLength(9, MinimumLength = 9, ErrorMessage = "Teléfono inválido")]
        public string Telefono { get; set; }

        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        public string Localidad { get; set; }
        public bool Transporte { get; set; }
        public bool EsEmpresa { get; set; }

        // Campo específico de login
        public bool RememberMe { get; set; }
    }
}