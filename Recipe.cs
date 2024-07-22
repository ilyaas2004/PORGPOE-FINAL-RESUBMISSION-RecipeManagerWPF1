using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeManagerWPF1
{
    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; } = new List<Ingredient>();
        public List<string> Steps { get; set; } = new List<string>();
        private decimal factor;

        public string DisplayRecipe()
        {
            StringBuilder recipeDetails = new StringBuilder();
            recipeDetails.AppendLine($"\nRecipe: {Name}");
            recipeDetails.AppendLine("Ingredients:");
            foreach (var ingredient in Ingredients)
            {
                recipeDetails.AppendLine($"{ingredient.Quantity} {ingredient.Unit} of {ingredient.Name}, {ingredient.Calories} calories, Food Group: {ingredient.FoodGroup}");
            }

            recipeDetails.AppendLine("\nSteps:");
            for (int i = 0; i < Steps.Count; i++)
            {
                recipeDetails.AppendLine($"{i + 1}. {Steps[i]}");
            }
            recipeDetails.AppendLine($"Total Calories: {CalculateTotalCalories()}");

            return recipeDetails.ToString();
        }

        public void ScaleRecipe(decimal scaleFactor)
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.Quantity *= scaleFactor;
            }
        }

        public void ResetQuantities()
        {
            foreach (var ingredient in Ingredients)
            {
                ingredient.ResetQuantity();
            }
        }

        public int CalculateTotalCalories()
        {
            return Ingredients.Sum(ingredient => ingredient.Calories);
        }
    }
}

