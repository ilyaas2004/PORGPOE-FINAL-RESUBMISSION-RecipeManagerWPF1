using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecipeManagerWPF1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RecipeManager _recipeManager;

        public MainWindow()
        {
            InitializeComponent();
            _recipeManager = new RecipeManager();
            RefreshRecipeList();
        }

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            // Collect data from the input fields
            string recipeName = txtRecipeName.Text;
            string ingredientName = txtIngredientName.Text;
            decimal quantity;
            decimal.TryParse(txtQuantity.Text, out quantity);
            string unit = cmbUnit.Text;
            int calories;
            int.TryParse(txtCalories.Text, out calories);
            string foodGroup = cmbFoodGroup.Text;

            // Add the recipe
            _recipeManager.AddRecipe(recipeName, ingredientName, quantity, unit, calories, foodGroup);

            // Clear input fields after adding the recipe
            txtRecipeName.Text = string.Empty;
            txtIngredientName.Text = string.Empty;
            txtQuantity.Text = string.Empty;
            cmbUnit.SelectedIndex = -1;
            txtCalories.Text = string.Empty;
            cmbFoodGroup.SelectedIndex = -1;

            // Refresh the recipe list
            RefreshRecipeList();
        }

        private void ClearAllData_Click(object sender, RoutedEventArgs e)
        {
            _recipeManager.ClearAllData();
            RefreshRecipeList();
        }

        private void FilterRecipes_Click(object sender, RoutedEventArgs e)
        {
            string ingredientName = txtFilterIngredient.Text;
            string foodGroup = cmbFilterFoodGroup.Text;
            int maxCalories;
            int.TryParse(txtFilterCalories.Text, out maxCalories);

            var filteredRecipes = _recipeManager.GetRecipes().Where(r =>
                (string.IsNullOrEmpty(ingredientName) || r.Ingredients.Any(i => i.Name.ToLower().Contains(ingredientName.ToLower()))) &&
                (string.IsNullOrEmpty(foodGroup) || r.Ingredients.Any(i => i.FoodGroup == foodGroup)) &&
                (maxCalories == 0 || r.CalculateTotalCalories() <= maxCalories)).ToList();

            lstRecipes.ItemsSource = filteredRecipes;
        }

        private void lstRecipes_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (lstRecipes.SelectedItem != null)
            {
                var selectedRecipe = lstRecipes.SelectedItem as Recipe;
                txtSelectedRecipe.Text = selectedRecipe?.DisplayRecipe();
            }
        }

        private void RefreshRecipeList()
        {
            lstRecipes.ItemsSource = null;
            lstRecipes.ItemsSource = _recipeManager.GetRecipes();
        }
    }
}

