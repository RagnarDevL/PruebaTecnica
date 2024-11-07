using System.ComponentModel.DataAnnotations;

namespace LiteThinkingPrueba.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }

        [Required]
        [StringLength(100)]
        public string CorreoElectronico { get; set; }

        [Required]
        [StringLength(50)]
        public string Contraseña { get; set; }
    }
}
