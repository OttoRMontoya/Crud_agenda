using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud_agenda.Models
{
    [Table("Agenda_Doctor")]
    public class AgendaDoctor
    {
        [Key]
        [Column("Codigo_Doctor")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Código Doctor")]
        public int CodigoDoctor { get; set; }

        [Display(Name = "Empresa")]
        public int IdEmpresa { get; set; }

        [Display(Name = "Clínica")]
        public int? IdClinica { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100)]
        [Display(Name = "Nombre del Doctor")]
        public string NombreDoctor { get; set; }

        [StringLength(100)]
        [Display(Name = "Especialidad")]
        public string Especialidad { get; set; }

        [Display(Name = "Activo")]
        public bool Activo { get; set; }

        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [Display(Name = "Fecha de Modificación")]
        public DateTime? FechaModificacion { get; set; }

        [ForeignKey("IdEmpresa")]
        public virtual AgendaEmpresa Empresa { get; set; }

        [ForeignKey("IdClinica")]
        public virtual AgendaClinica Clinica { get; set; }
    }
}
