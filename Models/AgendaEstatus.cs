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
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }
    }
}
