using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Crud_agenda.Models;

namespace Crud_agenda.Controllers
{
    public class AgendaEstatusController : Controller
    {
        private AgendaDbContext db = new AgendaDbContext();

        // GET: AgendaEstatus
        public ActionResult Index()
        {
            var agendaEstatus = db.AgendaEstatus.OrderByDescending(x => x.FechaCreacion).ToList();
            return View(agendaEstatus);
        }

        // GET: AgendaEstatus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgendaEstatus agendaEstatus = db.AgendaEstatus.Find(id);
            if (agendaEstatus == null)
            {
                return HttpNotFound();
            }
            return View(agendaEstatus);
        }

        // GET: AgendaEstatus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AgendaEstatus/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "NombreEstatus,Descripcion,Activo")] AgendaEstatus agendaEstatus)
        {
            if (ModelState.IsValid)
            {
                agendaEstatus.FechaCreacion = DateTime.Now;
                agendaEstatus.Activo = true;
                db.AgendaEstatus.Add(agendaEstatus);
                db.SaveChanges();
                TempData["Message"] = "Estado creado exitosamente.";
                return RedirectToAction("Index");
            }
            return View(agendaEstatus);
        }

        // GET: AgendaEstatus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgendaEstatus agendaEstatus = db.AgendaEstatus.Find(id);
            if (agendaEstatus == null)
            {
                return HttpNotFound();
            }
            return View(agendaEstatus);
        }

        // POST: AgendaEstatus/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdEstatus,NombreEstatus,Descripcion,Activo")] AgendaEstatus agendaEstatus)
        {
            if (ModelState.IsValid)
            {
                var existingEstatus = db.AgendaEstatus.Find(agendaEstatus.IdEstatus);
                if (existingEstatus != null)
                {
                    existingEstatus.NombreEstatus = agendaEstatus.NombreEstatus;
                    existingEstatus.Descripcion = agendaEstatus.Descripcion;
                    existingEstatus.Activo = agendaEstatus.Activo;
                    existingEstatus.FechaModificacion = DateTime.Now;

                    db.Entry(existingEstatus).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Estado actualizado exitosamente.";
                    return RedirectToAction("Index");
                }
            }
            return View(agendaEstatus);
        }

        // GET: AgendaEstatus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgendaEstatus agendaEstatus = db.AgendaEstatus.Find(id);
            if (agendaEstatus == null)
            {
                return HttpNotFound();
            }
            return View(agendaEstatus);
        }

        // POST: AgendaEstatus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AgendaEstatus agendaEstatus = db.AgendaEstatus.Find(id);
            if (agendaEstatus != null)
            {
                db.AgendaEstatus.Remove(agendaEstatus);
                db.SaveChanges();
                TempData["Message"] = "Estado eliminado exitosamente.";
            }
            return RedirectToAction("Index");
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
