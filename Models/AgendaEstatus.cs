using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud_agenda.Models
{
    [Table("Agenda_Estatus")]
    public class AgendaEstatus
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEstatus { get; set; }

        [Required(ErrorMessage = "El nombre del estado es requerido")]
        [StringLength(100)]
        [Display(Name = "Nombre del Estado")]
        public string NombreEstatus { get; set; }

        [StringLength(500)]
        [Display(Name = "Descripci�n")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El color es requerido")]
        [StringLength(7)]
        [Display(Name = "Color")]
        [RegularExpression(@"^#[0-9A-Fa-f]{6}$", ErrorMessage = "El color debe ser un c�digo hexadecimal v�lido (ej: #667eea)")]
        public string Color { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Display(Name = "Fecha de Creaci�n")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de Modificaci�n")]
        public DateTime? FechaModificacion { get; set; }
    }
}
