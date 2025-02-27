using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticaCubosMVC.Models
{
    [Table("COMPRA")]
    public class Compra
    {
        [Key]
        [Column("id_compra")] // Mapea la clave primaria con la columna en MySQL
        public int IdCompra { get; set; }

        [Required]
        [Column("id_cubo")] // Relacionado con la clave de Cubo
        public int IdCubo { get; set; }

        [Required]
        [Column("cantidad")]
        public int Cantidad { get; set; }

        [Required]
        [Column("precio")]
        public int Precio { get; set; }

        [Required]
        [Column("fechapedido")]
        public DateTime FechaPedido { get; set; }

        // Relación con la tabla CUBOS
        [ForeignKey("IdCubo")]
        public Cubo Cubo { get; set; }
    }
}
