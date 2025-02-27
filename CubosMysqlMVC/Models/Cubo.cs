using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PracticaCubosMVC.Models
{
    [Table("CUBOS")]
    public class Cubo
    {
        [Key]
        [Column("id_cubo")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)] // No lo genera la BD
        public int IdCubo { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [Column("nombre")]
        [MaxLength(500)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        [Column("modelo")]
        [MaxLength(500)]
        public string Modelo { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        [Column("marca")]
        [MaxLength(500)]
        public string Marca { get; set; }

        [Column("imagen")]
        [MaxLength(500)]
        public string Imagen { get; set; } = "default.png";

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Column("precio")]
        public int Precio { get; set; }

        public ICollection<Compra> Compras { get; set; } = new List<Compra>();
    }
}
