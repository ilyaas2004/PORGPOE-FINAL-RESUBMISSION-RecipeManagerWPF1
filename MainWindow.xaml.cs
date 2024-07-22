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
            try
            {
                // Collect data from the input fields
                string recipeName = txtRecipeName.Text;
                string ingredientName = txtIngredientName.Text;
                if (!decimal.TryParse(txtQuantity.Text, out decimal quantity))
                {
                    MessageBox.Show("Please enter a valid quantity.");
                    return;
                }
                string unit = cmbUnit.Text;
                if (!int.TryParse(txtCalories.Text, out int calories))
                {
                    MessageBox.Show("Please enter a valid calorie amount.");
                    return;
                }
                string foodGroup = cmbFoodGroup.Text;

                // Add the recipe
                _recipeManager.AddRecipe(recipeName, ingredientName, quantity, unit, calories, foodGroup);

                // Check if the total calories of the new recipe exceed 300
                var addedRecipe = _recipeManager.GetRecipes().FirstOrDefault(r => r.Name == recipeName);
                if (addedRecipe != null && addedRecipe.CalculateTotalCalories() > 300)
                {
                    MessageBox.Show("The total calories of this recipe exceed 300!");
                }

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
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
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

        private void lstRecipes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lstRecipes.SelectedItem != null)
            {
                var selectedRecipe = lstRecipes.SelectedItem as Recipe;
                if (selectedRecipe != null)
                {
                    txtSelectedRecipe.Text = selectedRecipe.DisplayRecipe();
                }
            }
        }

        private void RefreshRecipeList()
        {
            lstRecipes.ItemsSource = null;
            lstRecipes.ItemsSource = _recipeManager.GetRecipes();
        }

        private void ScaleRecipe_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (lstRecipes.SelectedItem != null)
                {
                    var selectedRecipe = lstRecipes.SelectedItem as Recipe;
                    if (selectedRecipe != null)
                    {
                        if (decimal.TryParse(txtScaleFactor.Text, out decimal scaleFactor) && scaleFactor > 0)
                        {
                            selectedRecipe.ScaleRecipe(scaleFactor);
                            txtSelectedRecipe.Text = selectedRecipe.DisplayRecipe();
                            RefreshRecipeList(); // Optionally refresh the list to reflect changes
                        }
                        else
                        {
                            MessageBox.Show("Please enter a valid scaling factor greater than 0.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please select a recipe to scale.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}