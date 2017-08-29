using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using ZooFeeding2.DAL;
using ZooFeeding2.Models;
using ZooFeeding2.ViewModel;

namespace ZooFeeding2.Controllers
{
    public class AnimalsController : Controller
    {
        private UnitOfWork unitOfWork = new UnitOfWork();

        // GET: Animals
        public ViewResult Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            var animals = from a in unitOfWork.AnimalRepository.Get() select a;

            switch (sortOrder)
            {
                case "name_desc":
                    animals = animals.OrderByDescending(a => a.AnimalName);
                    break;
                default:
                    animals = animals.OrderBy(a => a.AnimalName);
                    break;
            }
            return View(animals.ToList());
        }

        // GET: Animals/Details/5
        public ViewResult Details(int id)
        {
            Animal animal = unitOfWork.AnimalRepository.GetById(id);
            return View(animal);
        }

        // GET: Animals/Create
        public ActionResult Create()
        {
            var animal = new Animal();
            animal.Relationships = new List<Relationship>();
            PopulateAssignedFoodData(animal);
            return View();
        }

        // POST: Animals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnimalId,AnimalName")] Animal animal, string[] selectedFoods)
        {
            if (selectedFoods != null)
            {
                foreach (var food in selectedFoods)
                {
                    var foodsToAdd = unitOfWork.FoodRepository.GetById(int.Parse(food));
                    unitOfWork.RelationshipRepository.Insert(new Relationship { AnimalId = animal.AnimalId, FoodId = int.Parse(food) });
                }
            }
            if (ModelState.IsValid)
            {
                unitOfWork.AnimalRepository.Insert(animal);
                unitOfWork.Save();
                return RedirectToAction("Index");
            }
            PopulateAssignedFoodData(animal);
            return View(animal);
        }

        // GET: Animals/Edit/5
        public ActionResult Edit(int id)
        {
            Animal animal = unitOfWork.AnimalRepository.Get(includeProperties: "Relationships", filter: i => i.AnimalId == id).Single();
            PopulateAssignedFoodData(animal);
            return View(animal);
        }

        private void PopulateAssignedFoodData(Animal animal)
        {
            var allFoods = unitOfWork.FoodRepository.Get();
            var animalFoods = new HashSet<int>(animal.Relationships.Select(f => f.FoodId));
            var viewModel = new List<AssignedFoodData>();
            foreach (var food in allFoods)
            {
                viewModel.Add(new AssignedFoodData
                {
                    FoodId = food.FoodId,
                    FoodName = food.FoodName,
                    Assigned = animalFoods.Contains(food.FoodId)
                });
            }
            ViewBag.Foods = viewModel;
        }

        // POST: Animals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, string[] selectedFoods)
        {
            var animalToUpdate = unitOfWork.AnimalRepository.Get(includeProperties: "Relationships", filter: i => i.AnimalId == id).Single();

            if (TryUpdateModel(animalToUpdate, "", new string[] { "AnimalId", "AnimalName" }))
            {
                try
                {
                    UpdateAnimalFoods(selectedFoods, animalToUpdate);

                    unitOfWork.Save();

                    return RedirectToAction("Index");
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
                }
            }
            PopulateAssignedFoodData(animalToUpdate);
            return View(animalToUpdate);
        }

        private void UpdateAnimalFoods(string[] selectedFoods, Animal animalToUpdate)
        {
            if (selectedFoods == null)
            {
                animalToUpdate.Relationships = new List<Relationship>();
                return;
            }

            var selectedFoodsHS = new HashSet<string>(selectedFoods);
            var animalFoods = new HashSet<int>(animalToUpdate.Relationships.Select(f => f.FoodId));

            foreach (var food in unitOfWork.FoodRepository.Get())
            {
                if (selectedFoodsHS.Contains(food.FoodId.ToString()))
                {
                    if (!animalFoods.Contains(food.FoodId))
                    {
                        unitOfWork.RelationshipRepository.Insert(new Relationship { AnimalId = animalToUpdate.AnimalId, FoodId = food.FoodId});
                    }
                }
                else
                {
                    if (animalFoods.Contains(food.FoodId))
                    {
                        var relationshipToRemove = unitOfWork.RelationshipRepository.Get(filter: i => i.AnimalId == animalToUpdate.AnimalId && i.FoodId == food.FoodId).Single();
                        unitOfWork.RelationshipRepository.Delete(relationshipToRemove);
                    }
                }
            }
        }

        // GET: Animals/Delete/5
        public ActionResult Delete(int id)
        {
            Animal animal = unitOfWork.AnimalRepository.GetById(id);
            return View(animal);
        }

        // POST: Animals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Animal animal = unitOfWork.AnimalRepository.GetById(id);
            unitOfWork.AnimalRepository.Delete(animal);
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
