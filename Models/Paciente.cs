using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Crud_agenda.Models
{
    [Table("Pacientes")]
    public class Paciente
    {
        [Key]
        [Column("Codigo")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "Código")]
        public int Codigo { get; set; }

        [Required]
        [StringLength(15)]
        [Column("Codigo_Paciente")]
        [Display(Name = "Código Paciente")]
        public string CodigoPaciente { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Primer_Nombre")]
        [Display(Name = "Primer Nombre")]
        public string PrimerNombre { get; set; }

        [StringLength(50)]
        [Column("Segundo_Nombre")]
        [Display(Name = "Segundo Nombre")]
        public string SegundoNombre { get; set; }

        [Required]
        [StringLength(50)]
        [Column("Primer_Apellido")]
        [Display(Name = "Primer Apellido")]
        public string PrimerApellido { get; set; }

        [StringLength(50)]
        [Column("Segundo_Apellido")]
        [Display(Name = "Segundo Apellido")]
        public string SegundoApellido { get; set; }

        [StringLength(50)]
        [Column("Apellido_Casada")]
        [Display(Name = "Apellido de Casada")]
        public string ApellidoCasada { get; set; }

        [Column("Fecha_Nacimiento")]
        [Display(Name = "Fecha de Nacimiento")]
        [DataType(DataType.Date)]
        public DateTime? FechaNacimiento { get; set; }

        [StringLength(100)]
        [Column("Lugar_Nacimiento")]
        [Display(Name = "Lugar de Nacimiento")]
        public string LugarNacimiento { get; set; }

        [StringLength(30)]
        [Column("No_de_Identificacion")]
        [Display(Name = "No. Identificación")]
        public string NoIdentificacion { get; set; }

        [StringLength(60)]
        [Column("Extendida_en_Municipalidad")]
        [Display(Name = "Extendida en Municipalidad")]
        public string ExtendidaMunicipalidad { get; set; }

        [StringLength(30)]
        [Column("No_Pasaporte")]
        [Display(Name = "No. Pasaporte")]
        public string NoPasaporte { get; set; }

        [StringLength(60)]
        [Display(Name = "Dirección 1")]
        public string Direccion1 { get; set; }

        [Display(Name = "Zona")]
        public int? Zona { get; set; }

        [StringLength(60)]
        [Display(Name = "Dirección 2")]
        public string Direccion2 { get; set; }

        [StringLength(12)]
        [Display(Name = "Teléfono")]
        public string Telefono { get; set; }

        [StringLength(12)]
        [Display(Name = "Celular")]
        public string Celular { get; set; }

        [StringLength(12)]
        [Column("Telefono_Trabajo")]
        [Display(Name = "Teléfono Trabajo")]
        public string TelefonoTrabajo { get; set; }

        [StringLength(100)]
        [Column("EMail")]
        [Display(Name = "Correo electrónico")]
        public string EMail { get; set; }

        [StringLength(200)]
        [Display(Name = "Responsable de Cuenta")]
        public string ResponsableCuenta { get; set; }

        [Column("c_Usuario_Crea")]
        public int UsuarioCrea { get; set; }

        [Column("Fecha_Crea")]
        public DateTime FechaCrea { get; set; }

        [Column("c_Usuario_Modifica")]
        public int UsuarioModifica { get; set; }

        [Column("Fecha_Modifica")]
        public DateTime? FechaModifica { get; set; }

        [NotMapped]
        public string NombreCompleto
        {
            get
            {
                return string.Join(" ", new[]
                {
                    Trim(PrimerNombre),
                    Trim(SegundoNombre),
                    Trim(PrimerApellido),
                    Trim(SegundoApellido),
                    Trim(ApellidoCasada)
                }.Where(x => !string.IsNullOrEmpty(x)));
            }
        }

        public static string Trim(string value)
        {
            return string.IsNullOrWhiteSpace(value) ? string.Empty : value.Trim();
        }
    }
}
