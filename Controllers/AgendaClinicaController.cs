using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Crud_agenda.Models;

namespace Crud_agenda.Controllers
{
    public class AgendaClinicaController : Controller
    {
        private AgendaDbContext db = new AgendaDbContext();

        private void LoadEmpresas(int? selected = null)
        { 
            ViewBag.IdEmpresa = new SelectList(
                db.AgendaEmpresa.Where(x => x.Activo).OrderBy(x => x.NombreEmpresa),
                "IdEmpresa", "NombreEmpresa", selected);
        }

        public ActionResult Index()
        {
            var list = db.AgendaClinica
                .Include(x => x.Empresa)
                .OrderBy(x => x.NombreClinica)
                .ToList();
            return View(list);
        }

        public ActionResult Create()
        {
            LoadEmpresas();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdEmpresa,NombreClinica,Direccion,Activo")] AgendaClinica item)
        {
            if (ModelState.IsValid)
            {
                item.FechaCreacion = DateTime.Now;
                item.Activo = true;
                db.AgendaClinica.Add(item);
                db.SaveChanges();
                TempData["Message"] = "Clínica creada exitosamente.";
                return RedirectToAction("Index");
            }
            LoadEmpresas(item.IdEmpresa);
            return View(item);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var item = db.AgendaClinica.Find(id);
            if (item == null) return HttpNotFound();
            LoadEmpresas(item.IdEmpresa);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdClinica,IdEmpresa,NombreClinica,Direccion,Activo")] AgendaClinica item)
        {
            if (ModelState.IsValid)
            {
                var existing = db.AgendaClinica.Find(item.IdClinica);
                if (existing != null)
                {
                    existing.IdEmpresa = item.IdEmpresa;
                    existing.NombreClinica = item.NombreClinica;
                    existing.Direccion = item.Direccion;
                    existing.Activo = item.Activo;
                    existing.FechaModificacion = DateTime.Now;
                    db.Entry(existing).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Clínica actualizada exitosamente.";
                    return RedirectToAction("Index");
                }
            }
            LoadEmpresas(item.IdEmpresa);
            return View(item);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var item = db.AgendaClinica.Include(x => x.Empresa).FirstOrDefault(x => x.IdClinica == id);
            if (item == null) return HttpNotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var item = db.AgendaClinica.Find(id);
            if (item != null)
            {
                db.AgendaClinica.Remove(item);
                db.SaveChanges();
                TempData["Message"] = "Clínica eliminada exitosamente.";
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) db.Dispose();
            base.Dispose(disposing);
        }
    }
}
