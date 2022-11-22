using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        PizzeriaDbContext db;

        public PizzaController() : base()
        {
            db = new PizzeriaDbContext();
        }

        //index
        public IActionResult Index()
        {
            List<Pizza> listPizza = db.Pizzas.ToList();

            return View(listPizza);
        }

        //details
        public IActionResult Details(int id)
        {

            Pizza pizza = db.Pizzas.Where(p => p.Id == id).FirstOrDefault();

            return View(pizza);
        }

        //create page
        public IActionResult Create()
        {
            return View();
        }

        //create save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Pizza pizza)
        {
            if (!ModelState.IsValid)
            {
                //return View(post);
                return View();
            }

            db.Pizzas.Add(pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //update page
        public IActionResult Update(int id)
        {
            Pizza pizza = db.Pizzas.Where(p => p.Id == id).FirstOrDefault();

            if (pizza == null)
                return NotFound();

            return View(pizza);
        }

        //update save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, Pizza formData)
        {

            if (!ModelState.IsValid)
            {
                return View(formData);
            }

            Pizza pizza = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizza == null)
            {
                return NotFound();
            }

            pizza.Name = formData.Name;
            pizza.Description = formData.Description;
            pizza.Image = formData.Image;
            pizza.Cost = formData.Cost;

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Pizza pizza = db.Pizzas.Where(p => p.Id == id).FirstOrDefault();
            if (pizza == null)
            {
                return NotFound();
            }
            db.Pizzas.Remove(pizza);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
//DA PROVARE PER IL DISCORSO DELLE LETTERE NEL COSTO
//if (ModelState["Price"].Errors.Count > 0)
//{
//    ModelState["Price"].Errors.Clear();
//    ModelState["Price"].Errors.Add("Il prezzo deve essere compreso tra 1 e 30");
//}
