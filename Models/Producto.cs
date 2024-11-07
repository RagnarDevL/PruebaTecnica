using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LiteThinkingPrueba.Models
{
    public class Producto
    {
       
            [Key]
            public string Codigo { get; set; }
            public string Nombre { get; set; }
            public string Caracteristicas { get; set; }
            public string Precio { get; set; }

            [Required]
            public int EmpresaId { get; set; }
            public Empresa Empresa { get; set; }
        

    }
}
