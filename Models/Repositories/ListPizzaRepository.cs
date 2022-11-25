namespace la_mia_pizzeria_static.Models.Repositories
{
    public class ListPizzaRepository : IDbPizzaRepository
    {
        public static List<Pizza> Pizzas = new List<Pizza>();
        
        public ListPizzaRepository() 
        {
            //non lo possiamo fare perché ogni nuova possibile istanza ci cancella la lista
            //Pizzas = new List<Pizza>();
        }
        public List<Pizza> All()
        {
            return Pizzas;
        }

        public void Create(Pizza pizza, List<int> selectedIngredients)
        {
            //simuliamo la primary key
            pizza.Id = Pizzas.Count;
            pizza.Category = new Category() { Id = 1, Name = "Fake categroy" };

            //simulazione da implentare con ListTagRepository
            pizza.Ingredients = new List<Ingredient>();
            IngredientToPizza(pizza, selectedIngredients);
            //fine simulazione

            Pizzas.Add(pizza);
        }

        private static void IngredientToPizza(Pizza pizza, List<int> selectedIngredients)
        {
            pizza.Category = new Category() { Id = 1, Name = "Fake categroy" };

            foreach (int ingredientId in selectedIngredients)
            {
                pizza.Ingredients.Add(new Ingredient() { Id = ingredientId, Name = "Fake Ingredient " + ingredientId });
            }
        }

        public void Delete(Pizza pizza)
        {
            Pizzas.Remove(pizza);
        }

        public Pizza GetById(int id)
        {
            Pizza pizza = Pizzas.Where(pizza => pizza.Id == id).FirstOrDefault();

            pizza.Category = new Category() { Id = 1, Name = "Fake categroy" };

            return pizza;
        }

        public void Update(Pizza pizza, Pizza formData, List<int>? selectedIngredients)
        {
            pizza = formData;
            pizza.Category = new Category() { Id = 1, Name = "Fake categroy" };

            pizza.Ingredients = new List<Ingredient>();

            IngredientToPizza(pizza, selectedIngredients);
        }
    }
}
