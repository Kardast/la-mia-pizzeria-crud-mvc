using la_mia_pizzeria_static.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Server;

namespace la_mia_pizzeria_static.Models.Repositories
{
    public class DbPizzaRepository : IDbPizzaRepository
    {
        private PizzeriaDbContext db;

        public DbPizzaRepository()
        {
            db = new PizzeriaDbContext();
        }

        public List<Pizza> All()
        {
            return db.Pizzas.Include(pizza => pizza.Category).Include(pizza => pizza.Ingredients).ToList();
        }

        public Pizza GetById(int id)
        {
            return db.Pizzas.Where(p => p.Id == id).Include("Category").Include("Ingredients").FirstOrDefault();
        }

        public void Create(Pizza pizza, List<int> selectedIngredients)
        {
            //associazione degli ingredienti selezionati dall'utente al modello
            pizza.Ingredients = new List<Ingredient>();

            foreach (int ingredientId in selectedIngredients)
            {
                Ingredient ingredient = db.Ingredients.Where(i => i.Id == ingredientId).FirstOrDefault();
                pizza.Ingredients.Add(ingredient);
            }

            db.Pizzas.Add(pizza);
            db.SaveChanges();
        }

        public void Update(Pizza pizza, Pizza formData, List<int>? selectedIngredients)
        {

            //creazione nuova list se null
            if (selectedIngredients == null)
            {
                selectedIngredients = new List<int>();
            }

            //aggiornamento dei singoli dettagli della pizza
            pizza.Name = formData.Name;
            pizza.Description = formData.Description;
            pizza.Image = formData.Image;
            pizza.Cost = formData.Cost;
            pizza.CategoryId = formData.CategoryId;

            //lista ingredienti viene svuotata
            pizza.Ingredients.Clear();


            //foreach su selected ingredients e popolazione pizza.ingredients 
            foreach (int ingredientId in selectedIngredients)
            {
                //query per ricavare gli ingredienti dal db
                Ingredient ingredient = db.Ingredients.Where(i => i.Id == ingredientId).FirstOrDefault();
                pizza.Ingredients.Add(ingredient);
            }

            //salvataggio su db in modo implicito

            db.SaveChanges();
        }

        public void Delete(Pizza pizza)
        {
            db.Pizzas.Remove(pizza);
            db.SaveChanges();
        }


        //METODO ALTERNATIVO CON OVERLOAD
        //public void Update(int id, Pizza formData, List<int>? selectedIngredients)
        //{
        //    Pizza pizzaItem = GetById(id);

        //    if(pizzaItem == null)
        //    {
        //        //throw new
        //    }

        //    this.Update(pizzaItem, formData, selectedIngredients);

        //}
    }
}
