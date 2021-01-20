using Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RecipesApp
{
    public class Recipe 
    {
        public string Name { get; set; }
        public int Duration { get; set; }
        public double Proteins { get; set; }
        public double Fats { get; set; }
        public double Carbonohydrates { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<double> Quantities { get; set; }
        [NonSerialized]
        public static List<Recipe> Recipes = new List<Recipe>(0);

        public Recipe() { }
        public Recipe(string name, int duration, double proteins, double fats, double carbonohydrates, List<Ingredient> ingredients, List<double> quantities)
        {
            if (ingredients.Count != quantities.Count) throw new ArgumentException();
            Name = name;
            Duration = duration;
            Proteins = proteins;
            Fats = fats;
            Carbonohydrates = carbonohydrates;
            Ingredients = new List<Ingredient>(ingredients.Count);
            Ingredients.AddRange(ingredients);

            Quantities = new List<double>(quantities.Count);
            Quantities.AddRange(quantities);

            Recipes.Add(this);
            SaveToFile();
        }
        public void Delete()
        {
            Recipes.Remove(this);
            SaveToFile();
        }
        public void ShowInfo()
        {
            var header = "\"" + Name + "\"";
            MenuManager.DrawHeader(header, 73);
            ShowIngredients();
            if (Duration != 0) Console.WriteLine($"Duration: {Duration} minutes");
            
        }
        public void ShowIngredients()
        {
            var calories = new string[1][];
            calories[0] = new string[] { Proteins.ToString(), Fats.ToString(), Carbonohydrates.ToString() };
            TableBuilder.DrawTable(new string[] { "Proteins", "Fats", "Carbonohydrates" }, calories);

            string[] columnsHeaders = new string[] { "Ingredient", "Unit", "Quantity" };
            string[][] input = new string[Ingredients.Count][];
            for (int i = 0; i < input.Length; i++)
            {
                var temp = Ingredients[i].ToDataArray();
                temp[2] = Quantities[i].ToString();
                input[i] = temp;
            }
            TableBuilder.DrawTable(columnsHeaders, input);
        }

        public static void SaveToFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = JsonSerializer.Serialize(Recipes, options);
            FileWorker.Write("recipes.json", json);
        }
        public static void ReadFromFile()
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var json = FileWorker.Read("recipes.json");
            Recipes = JsonSerializer.Deserialize<List<Recipe>>(json, options);
        }
        public override string ToString()
        {
            return $" {Name}{Duration,10}{Proteins,10},{Fats,5},{Carbonohydrates,5}";
        }
    }
}
