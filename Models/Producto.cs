using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LiteThinkingPrueba.Models
{
    public class Producto
    {
       
            [Key]
            public int ProductoId { get; set; }
            public string Nombre { get; set; }
            public int Precio { get; set; }

            [Required]
            public int EmpresaId { get; set; }
            public Empresa Empresa { get; set; }
        

    }
}
