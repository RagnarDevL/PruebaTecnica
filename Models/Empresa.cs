using System.ComponentModel.DataAnnotations;

namespace LiteThinkingPrueba.Models
{
    public class Empresa
    {
        [Key]
        public int NIT { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }

}
