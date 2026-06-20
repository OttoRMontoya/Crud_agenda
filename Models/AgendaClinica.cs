using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud_agenda.Models
{
    [Table("Agenda_Clinica")]
    public class AgendaClinica
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdClinica { get; set; }

        [Display(Name = "Empresa")]
        public int IdEmpresa { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        [Display(Name = "Nombre de Clínica")]
        public string NombreClinica { get; set; }

        [StringLength(200)]
        [Display(Name = "Dirección")]
        public string Direccion { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }

        [ForeignKey("IdEmpresa")]
        public virtual AgendaEmpresa Empresa { get; set; }
    }
}
