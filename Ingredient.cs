using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManagerWPF1
{
    public class Ingredient
    {
        public string Name { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public int Calories { get; set; }
        public string FoodGroup { get; set; }
        private decimal originalQuantity;

        public Ingredient(string name, decimal quantity, string unit, int calories, string foodGroup)
        {
            Name = name;
            Quantity = quantity;
            originalQuantity = quantity; // Store the original quantity for resetting later
            Unit = unit;
            Calories = calories;
            FoodGroup = foodGroup;
        }

        public void ResetQuantity()
        {
            Quantity = originalQuantity;
        }
    }
}

