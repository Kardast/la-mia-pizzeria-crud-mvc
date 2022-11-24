namespace la_mia_pizzeria_static.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //relazione molti a molti tra pizze e ingredienti
        public List<Pizza> Pizzas { get; set; }
    }
}
