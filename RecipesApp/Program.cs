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
            
            
            try
            {
                Setup();
                MenuManager.MainMenu();
            }
            catch(Exception)
            {
                Console.Clear();
                ConsoleShef.ChangeMood("dead");
                ConsoleShef.Say("I'm terribly sorry, but some unknown error ocured!");
            }
            Teardown();
            
        }

        private static void Setup()
        {
            ConsoleShef.Activate();
            if(File.Exists("recipes.json"))
            Recipe.ReadFromFile();
            System.Threading.Thread.Sleep(100);
        }

        private static void Teardown()
        {
            ConsoleShef.Disactivate();
            Recipe.SaveToFile();
        }
    }
}
