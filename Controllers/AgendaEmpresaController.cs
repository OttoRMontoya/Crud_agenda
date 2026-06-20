using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Crud_agenda.Models;

namespace Crud_agenda.Controllers
{
    public class AgendaEmpresaController : Controller
    {
        private AgendaDbContext db = new AgendaDbContext();

        public ActionResult Index()
        {
            return View(db.AgendaEmpresa.OrderBy(x => x.NombreEmpresa).ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NombreEmpresa,Activo")] AgendaEmpresa item)
        {
            if (ModelState.IsValid)
            {
                item.FechaCreacion = DateTime.Now;
                item.Activo = true;
                db.AgendaEmpresa.Add(item);
                db.SaveChanges();
                TempData["Message"] = "Empresa creada exitosamente.";
                return RedirectToAction("Index");
            }
            return View(item);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var item = db.AgendaEmpresa.Find(id);
            if (item == null) return HttpNotFound();
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEmpresa,NombreEmpresa,Activo")] AgendaEmpresa item)
        {
            if (ModelState.IsValid)
            {
                var existing = db.AgendaEmpresa.Find(item.IdEmpresa);
                if (existing != null)
                {
                    existing.NombreEmpresa = item.NombreEmpresa;
                    existing.Activo = item.Activo;
                    existing.FechaModificacion = DateTime.Now;
                    db.Entry(existing).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Empresa actualizada exitosamente.";
                    return RedirectToAction("Index");
                }
            }
            return View(item);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var item = db.AgendaEmpresa.Find(id);
            if (item == null) return HttpNotFound();
            return View(item);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var item = db.AgendaEmpresa.Find(id);
            if (item != null)
            {
                db.AgendaEmpresa.Remove(item);
                db.SaveChanges();
                TempData["Message"] = "Empresa eliminada exitosamente.";
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
