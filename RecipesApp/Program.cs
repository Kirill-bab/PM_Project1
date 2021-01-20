using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Library;

namespace RecipesApp
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            Recipe.ReadFromFile();
            MenuManager.MainMenu();
            /*
            ConsoleShef.Activate();
            try
            {
                MenuManager.MainMenu();
                
            }
            catch(Exception)
            {
                Console.Clear();
                ConsoleShef.ChangeMood("dead");
                ConsoleShef.Say("I'm terribly sorry, but some unknown error ocured!");
            }
            ConsoleShef.Disactivate();
            */
        }

        private void Setup()
        {
            ConsoleShef.Activate();
            if(File.Exists("recipes.json"))
            Recipe.ReadFromFile();
        }

        private void Teardown()
        {
            ConsoleShef.Disactivate();
            Recipe.SaveToFile();
        }
    }
}
