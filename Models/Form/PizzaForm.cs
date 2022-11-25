using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Hosting;

namespace la_mia_pizzeria_static.Models.Form
{
    public class PizzaForm
    {
        //model del db che con le views: create, read (list, details), update
        public Pizza Pizza { get; set; }
        //views: create, update, 
        public List<Category>? Categories { get; set; }
        //views: create, update
        public List<SelectListItem>? Ingredients { get; set; }
        //model: create, update
        public List<int>? SelectedIngredients { get; set; }
    }
}
