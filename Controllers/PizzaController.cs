using Azure;
using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Models.Form;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.SqlServer.Server;
using System.Diagnostics;

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

            Pizza pizza = db.Pizzas.Where(p => p.Id == id).Include("Category").Include("Ingredients").FirstOrDefault();

            if(pizza == null)
            {
                return NotFound();
            }

            return View(pizza);
        }

        //create page
        public IActionResult Create()
        {
            PizzaForm formData = new PizzaForm();

            formData.Pizza = new Pizza();
            formData.Categories = db.Categories.ToList();
            formData.Ingredients = new List<SelectListItem>();

            //creazione lista ingredienti dal db per passarli al create
            List<Ingredient> ingredientList = db.Ingredients.ToList();

            foreach (Ingredient ingredient in ingredientList)
            {
                formData.Ingredients.Add(new SelectListItem(ingredient.Name, ingredient.Id.ToString()));
            }

            return View(formData);
        }

        //create save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaForm formData)
        {
            if (!ModelState.IsValid)
            {
                //ricreazione della pagina se il modelstate non è valido
                formData.Categories = db.Categories.ToList();
                formData.Ingredients = new List<SelectListItem>();

                List<Ingredient> ingredientList = db.Ingredients.ToList();

                foreach (Ingredient ingredient in ingredientList)
                {
                    formData.Ingredients.Add(new SelectListItem(ingredient.Name, ingredient.Id.ToString()));
                }

                return View(formData);
            }

            //associazione degli ingredienti selezionati dall'utente al modello
            formData.Pizza.Ingredients = new List<Ingredient>();

            foreach (int ingredientId in formData.SelectedIngredients)
            {
                Ingredient ingredient = db.Ingredients.Where(i => i.Id == ingredientId).FirstOrDefault();
                formData.Pizza.Ingredients.Add(ingredient);
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
            // ---Metodo implicito---

            //da mettere qui per evitare problemi con update nello scenario seguente
            //1. dati invalidi
            //2. dati validi
            formData.Pizza.Id = id;

            if (!ModelState.IsValid)
            {
                //return View(postItem);
                formData.Categories = db.Categories.ToList();
                return View(formData);
            }
            db.Pizzas.Update(formData.Pizza);

            // ---Metodo esplicito---

            //if (!ModelState.IsValid)
            //{
            //    formData.Categories = db.Categories.ToList();
            //    return View(formData);
            //}

            //Pizza pizzaItem = db.Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            //if (pizzaItem == null)
            //{
            //    return NotFound();
            //}

            //pizzaItem.Name = formData.Pizza.Name;
            //pizzaItem.Description = formData.Pizza.Description;
            //pizzaItem.Image = formData.Pizza.Image;
            //pizzaItem.Cost = formData.Pizza.Cost;
            //pizzaItem.CategoryId = formData.Pizza.CategoryId;

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
