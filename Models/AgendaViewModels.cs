using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Crud_agenda.Models
{
    public class AgendaCitaViewModel
    {
        public int IdAgendaCita { get; set; }

        public int IdCalendario { get; set; }

        public int Empresa { get; set; }

        public int CodigoDoctor { get; set; }

        public int Codigo { get; set; }

        public int Clinica { get; set; }

        public int NoFichaIngreso { get; set; }

        [Required(ErrorMessage = "El contacto es requerido")]
        [StringLength(100)]
        public string Contacto { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es requerida")]
        public DateTime Inicio { get; set; }

        [Required(ErrorMessage = "La fecha de fin es requerida")]
        public DateTime Fin { get; set; }

        [Required(ErrorMessage = "El asunto es requerido")]
        [StringLength(50)]
        public string Asunto { get; set; }

        [StringLength(250)]
        public string Descripcion { get; set; }

        [StringLength(50)]
        public string Lugar { get; set; }

        public int Etiqueta { get; set; }

        public int Estatus { get; set; }

        public bool TodoelDia { get; set; }

        [StringLength(12)]
        public string Telefono1 { get; set; }

        [StringLength(12)]
        public string Telefono2 { get; set; }

        [StringLength(12)]
        public string Celular { get; set; }

        [StringLength(100)]
        [EmailAddress(ErrorMessage = "Correo electrónico no válido")]
        public string EMail { get; set; }

        public bool EnviaRecordatorio { get; set; }
    }

    public class AgendaEventDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public bool allDay { get; set; }
        public string backgroundColor { get; set; }
        public string borderColor { get; set; }
        public string textColor { get; set; }
        public string resourceId { get; set; }
        public object extendedProps { get; set; }
    }

    public class EtiquetaOption
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Color { get; set; }
    }

    public static class AgendaEtiquetas
    {
        public static readonly List<EtiquetaOption> Opciones = new List<EtiquetaOption>
        {
            new EtiquetaOption { Id = 1, Nombre = "Consulta", Color = "#3788d8" },
            new EtiquetaOption { Id = 2, Nombre = "Cirugía", Color = "#e74c3c" },
            new EtiquetaOption { Id = 3, Nombre = "Seguimiento", Color = "#2ecc71" },
            new EtiquetaOption { Id = 4, Nombre = "Urgencia", Color = "#f39c12" },
            new EtiquetaOption { Id = 5, Nombre = "Laboratorio", Color = "#9b59b6" },
            new EtiquetaOption { Id = 6, Nombre = "Otro", Color = "#95a5a6" }
        };

        public static string GetColor(int etiquetaId)
        {
            var item = Opciones.Find(x => x.Id == etiquetaId);
            return item != null ? item.Color : "#667eea";
        }
    }
}
