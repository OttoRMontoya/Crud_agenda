using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Crud_agenda.Models;
using Newtonsoft.Json;

namespace Crud_agenda.Controllers
{
    public class AgendaController : Controller
    {
        private AgendaDbContext db = new AgendaDbContext();
        private const int DefaultUsuario = 1;

        public ActionResult Index()
        {
            ViewBag.EstatusList = db.AgendaEstatus
                .Where(x => x.Activo)
                .OrderBy(x => x.NombreEstatus)
                .ToList();
            ViewBag.Etiquetas = AgendaEtiquetas.Opciones;
            return View();
        }

        [HttpGet]
        public JsonResult GetEmpresas()
        {
            var list = db.AgendaEmpresa
                .Where(x => x.Activo)
                .OrderBy(x => x.NombreEmpresa)
                .Select(x => new { id = x.IdEmpresa, nombre = x.NombreEmpresa })
                .ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetClinicas(int? empresaId)
        {
            var query = db.AgendaClinica.Where(x => x.Activo);
            if (empresaId.HasValue && empresaId.Value > 0)
                query = query.Where(x => x.IdEmpresa == empresaId.Value);

            var list = query.OrderBy(x => x.NombreClinica)
                .Select(x => new { id = x.IdClinica, nombre = x.NombreClinica, empresaId = x.IdEmpresa })
                .ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDoctores(int? empresaId, int? clinicaId)
        {
            var query = db.AgendaDoctor.Where(x => x.Activo);

            if (clinicaId.HasValue && clinicaId.Value > 0)
            {
                var clinica = db.AgendaClinica.Find(clinicaId.Value);
                if (clinica != null)
                {
                    query = query.Where(x =>
                        x.IdEmpresa == clinica.IdEmpresa &&
                        (x.IdClinica == clinicaId.Value || x.IdClinica == null));
                }
            }
            else if (empresaId.HasValue && empresaId.Value > 0)
            {
                query = query.Where(x => x.IdEmpresa == empresaId.Value);
            }

            var list = query.OrderBy(x => x.NombreDoctor)
                .Select(x => new
                {
                    id = x.CodigoDoctor,
                    nombre = x.NombreDoctor,
                    especialidad = x.Especialidad,
                    empresaId = x.IdEmpresa,
                    clinicaId = x.IdClinica
                })
                .ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetClinicasFiltro()
        {
            var list = db.AgendaClinica
                .Where(x => x.Activo)
                .OrderBy(x => x.NombreClinica)
                .Select(x => new { id = x.IdClinica, nombre = x.NombreClinica })
                .ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetResources()
        {
            var list = db.AgendaClinica
                .Where(x => x.Activo)
                .OrderBy(x => x.NombreClinica)
                .Select(x => new
                {
                    id = x.IdClinica.ToString(),
                    title = x.NombreClinica,
                    empresaId = x.IdEmpresa
                })
                .ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDoctoresFiltro()
        {
            var list = db.AgendaDoctor
                .Where(x => x.Activo)
                .OrderBy(x => x.NombreDoctor)
                .Select(x => new
                {
                    id = x.CodigoDoctor,
                    nombre = x.NombreDoctor,
                    especialidad = x.Especialidad
                })
                .ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetEvents(DateTime start, DateTime end, int? codigoDoctor, int? clinica, string clinicas)
        {
            var query = db.Agenda.Where(x => x.Inicio < end && x.Fin > start);

            if (codigoDoctor.HasValue && codigoDoctor.Value > 0)
                query = query.Where(x => x.CodigoDoctor == codigoDoctor.Value);

            var clinicaIds = new List<int>();
            if (!string.IsNullOrWhiteSpace(clinicas))
            {
                clinicaIds = clinicas
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => int.TryParse(s.Trim(), out var id) ? id : 0)
                    .Where(id => id > 0)
                    .Distinct()
                    .ToList();
            }
            else if (clinica.HasValue && clinica.Value > 0)
            {
                clinicaIds.Add(clinica.Value);
            }

            if (clinicaIds.Any())
                query = query.Where(x => clinicaIds.Contains(x.Clinica));

            var citas = query.OrderBy(x => x.Inicio).ToList();
            var estatusColors = LoadEstatusColors();
            var citasClinicaIds = citas.Select(c => c.Clinica).Where(id => id > 0).Distinct().ToList();
            var doctorIds = citas.Select(c => c.CodigoDoctor).Where(id => id > 0).Distinct().ToList();
            var clinicaNames = db.AgendaClinica
                .Where(c => citasClinicaIds.Contains(c.IdClinica))
                .ToDictionary(c => c.IdClinica, c => c.NombreClinica);
            var doctors = db.AgendaDoctor
                .Where(d => doctorIds.Contains(d.CodigoDoctor))
                .ToDictionary(d => d.CodigoDoctor);

            var events = citas.Select(c => MapToEventDto(c, estatusColors, clinicaNames, doctors)).ToList();
            return Json(events, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCita(int id)
        {
            var cita = db.Agenda.Find(id);
            if (cita == null)
            {
                Response.StatusCode = (int)HttpStatusCode.NotFound;
                return Json(new { success = false, message = "Reservación no encontrada." }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = true, data = MapToViewModel(cita) }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveCita()
        {
            AgendaCitaViewModel model;
            try
            {
                Request.InputStream.Position = 0;
                using (var reader = new StreamReader(Request.InputStream))
                {
                    var json = reader.ReadToEnd();
                    model = JsonConvert.DeserializeObject<AgendaCitaViewModel>(json);
                }
            }
            catch
            {
                return Json(new { success = false, message = "Datos inválidos." });
            }

            if (model == null)
            {
                return Json(new { success = false, message = "Datos inválidos." });
            }

            if (string.IsNullOrWhiteSpace(model.Asunto))
            {
                return Json(new { success = false, message = "El asunto es requerido." });
            }

            if (string.IsNullOrWhiteSpace(model.Contacto))
            {
                return Json(new { success = false, message = "El contacto es requerido." });
            }

            if (model.Empresa <= 0)
            {
                return Json(new { success = false, message = "Debe seleccionar una empresa." });
            }

            if (model.Clinica <= 0)
            {
                return Json(new { success = false, message = "Debe seleccionar una habitación." });
            }

            if (model.Fin <= model.Inicio)
            {
                return Json(new { success = false, message = "La fecha de fin debe ser posterior al inicio." });
            }

            model.Inicio = AgendaDateHelper.EnsureWallClock(model.Inicio);
            model.Fin = AgendaDateHelper.EnsureWallClock(model.Fin);

            Agenda cita;

            if (model.IdAgendaCita > 0)
            {
                cita = db.Agenda.Find(model.IdAgendaCita);
                if (cita == null)
                {
                    return Json(new { success = false, message = "Reservación no encontrada." });
                }

                ApplyViewModel(cita, model);
                cita.UsuarioModifica = DefaultUsuario;
                cita.FechaModifica = DateTime.Now;
                db.Entry(cita).State = EntityState.Modified;
            }
            else
            {
                cita = new Agenda
                {
                    UsuarioCrea = DefaultUsuario,
                    FechaCrea = DateTime.Now,
                    UsuarioModifica = DefaultUsuario,
                    FechaModifica = DateTime.Now
                };
                ApplyViewModel(cita, model);
                db.Agenda.Add(cita);
            }

            db.SaveChanges();

            return Json(new
            {
                success = true,
                message = model.IdAgendaCita > 0 ? "Reservación actualizada correctamente." : "Reservación creada correctamente.",
                data = MapToEventDto(cita)
            });
        }

        [HttpPost]
        public JsonResult DeleteCita(int id)
        {
            var cita = db.Agenda.Find(id);
            if (cita == null)
            {
                return Json(new { success = false, message = "Reservación no encontrada." });
            }

            db.Agenda.Remove(cita);
            db.SaveChanges();

            return Json(new { success = true, message = "Reservación eliminada correctamente." });
        }

        [HttpPost]
        public JsonResult UpdateDates(int id, string inicio, string fin, bool todoelDia, int? clinica = null)
        {
            DateTime inicioDt;
            DateTime finDt;
            if (!AgendaDateHelper.TryParseWallClock(inicio, out inicioDt) ||
                !AgendaDateHelper.TryParseWallClock(fin, out finDt))
            {
                return Json(new { success = false, message = "Fechas inválidas." });
            }

            var cita = db.Agenda.Find(id);
            if (cita == null)
            {
                return Json(new { success = false, message = "Reservación no encontrada." });
            }

            if (finDt <= inicioDt)
            {
                return Json(new { success = false, message = "La fecha de fin debe ser posterior al inicio." });
            }

            cita.Inicio = inicioDt;
            cita.Fin = finDt;
            cita.TodoelDia = todoelDia;

            if (clinica.HasValue && clinica.Value > 0)
            {
                var clinicaEntity = db.AgendaClinica.Find(clinica.Value);
                if (clinicaEntity != null)
                {
                    cita.Clinica = clinicaEntity.IdClinica;
                    cita.Empresa = clinicaEntity.IdEmpresa;
                }
            }

            cita.UsuarioModifica = DefaultUsuario;
            cita.FechaModifica = DateTime.Now;

            db.Entry(cita).State = EntityState.Modified;
            db.SaveChanges();

            return Json(new { success = true, message = "Horario actualizado.", data = MapToEventDto(cita) });
        }

        private static void ApplyViewModel(Agenda cita, AgendaCitaViewModel model)
        {
            cita.IdCalendario = model.IdCalendario;
            cita.Empresa = model.Empresa;
            cita.CodigoDoctor = model.CodigoDoctor;
            cita.Codigo = model.Codigo;
            cita.Clinica = model.Clinica;
            cita.NoFichaIngreso = model.NoFichaIngreso;
            cita.Contacto = model.Contacto ?? string.Empty;
            cita.Inicio = model.Inicio;
            cita.Fin = model.Fin;
            cita.Asunto = model.Asunto ?? string.Empty;
            cita.Descripcion = model.Descripcion ?? string.Empty;
            cita.Lugar = model.Lugar ?? string.Empty;
            cita.Etiqueta = model.Etiqueta;
            cita.Estatus = model.Estatus;
            cita.TodoelDia = model.TodoelDia;
            cita.Telefono1 = model.Telefono1 ?? string.Empty;
            cita.Telefono2 = model.Telefono2 ?? string.Empty;
            cita.Celular = model.Celular ?? string.Empty;
            cita.EMail = model.EMail ?? string.Empty;
            cita.EnviaRecordatorio = model.EnviaRecordatorio;
        }

        private static AgendaCitaViewModel MapToViewModel(Agenda cita)
        {
            return new AgendaCitaViewModel
            {
                IdAgendaCita = cita.IdAgendaCita,
                IdCalendario = cita.IdCalendario,
                Empresa = cita.Empresa,
                CodigoDoctor = cita.CodigoDoctor,
                Codigo = cita.Codigo,
                Clinica = cita.Clinica,
                NoFichaIngreso = cita.NoFichaIngreso,
                Contacto = cita.Contacto,
                Inicio = cita.Inicio,
                Fin = cita.Fin,
                Asunto = cita.Asunto,
                Descripcion = cita.Descripcion,
                Lugar = cita.Lugar,
                Etiqueta = cita.Etiqueta,
                Estatus = cita.Estatus,
                TodoelDia = cita.TodoelDia,
                Telefono1 = cita.Telefono1,
                Telefono2 = cita.Telefono2,
                Celular = cita.Celular,
                EMail = cita.EMail,
                EnviaRecordatorio = cita.EnviaRecordatorio
            };
        }

        private static object MapToExtendedProps(Agenda cita, string nombreClinica = null)
        {
            return new
            {
                IdAgendaCita = cita.IdAgendaCita,
                IdCalendario = cita.IdCalendario,
                Empresa = cita.Empresa,
                CodigoDoctor = cita.CodigoDoctor,
                Codigo = cita.Codigo,
                Clinica = cita.Clinica,
                NombreClinica = nombreClinica,
                NoFichaIngreso = cita.NoFichaIngreso,
                Contacto = cita.Contacto,
                Inicio = AgendaDateHelper.FormatWallClock(cita.Inicio),
                Fin = AgendaDateHelper.FormatWallClock(cita.Fin),
                Asunto = cita.Asunto,
                Descripcion = cita.Descripcion,
                Lugar = cita.Lugar,
                Etiqueta = cita.Etiqueta,
                Estatus = cita.Estatus,
                TodoelDia = cita.TodoelDia,
                Telefono1 = cita.Telefono1,
                Telefono2 = cita.Telefono2,
                Celular = cita.Celular,
                EMail = cita.EMail,
                EnviaRecordatorio = cita.EnviaRecordatorio
            };
        }

        private Dictionary<int, string> LoadEstatusColors()
        {
            return db.AgendaEstatus.ToDictionary(
                e => e.IdEstatus,
                e => string.IsNullOrWhiteSpace(e.Color) ? "#667eea" : e.Color);
        }

        private string GetEstatusColor(int estatusId, Dictionary<int, string> estatusColors = null)
        {
            string color;
            if (estatusColors != null && estatusColors.TryGetValue(estatusId, out color))
                return color;

            var est = db.AgendaEstatus.Find(estatusId);
            return est != null && !string.IsNullOrWhiteSpace(est.Color) ? est.Color : "#667eea";
        }

        private AgendaEventDto MapToEventDto(
            Agenda cita,
            Dictionary<int, string> estatusColors = null,
            Dictionary<int, string> clinicaNames = null,
            Dictionary<int, AgendaDoctor> doctors = null)
        {
            var color = GetEstatusColor(cita.Estatus, estatusColors);
            AgendaDoctor doctor = null;
            if (doctors != null)
                doctors.TryGetValue(cita.CodigoDoctor, out doctor);
            else if (cita.CodigoDoctor > 0)
                doctor = db.AgendaDoctor.Find(cita.CodigoDoctor);

            string nombreClinica = null;
            if (clinicaNames != null)
                clinicaNames.TryGetValue(cita.Clinica, out nombreClinica);
            else if (cita.Clinica > 0)
            {
                var clinica = db.AgendaClinica.Find(cita.Clinica);
                nombreClinica = clinica?.NombreClinica;
            }

            var title = BuildAppointmentTitle(cita, doctor, nombreClinica);

            return new AgendaEventDto
            {
                id = cita.IdAgendaCita,
                title = title,
                start = AgendaDateHelper.FormatWallClock(cita.Inicio),
                end = AgendaDateHelper.FormatWallClock(cita.Fin),
                allDay = cita.TodoelDia,
                resourceId = cita.Clinica > 0 ? cita.Clinica.ToString() : null,
                backgroundColor = color,
                borderColor = color,
                textColor = "#ffffff",
                extendedProps = MapToExtendedProps(cita, nombreClinica)
            };
        }

        private static string BuildAppointmentTitle(Agenda cita, AgendaDoctor doctor, string nombreClinica)
        {
            var title = string.Empty;
            if (!string.IsNullOrWhiteSpace(nombreClinica))
                title += nombreClinica + " ";
            title += "#" + cita.IdAgendaCita + " - " + cita.Asunto + " - " + cita.Contacto;
            if (doctor != null)
                title += " (" + doctor.NombreDoctor + ")";
            return title;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
