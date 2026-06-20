using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Crud_agenda.Models;

namespace Crud_agenda.Controllers
{
    public class AgendaDoctorController : Controller
    {
        private AgendaDbContext db = new AgendaDbContext();

        private void LoadCatalogos(int? empresaId = null, int? clinicaId = null)
        {
            ViewBag.IdEmpresa = new SelectList(
                db.AgendaEmpresa.Where(x => x.Activo).OrderBy(x => x.NombreEmpresa),
                "IdEmpresa", "NombreEmpresa", empresaId);

            var clinicas = db.AgendaClinica.Where(x => x.Activo);
            if (empresaId.HasValue && empresaId > 0)
                clinicas = clinicas.Where(x => x.IdEmpresa == empresaId);

            ViewBag.IdClinica = new SelectList(
                clinicas.OrderBy(x => x.NombreClinica),
                "IdClinica", "NombreClinica", clinicaId);
        }

        public ActionResult Index()
        {
            var list = db.AgendaDoctor
                .Include(x => x.Empresa)
                .Include(x => x.Clinica)
                .OrderBy(x => x.NombreDoctor)
                .ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            LoadCatalogos();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEmpresa,IdClinica,NombreDoctor,Especialidad,Activo")] AgendaDoctor item)
        {
            if (ModelState.IsValid)
            {
                item.FechaCreacion = DateTime.Now;
                item.Activo = true;
                db.AgendaDoctor.Add(item);
                db.SaveChanges();
                TempData["Message"] = "Doctor creado exitosamente.";
                return RedirectToAction("Index");
            }
            LoadCatalogos(item.IdEmpresa, item.IdClinica);
            return View(item);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var item = db.AgendaDoctor.Find(id);
            if (item == null) return HttpNotFound();
            LoadCatalogos(item.IdEmpresa, item.IdClinica);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodigoDoctor,IdEmpresa,IdClinica,NombreDoctor,Especialidad,Activo")] AgendaDoctor item)
        {
            if (ModelState.IsValid)
            {
                var existing = db.AgendaDoctor.Find(item.CodigoDoctor);
                if (existing != null)
                {
                    existing.IdEmpresa = item.IdEmpresa;
                    existing.IdClinica = item.IdClinica;
                    existing.NombreDoctor = item.NombreDoctor;
                    existing.Especialidad = item.Especialidad;
                    existing.Activo = item.Activo;
                    existing.FechaModificacion = DateTime.Now;
                    db.Entry(existing).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Doctor actualizado exitosamente.";
                    return RedirectToAction("Index");
                }
            }
            LoadCatalogos(item.IdEmpresa, item.IdClinica);
            return View(item);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var item = db.AgendaDoctor
                .Include(x => x.Empresa)
                .Include(x => x.Clinica)
                .FirstOrDefault(x => x.CodigoDoctor == id);
            if (item == null) return HttpNotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var item = db.AgendaDoctor.Find(id);
            if (item != null)
            {
                db.AgendaDoctor.Remove(item);
                db.SaveChanges();
                TempData["Message"] = "Doctor eliminado exitosamente.";
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetClinicasByEmpresa(int empresaId)
        {
            var list = db.AgendaClinica
                .Where(x => x.Activo && x.IdEmpresa == empresaId)
                .OrderBy(x => x.NombreClinica)
                .Select(x => new { id = x.IdClinica, nombre = x.NombreClinica })
                .ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
