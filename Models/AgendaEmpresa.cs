using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud_agenda.Models
{
    [Table("Agenda_Empresa")]
    public class AgendaEmpresa
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdEmpresa { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        [Display(Name = "Nombre de Empresa")]
        public string NombreEmpresa { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }
    }
}
