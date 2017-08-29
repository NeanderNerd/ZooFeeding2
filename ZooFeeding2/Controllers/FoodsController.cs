using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ZooFeeding2.DAL;
using ZooFeeding2.Models;

namespace ZooFeeding2.Controllers
{
    public class FoodsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Foods
        public ViewResult Index()
        {
            var foods = unitOfWork.FoodRepository.Get();
            return View(foods.ToList());
        }

        // GET: Foods/Details/5
        public ViewResult Details(int id)
        {
            Food food = unitOfWork.FoodRepository.GetById(id);
            return View(food);
        }

        // GET: Foods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FoodId,FoodName")] Food food)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.FoodRepository.Insert(food);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }

            return View(food);
        }

        // GET: Foods/Edit/5
        public ActionResult Edit(int id)
        {
            Food food = unitOfWork.FoodRepository.GetById(id);
            return View(food);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FoodId,FoodName")] Food food)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    unitOfWork.FoodRepository.Update(food);
                    unitOfWork.Save();
                    return RedirectToAction("Index");
                }
            }
            catch (DataException)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
            }
            return View(food);
        }

        // GET: Foods/Delete/5
        public ActionResult Delete(int id)
        {
            Food food = unitOfWork.FoodRepository.GetById(id);
            return View(food);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Food food = unitOfWork.FoodRepository.GetById(id);
            unitOfWork.FoodRepository.Delete(food);
            unitOfWork.Save();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
