using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Crud_agenda.Models;

namespace Crud_agenda.Controllers
{
    public class PacientesController : Controller
    {
        private AgendaDbContext db = new AgendaDbContext();
        private const int DefaultUsuario = 1;

        public ActionResult Index(string q)
        {
            ViewBag.Search = q;
            var pacientes = db.Pacientes.OrderByDescending(p => p.FechaCrea).ToList();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim().ToLower();
                pacientes = pacientes.Where(p => CoincideBusqueda(p, term)).ToList();
            }

            return View(pacientes);
        }

        public ActionResult Details(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var item = db.Pacientes.Find(id);
            if (item == null) return HttpNotFound();
            return View(item);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodigoPaciente,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,ApellidoCasada,FechaNacimiento,LugarNacimiento,NoIdentificacion,ExtendidaMunicipalidad,NoPasaporte,Direccion1,Zona,Direccion2,Telefono,Celular,TelefonoTrabajo,EMail,ResponsableCuenta")] Paciente item)
        {
            if (ModelState.IsValid)
            {
                item.UsuarioCrea = DefaultUsuario;
                item.UsuarioModifica = DefaultUsuario;
                item.FechaCrea = DateTime.Now;
                item.FechaModifica = DateTime.Now;
                NormalizarCampos(item);
                db.Pacientes.Add(item);
                db.SaveChanges();
                TempData["Message"] = "Cliente creado exitosamente.";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var item = db.Pacientes.Find(id);
            if (item == null) return HttpNotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codigo,CodigoPaciente,PrimerNombre,SegundoNombre,PrimerApellido,SegundoApellido,ApellidoCasada,FechaNacimiento,LugarNacimiento,NoIdentificacion,ExtendidaMunicipalidad,NoPasaporte,Direccion1,Zona,Direccion2,Telefono,Celular,TelefonoTrabajo,EMail,ResponsableCuenta")] Paciente item)
        {
            if (ModelState.IsValid)
            {
                var existing = db.Pacientes.Find(item.Codigo);
                if (existing != null)
                {
                    existing.CodigoPaciente = item.CodigoPaciente;
                    existing.PrimerNombre = item.PrimerNombre;
                    existing.SegundoNombre = item.SegundoNombre;
                    existing.PrimerApellido = item.PrimerApellido;
                    existing.SegundoApellido = item.SegundoApellido;
                    existing.ApellidoCasada = item.ApellidoCasada;
                    existing.FechaNacimiento = item.FechaNacimiento;
                    existing.LugarNacimiento = item.LugarNacimiento;
                    existing.NoIdentificacion = item.NoIdentificacion;
                    existing.ExtendidaMunicipalidad = item.ExtendidaMunicipalidad;
                    existing.NoPasaporte = item.NoPasaporte;
                    existing.Direccion1 = item.Direccion1;
                    existing.Zona = item.Zona;
                    existing.Direccion2 = item.Direccion2;
                    existing.Telefono = item.Telefono;
                    existing.Celular = item.Celular;
                    existing.TelefonoTrabajo = item.TelefonoTrabajo;
                    existing.EMail = item.EMail;
                    existing.ResponsableCuenta = item.ResponsableCuenta;
                    existing.UsuarioModifica = DefaultUsuario;
                    existing.FechaModifica = DateTime.Now;
                    NormalizarCampos(existing);
                    db.Entry(existing).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Cliente actualizado exitosamente.";
                    return RedirectToAction("Index");
                }
            }
            return View(item);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var item = db.Pacientes.Find(id);
            if (item == null) return HttpNotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var item = db.Pacientes.Find(id);
            if (item != null)
            {
                db.Pacientes.Remove(item);
                db.SaveChanges();
                TempData["Message"] = "Cliente eliminado exitosamente.";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult Search(string q, int limit = 30)
        {
            var pacientes = db.Pacientes.OrderByDescending(p => p.FechaCrea).ToList();

            if (!string.IsNullOrWhiteSpace(q))
            {
                var term = q.Trim().ToLower();
                pacientes = pacientes.Where(p => CoincideBusqueda(p, term)).ToList();
            }

            var list = pacientes.Take(limit).Select(MapToDto).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPaciente(int id)
        {
            var p = db.Pacientes.Find(id);
            if (p == null)
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            return Json(new { success = true, data = MapToDto(p) }, JsonRequestBehavior.AllowGet);
        }

        private static bool CoincideBusqueda(Paciente p, string term)
        {
            return (Paciente.Trim(p.CodigoPaciente) ?? "").ToLower().Contains(term)
                || p.NombreCompleto.ToLower().Contains(term)
                || (Paciente.Trim(p.NoIdentificacion) ?? "").ToLower().Contains(term)
                || (Paciente.Trim(p.Telefono) ?? "").Contains(term)
                || (Paciente.Trim(p.Celular) ?? "").Contains(term)
                || (Paciente.Trim(p.EMail) ?? "").ToLower().Contains(term);
        }

        private static object MapToDto(Paciente p)
        {
            int ficha;
            int.TryParse(Paciente.Trim(p.CodigoPaciente), out ficha);

            return new
            {
                codigo = p.Codigo,
                codigoPaciente = Paciente.Trim(p.CodigoPaciente),
                nombreCompleto = p.NombreCompleto,
                contacto = p.NombreCompleto.Length > 100 ? p.NombreCompleto.Substring(0, 100) : p.NombreCompleto,
                telefono1 = Paciente.Trim(p.Telefono) ?? "",
                telefono2 = Paciente.Trim(p.TelefonoTrabajo) ?? "",
                celular = Paciente.Trim(p.Celular) ?? "",
                eMail = Paciente.Trim(p.EMail) ?? "",
                noIdentificacion = Paciente.Trim(p.NoIdentificacion) ?? "",
                noFichaIngreso = ficha
            };
        }

        private static void NormalizarCampos(Paciente p)
        {
            p.CodigoPaciente = Paciente.Trim(p.CodigoPaciente) ?? "";
            p.PrimerNombre = Paciente.Trim(p.PrimerNombre) ?? "";
            p.SegundoNombre = Paciente.Trim(p.SegundoNombre);
            p.PrimerApellido = Paciente.Trim(p.PrimerApellido) ?? "";
            p.SegundoApellido = Paciente.Trim(p.SegundoApellido);
            p.ApellidoCasada = Paciente.Trim(p.ApellidoCasada);
            p.LugarNacimiento = Paciente.Trim(p.LugarNacimiento);
            p.NoIdentificacion = Paciente.Trim(p.NoIdentificacion);
            p.ExtendidaMunicipalidad = Paciente.Trim(p.ExtendidaMunicipalidad);
            p.NoPasaporte = Paciente.Trim(p.NoPasaporte);
            p.Direccion1 = Paciente.Trim(p.Direccion1);
            p.Direccion2 = Paciente.Trim(p.Direccion2);
            p.Telefono = Paciente.Trim(p.Telefono);
            p.Celular = Paciente.Trim(p.Celular);
            p.TelefonoTrabajo = Paciente.Trim(p.TelefonoTrabajo);
            p.EMail = Paciente.Trim(p.EMail);
            p.ResponsableCuenta = Paciente.Trim(p.ResponsableCuenta);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
