using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud_agenda.Models
{
    [Table("Agenda")]
    public class Agenda
    {
        [Key]
        [Column("Id_Agenda_Cita")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAgendaCita { get; set; }

        [Column("Id_Calendario")]
        [Display(Name = "Calendario")]
        public int IdCalendario { get; set; }

        [Display(Name = "Empresa")]
        public int Empresa { get; set; }

        [Column("Codigo_Doctor")]
        [Display(Name = "Código Doctor")]
        public int CodigoDoctor { get; set; }

        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [Display(Name = "Clínica")]
        public int Clinica { get; set; }

        [Column("No_Ficha_Ingreso")]
        [Display(Name = "No. Ficha Ingreso")]
        public int NoFichaIngreso { get; set; }

        [Required(ErrorMessage = "El contacto es requerido")]
        [StringLength(100)]
        [Display(Name = "Contacto")]
        public string Contacto { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        [Display(Name = "Inicio")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Inicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        [Display(Name = "Fin")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Fin { get; set; }

        [Required(ErrorMessage = "El asunto es requerido")]
        [StringLength(50)]
        [Display(Name = "Asunto")]
        public string Asunto { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Lugar")]
        public string Lugar { get; set; }

        [Display(Name = "Etiqueta")]
        public int Etiqueta { get; set; }

        [Display(Name = "Estatus")]
        public int Estatus { get; set; }

        [Column("TodoelDia")]
        [Display(Name = "Todo el día")]
        public bool TodoelDia { get; set; }

        [Required]
        [StringLength(12)]
        [Display(Name = "Teléfono 1")]
        public string Telefono1 { get; set; }

        [Required]
        [StringLength(12)]
        [Display(Name = "Teléfono 2")]
        public string Telefono2 { get; set; }

        [Required]
        [StringLength(12)]
        [Display(Name = "Celular")]
        public string Celular { get; set; }

        [Required]
        [StringLength(100)]
        [Column("EMail")]
        [Display(Name = "Correo electrónico")]
        public string EMail { get; set; }

        [Column("Envia_Recordatorio")]
        [Display(Name = "Enviar recordatorio")]
        public bool EnviaRecordatorio { get; set; }

        [Column("c_Usuario_Crea")]
        public int UsuarioCrea { get; set; }

        [Column("Fecha_Crea")]
        public DateTime FechaCrea { get; set; }

        [Column("c_Usuario_Modifica")]
        public int UsuarioModifica { get; set; }

        [Column("Fecha_Modifica")]
        public DateTime FechaModifica { get; set; }
    }
}
