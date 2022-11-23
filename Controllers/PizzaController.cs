using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Models.Form;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Server;

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
            List<Pizza> listPizza = db.Pizzas.Include(pizza => pizza.Category).ToList();

            return View(listPizza);
        }

        //details
        public IActionResult Details(int id)
        {

            Pizza pizza = db.Pizzas.Where(p => p.Id == id).Include("Category").FirstOrDefault();

            return View(pizza);
        }

        //create page
        public IActionResult Create()
        {
            PizzaForm formData = new PizzaForm();

            formData.Pizza = new Pizza();
            formData.Categories = db.Categories.ToList();

            return View(formData);
        }

        //create save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaForm formData)
        {
            if (!ModelState.IsValid)
            {
                formData.Categories = db.Categories.ToList();
                return View(formData);
            }

            db.Pizzas.Add(formData.Pizza);
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        //update page
        public IActionResult Update(int id)
        {
            Pizza pizza = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizza == null)
                return NotFound();

            PizzaForm formData = new PizzaForm();

            formData.Pizza = pizza;
            formData.Categories = db.Categories.ToList();

            return View(formData);
        }

        //update save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaForm formData)
        {

            if (!ModelState.IsValid)
            {
                formData.Categories = db.Categories.ToList();
                return View(formData);
            }

            Pizza pizzaItem = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            if (pizzaItem == null)
            {
                return NotFound();
            }

            pizzaItem.Name = formData.Pizza.Name;
            pizzaItem.Description = formData.Pizza.Description;
            pizzaItem.Image = formData.Pizza.Image;
            pizzaItem.Cost = formData.Pizza.Cost;
            pizzaItem.CategoryId = formData.Pizza.CategoryId;

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
