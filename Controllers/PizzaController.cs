using Azure;
using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using la_mia_pizzeria_static.Models.Form;
using la_mia_pizzeria_static.Models.Repositories;
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

        DbPizzaRepository pizzaRepository;

        public PizzaController() : base()
        {
            db = new PizzeriaDbContext();

            pizzaRepository = new DbPizzaRepository();
        }

        //index
        public IActionResult Index()
        {
            List<Pizza> listPizza = pizzaRepository.All();

            return View(listPizza);
        }

        //details
        public IActionResult Details(int id)
        {

            Pizza pizza = pizzaRepository.GetById(id);

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
            //ricreazione della pagina se il modelstate non è valido
            if (!ModelState.IsValid)
            {
                formData.Categories = db.Categories.ToList();
                formData.Ingredients = new List<SelectListItem>();

                List<Ingredient> ingredientList = db.Ingredients.ToList();

                foreach (Ingredient ingredient in ingredientList)
                {
                    formData.Ingredients.Add(new SelectListItem(ingredient.Name, ingredient.Id.ToString()));
                }

                return View(formData);
            }

            pizzaRepository.Create(formData.Pizza, formData.SelectedIngredients);

            return RedirectToAction("Index");
        }

        //update page
        public IActionResult Update(int id)
        {
            Pizza pizza = pizzaRepository.GetById(id);

            if (pizza == null)
                return NotFound();

            PizzaForm formData = new PizzaForm();

            formData.Pizza = pizza;
            formData.Categories = db.Categories.ToList();
            formData.Ingredients = new List<SelectListItem>();

            List<Ingredient> ingredientsList = db.Ingredients.ToList();

            foreach(Ingredient ingredient in ingredientsList) 
            {
                formData.Ingredients.Add(new SelectListItem(
                    ingredient.Name,
                    ingredient.Id.ToString(),
                    pizza.Ingredients.Any(i => i.Id == ingredient.Id)
                    ));
            }

            return View(formData);
        }

        //update save
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaForm formData)
        {

            //ritorna la view in caso di invalid modelstate
            if (!ModelState.IsValid)
            {
                formData.Pizza.Id = id;
                //return View(postItem);
                formData.Categories = db.Categories.ToList();
                formData.Ingredients = new List<SelectListItem>();

                List<Ingredient> ingredientList = db.Ingredients.ToList();

                foreach (Ingredient ingredient in ingredientList)
                {
                    formData.Ingredients.Add(new SelectListItem(ingredient.Name, ingredient.Id.ToString()));
                }

                return View(formData);
            }


            // ---Metodo esplicito---

            Pizza pizzaItem = pizzaRepository.GetById(id);

            if (pizzaItem == null)
            {
                return NotFound();
            }

            pizzaRepository.Update(pizzaItem, formData.Pizza, formData.SelectedIngredients);

            return RedirectToAction("Index");
        }

        //delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            Pizza pizza = pizzaRepository.GetById(id);
            if (pizza == null)
            {
                return NotFound();
            }

            pizzaRepository.Delete(pizza);

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
