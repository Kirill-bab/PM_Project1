using Library;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RecipesApp
{
    public class Ingredient
    {          
        public string Name { get; set; }        
        public string Unit { get; set; }

        [NonSerialized]
        public readonly static string[] _units = new string[]
        {
            "table spoon",
            "tea spoon",
            "handful",
            "liter",
            "milliliter",
            "gram",
            "kilo",
            "piece",
            "not mentioned"
        };

        public Ingredient() { }
        public Ingredient(string name, string unit)
        {
            Name = name;
            Unit = unit;
        }
        public string[] ToDataArray()
        {
            return new string[] { Name, Unit, ""};
        }

        public static Ingredient AddIngredient(bool isInLoop = true)
        {
           
            Console.WriteLine("Enter Name: ");
            var iName = Console.ReadLine().Trim();
            if (iName == "q" && isInLoop) return null;
            string unit;

            Console.WriteLine("Please, choose ingredient unit(enter digit): ");
            MenuManager.DrawHeader("Available units", 37);
            for (int j = 0; j < Ingredient._units.Length; j++)
            {
                Console.WriteLine($" {j + 1}) {Ingredient._units[j].PadLeft(20)}");
            }
            var answer = Console.ReadLine();
            int chosenUnit;
            while (!int.TryParse(answer, out chosenUnit) || (chosenUnit < 1 || chosenUnit > Ingredient._units.Length))
            {
                Console.WriteLine("wrong option! Try again");
                answer = Console.ReadLine();
            }
            unit = Ingredient._units[chosenUnit - 1];

            return new Ingredient(iName, unit);
        }
    }
}
