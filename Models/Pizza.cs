using la_mia_pizzeria_static.Validator;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace la_mia_pizzeria_static.Models
{
    public class Pizza
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(100, ErrorMessage = "Il titolo non può essere più lungo di 100 caratteri")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        [StringLength(100, ErrorMessage = "La descrizione non può essere più lunga di 500 caratteri")]
        [Column(TypeName = "text")]
        [FiveWordsAttribute]
        public string Description { get; set; }

        [Required(ErrorMessage = "Il campo è obbligatorio")]
        public string Image { get; set; }

        //[Required(ErrorMessage = "Il campo è obbligatorio")]
        //[ValidateNever]
        //[IsNumberAttribute]
        //[DataType(DataType.Currency, ErrorMessage = "Il campo deve essere numerico")]
        //[Range(1, 30, ErrorMessage = "Il campo deve essere numerico")]
        public float Cost { get; set; }
        public bool Available { get; set; }

        //relazione 1 a n con Category ed è la vera e propria chiave esterna
        public int CategoryId { get; set; }
        //relazione 1 a n con Category l'istanza del record nelle query
        public Category? Category { get; set; }

        //relazione molti a molti tra pizze e ingredienti
        public List<Ingredient>? Ingredients { get; set; }

        public Pizza()
        {

        }

        public Pizza(string name, string description, string image, float cost, bool available)
        {
            this.Name = name;
            this.Description = description;
            this.Image = image;
            this.Cost = cost;
            this.Available = available;
        }
    }
}
