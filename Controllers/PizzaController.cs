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
        public IActionResult Index()
        {
            List<Pizza> listPizza = db.Pizzas.ToList();

            //qualche altra cosa...

            return View(listPizza);
        }

        public IActionResult Details(int id)
        {

            Pizza pizza = db.Pizzas.Where(p => p.Id == id).FirstOrDefault();

            return View(pizza);
        }

        public IActionResult Create()
        {
            return View();
        }

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
    }
}
//DA PROVARE PER IL DISCORSO DELLE LETTERE NEL COSTO
//if (ModelState["Price"].Errors.Count > 0)
//{
//    ModelState["Price"].Errors.Clear();
//    ModelState["Price"].Errors.Add("Il prezzo deve essere compreso tra 1 e 30");
//}
