using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PFSList.Models;
using PagedList;

namespace PFSList.Controllers
{
    public class CarsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Cars
        public async Task<ActionResult> Index(string SortOrder, string ProducerName, string CarName, int? Page)
        {
            var cars = db.Cars.Include(c => c.Producer);

            ViewBag.ProducerNameFilter = ProducerName;
            ViewBag.CarNameFilter = CarName;

            ViewBag.ProducerNameSort = SortOrder == "ProducerName" ? "ProducerNameDesc" : "ProducerName";
            ViewBag.CarNameSort = SortOrder == "CarName" ? "CarNameDesc" : "CarName";
            ViewBag.CostSort = SortOrder == "Cost" ? "CostDesc" : "Cost";

            if(!String.IsNullOrEmpty(ProducerName))
            {
                cars = cars.Where(c => c.Producer.Name.Contains(ProducerName));
            }
            if (!String.IsNullOrEmpty(CarName))
            {
                cars = cars.Where(c => c.Name.Contains(CarName));
            }

            switch (SortOrder)
            {
                case "ProducerName":
                    cars = cars.OrderBy(c => c.Producer.Name);
                    break;
                case "ProducerNameDesc":
                    cars = cars.OrderByDescending(c => c.Producer.Name);
                    break;
                case "CarName":
                    cars = cars.OrderBy(c => c.Name);
                    break;
                case "CarNameDesc":
                    cars = cars.OrderByDescending(c => c.Name);
                    break;
                case "Cost":
                    cars = cars.OrderBy(c => c.Cost);
                    break;
                case "CostDesc":
                    cars = cars.OrderByDescending(c => c.Cost);
                    break;
                default:
                    cars = cars.OrderBy(c => c.Id);
                    break;
            }

            ViewBag.ProducerName = new SelectList(db.Producers.OrderBy(p => p.Name).ToList(), "Name", "Name");

            int PageSize = 2;
            int PageNumber = (Page ?? 1);

            //return View(await cars.ToListAsync());
            return View(cars.ToPagedList(PageNumber, PageSize));
        }

        // GET: Cars/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = await db.Cars.FindAsync(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // GET: Cars/Create
        public ActionResult Create()
        {
            ViewBag.ProducerId = new SelectList(db.Producers, "Id", "Name");
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,ProducerId,Name,Cost")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Cars.Add(car);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.ProducerId = new SelectList(db.Producers, "Id", "Name", car.ProducerId);
            return View(car);
        }

        // GET: Cars/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = await db.Cars.FindAsync(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProducerId = new SelectList(db.Producers, "Id", "Name", car.ProducerId);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,ProducerId,Name,Cost")] Car car)
        {
            if (ModelState.IsValid)
            {
                db.Entry(car).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.ProducerId = new SelectList(db.Producers, "Id", "Name", car.ProducerId);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Car car = await db.Cars.FindAsync(id);
            if (car == null)
            {
                return HttpNotFound();
            }
            return View(car);
        }

        // POST: Cars/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Car car = await db.Cars.FindAsync(id);
            db.Cars.Remove(car);
            await db.SaveChangesAsync();
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
