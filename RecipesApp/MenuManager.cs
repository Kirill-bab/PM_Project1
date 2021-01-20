using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Library;
using System.Linq;

namespace RecipesApp
{
    public class MenuManager
    {
        private delegate void PlayScript();
        private static Dictionary<string, bool> _isTutorialPassed = new Dictionary<string, bool>
        {
            ["Main"] = false,
            ["EditIngredient"] = false,
            ["EditCalories"] = false,
            ["Find"] = false,
            ["Edit"] = false,
            ["EditIngredientList"] = false,
            ["Add"] = false,
            ["Delete"] = false
        };
        private static int Menu(string header, int menuWidth, PlayScript script, params string[] options)
        {
            Console.Clear();
            int chosen = 0;
            bool scriptisPlayed = false;
            while (true)
            {
                Console.SetCursorPosition(0, 0);
                DrawHeader(header, menuWidth);
                ConsoleShef.ChangeMood("regular");

                foreach (var option in options)
                {
                    if (option == options[chosen])
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.Write(TableBuilder.AlignCentre(option, menuWidth));
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(TableBuilder.AlignCentre(option, menuWidth));
                    }
                    Console.SetCursorPosition(0, Console.CursorTop + 1);
                }
                if (!scriptisPlayed)
                {
                    script();
                    scriptisPlayed = true;
                }

                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.Enter:
                        Console.Clear();
                        return chosen;
                    case ConsoleKey.UpArrow:
                        if (chosen == 0) chosen = options.Length - 1;
                        else chosen--;
                        break;
                    case ConsoleKey.DownArrow:
                        if (chosen == options.Length - 1) chosen = 0;
                        else chosen++;
                        break;
                    case ConsoleKey.Escape: return -1;
                }
            }
        }
        public static void DrawHeader(string header,int width)
        {
            header = header.ToUpper();          
            Console.WriteLine("".PadLeft(width,'='));
            Console.WriteLine("|" + TableBuilder.AlignCentre(header, width - 2) + "|");
            Console.WriteLine("".PadLeft(width, '='));
        }
        public static void MainMenu()
        {
            while (true)
            {
                var chosenOption = Menu("main menu", 37, MainMenuIntro, new string[] { "Search Recipes", "Add Recipe", "Delete Recipe", "Edit Receipe", "Turn Hints Off", "Exit" });
                switch (chosenOption)
                {
                    case 0:
                        ViewRecipe();
                        break;
                    case 1:
                        AddRecipe();
                        break;
                    case 2:
                        DeleteRecipe();
                        break;
                    case 3:
                        EditRecipe();
                        break;
                    case 4:
                        ConsoleShef.Disactivate();
                        break;
                    case 5:
                    case -1:
                        return;
                }
            }
        }

        private static void EditRecipe()
        {
            Console.Clear();
            var chosenRecipe = SearchRecipes("Edit Recipe");
            if (chosenRecipe == null) return;

            Console.Clear();
            var option = Menu("Edit Recipe", 73, EditMenuIntro, "Change Name", "Change duration", "Change Calories Quantity",
                "Change Ingredients List");
            switch (option)
            {
                case -1: return;
               case 0:
                    Console.WriteLine("Enter new Name of your recipe: ");
                    var name = Console.ReadLine().Trim();
                    chosenRecipe.Name = name;
                    break;
                case 1:
                    Console.WriteLine("Enter new Duration of your recipe(in minutes): ");
                    var duration = (int)GetNumb();
                    chosenRecipe.Duration = duration;
                    break;
                case 2:
                    var chose = Menu("Calories", 43, EditCaloriesIntro, "Proteins", "Fats", "Carbonohydrates","Back");
                    switch (chose)
                    {
                        case -1:
                        case 3:
                            break;
                        case 0: Console.WriteLine("Enter new number of Proteins:");
                            chosenRecipe.Proteins = GetNumb();
                            break;
                        case 1:
                            Console.WriteLine("Enter new number of Fats:");
                            chosenRecipe.Fats = GetNumb();
                            break;
                        case 2:
                            Console.WriteLine("Enter new number of Carbonohydrates:");
                            chosenRecipe.Carbonohydrates = GetNumb();
                            break;
                    }
                    break;
                case 3:             
                    var chosen = Menu("Ingredients", 43, Unfinished, "Add Ingredient", "Delete Ingredient", "Edit Ingredient", "Back");
                    switch (chosen)
                    {
                        case -1:
                        case 3:
                            break;
                        case 0:
                            var ingredient = Ingredient.AddIngredient(false);
                            chosenRecipe.Ingredients.Add(ingredient);
                            Console.WriteLine("Enter Quantity :");
                            chosenRecipe.Quantities.Add(GetNumb());
                            break;
                        case 1:
                            var oddIngredient = GetIngredient(chosenRecipe);
                            chosenRecipe.Ingredients.Remove(oddIngredient);
                            chosenRecipe.Quantities.Remove(chosenRecipe.Ingredients.IndexOf(oddIngredient));
                            break;
                        case 2:
                            var chosenIngredient = GetIngredient(chosenRecipe);
                            var op = Menu("Edit Ingredient", 43, Unfinished, "Change Name", "Change Unit", "Change Quantity", "Back");
                            switch (op)
                            {
                                case -1:
                                case 3:
                                    break;
                                case 0:
                                    Console.WriteLine("Enter new Ingredient Name: ");
                                    chosenIngredient.Name = Console.ReadLine().Trim();
                                    break;
                                case 1:
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
                                    chosenIngredient.Unit = Ingredient._units[chosenUnit - 1];
                                    break;
                                case 2:
                                    Console.WriteLine("Enter new Quantity:");
                                    chosenRecipe.Quantities[chosenRecipe.Ingredients.IndexOf(chosenIngredient)] = GetNumb();
                                    break;
                            }
                            break;
                        default:
                            break;
                    }
                    break;
            }
        }

        private static Recipe SearchRecipes(string header = "Search Recipes")
        {
            Console.Clear();
            DrawHeader(header,73);
            if (!_isTutorialPassed["Find"])
            {
                ConsoleShef.ChangeMood("surprized");
                ConsoleShef.Say("this is search menu");
                ConsoleShef.ChangeMood("puzzled");
                ConsoleShef.Say("Firstly, enter filter row by wich i will search recipes");
                ConsoleShef.Say("This can be the name of the whole recipe or it's particular ingredient");
            }
            Console.WriteLine("Please, enter name of recipe or name of ingredient:");
            var filter = Console.ReadLine().Trim().ToLower();
            Console.Write("Searching");
            for (int i = 0; i < 3; i++)
            {
                System.Threading.Thread.Sleep(500);
                Console.Write(".");
            }
            Console.WriteLine();
            var recipesList = Recipe.Recipes.Where(r => r.Name.ToLower().Contains(filter)
            || r.Ingredients.Select(i => i.Name.ToLower()).Contains(filter)).ToList();
            
            if(recipesList.Count == 0)
            {
                Console.WriteLine("-------------------------");
                Console.WriteLine("    No matches found!");
                Console.WriteLine("-------------------------");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
                return null;
            }
            recipesList.OrderBy(e => e.Name).OrderBy(e => e.Proteins + e.Fats + e.Carbonohydrates);
            var recipesArray = new string[recipesList.Count];
            for (int i = 0; i < recipesList.Count; i++)
            {
                recipesArray[i] = recipesList[i].ToString();
            }
            int chosenRecipe = Menu("Search results", 73, SearchMenuIntro, recipesArray);
            if (chosenRecipe == -1) return null;
            var recipe = recipesList[chosenRecipe];
            return recipe;
            
        }

        private static void ViewRecipe()
        {
            Console.Clear();
            Recipe chosenRecipe = SearchRecipes();
            if (chosenRecipe == null) return;
            chosenRecipe.ShowInfo();
            Console.WriteLine("-------------------------");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private static void AddRecipe()
        {
            Console.Clear();
            DrawHeader("add recipe", 73);
            AddMenuIntro();
            Console.WriteLine("Please, enter your recipie.");
            Console.WriteLine("If you don't know some parameters just left fields empty and press ENTER.\n");
            Console.WriteLine("Enter name of your recipe: ");
            var name = Console.ReadLine().Trim();
            Console.WriteLine("Enter time of coocking(in minutes):");
            var duration = (int)GetNumb();
           
            Console.WriteLine("Now, Please, fill in ingredients list.");
            Console.WriteLine("To proceed press ENTER, or press ESC to go back to the Main menu");
            var key = Console.ReadKey().Key;
            while (key != ConsoleKey.Enter && key != ConsoleKey.Escape)
            {
                key = Console.ReadKey().Key;
            }
            if (key == ConsoleKey.Escape) return;

            List<Ingredient> ingredients = new List<Ingredient>(5);
            List<double> quantities = new List<double>(5);

            DrawHeader("Filling ingredients list", 73);
            for (int i = 1;; i++)
            {           
                Console.WriteLine("To stop filling list, enter \'q\' in the Name Field");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine($"Enter ingredient {i}:");
                 var newIngredient = Ingredient.AddIngredient();
                if (newIngredient == null) break;
                Console.WriteLine("Enter Quantity :");
                quantities.Add(GetNumb());
                ingredients.Add(newIngredient);
            }
            Console.WriteLine("===========================================================================");
            Console.WriteLine("Enter Receipt Proteins: ");
            var proteins = GetNumb();
            Console.WriteLine("Enter Receipt Fats: ");
            var fats = GetNumb();
            Console.WriteLine("Enter Receipt Carbonohydrates: ");
            var carbonohydrates = GetNumb();

            new Recipe(name, duration, proteins, fats, carbonohydrates, ingredients, quantities);
            Console.WriteLine("Recipe was successfully added!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadLine();
        }

        private static void DeleteRecipe()
        {
            DeleteMenuIntro();
            var chosenRecipe = SearchRecipes("Delete Recipe");
            if (chosenRecipe == null) return;
            Console.WriteLine("Enter CONFIRM to delete recipe");
            if (Console.ReadLine().ToLower() == "confirm") chosenRecipe.Delete();
        }
        private static double GetNumb()
        {
            double number;
            var str = Console.ReadLine().Trim();
            while((!double.TryParse(str,out number) || number < 0) && !string.IsNullOrEmpty(str))
            {
                Console.WriteLine("Wrong input! Must be a positive double!");
                str = Console.ReadLine().Trim();
            }
            if (string.IsNullOrEmpty(str)) return default(double);
            return number;
        }
        private static Ingredient GetIngredient(Recipe recipe)
        {
            recipe.ShowIngredients();
            Console.WriteLine("Enter number of ingredient to delete(starts from 1):");
            var oddIngredient = (int)GetNumb();
            while (oddIngredient < 1 || oddIngredient > recipe.Ingredients.Count)
            {
                Console.WriteLine("Number is out of ingredient list bounds!");
                Console.WriteLine("Try Again!");
                oddIngredient = (int)GetNumb();
            }
            return recipe.Ingredients[oddIngredient - 1];
        }
        // ConsoleShef Scenarios
        private static void BlankScript()
        {            
        }   
        private static void Unfinished()
        {
            System.Threading.Thread.Sleep(100);
            ConsoleShef.ChangeMood("angry");
            ConsoleShef.Say("Didn't have time for that");
            ConsoleShef.ChangeMood("happy");
            ConsoleShef.Say("But i'm sure you will deal with it!");
            ConsoleShef.ChangeMood("regular");
        }
        private static void MainMenuIntro()
        {
            if (_isTutorialPassed["Main"])
            {
                BlankScript();
                return;
            }

            ConsoleShef.ChangeMood("happy");
            ConsoleShef.Say("Hello!");
            ConsoleShef.ChangeMood("surprized");
            ConsoleShef.Say("this is main menu");
            ConsoleShef.ChangeMood("puzzled");
            ConsoleShef.Say("You can navigate with arrows on your keyboard");
            ConsoleShef.Say("Press Enter to select an option, or press esc to quit menu");
            ConsoleShef.ChangeMood("regular");
            ConsoleShef.Say("This rules are same for all menues");
            ConsoleShef.ChangeMood("doubtful");
            ConsoleShef.Say("if I annoy you way too much you can always turn my hints off.");
            ConsoleShef.ChangeMood("regular");
            _isTutorialPassed["Main"] = true;
            
        }
        private static void SearchMenuIntro()
        {
            if (_isTutorialPassed["Find"])
            {
                BlankScript();
                return;
            }          
            ConsoleShef.ChangeMood("advicing");
            ConsoleShef.Say("Choose any available recipe from occured list and see it full information");
            ConsoleShef.ChangeMood("regular");
            _isTutorialPassed["Find"] = true;
        }
        private static void AddMenuIntro()
        {
            if (_isTutorialPassed["Add"])
            {
                BlankScript();
                return;
            }
            System.Threading.Thread.Sleep(100);
            ConsoleShef.ChangeMood("surprized");
            ConsoleShef.Say("here you can add new recipe");
            ConsoleShef.ChangeMood("puzzled");
            ConsoleShef.Say("Simply follow given below instructions.");
            ConsoleShef.ChangeMood("sad");
            ConsoleShef.Say("it's really hard to mess things up here");
            ConsoleShef.ChangeMood("regular");

            _isTutorialPassed["Add"] = true;

        }
        private static void DeleteMenuIntro()
        {
            if (_isTutorialPassed["Delete"])
            {
                BlankScript();
                return;
            }
            System.Threading.Thread.Sleep(100);
            ConsoleShef.ChangeMood("surprized");
            ConsoleShef.Say("here you can delete recipe");
            ConsoleShef.ChangeMood("puzzled");
            ConsoleShef.Say("Rules are all the same with search menu");
            ConsoleShef.ChangeMood("regular");
            ConsoleShef.Say("After choosing recipe type confirm to approve its deletion");
            ConsoleShef.ChangeMood("happy");
            ConsoleShef.Say("Easy,right?");
            ConsoleShef.ChangeMood("regular");

            _isTutorialPassed["Delete"] = true;
        }
        private static void EditMenuIntro()
        {
            if (_isTutorialPassed["Edit"])
            {
                BlankScript();
                return;
            }
            System.Threading.Thread.Sleep(100);
            ConsoleShef.ChangeMood("surprized");
            ConsoleShef.Say("in this menu you can edit your recipes");
            ConsoleShef.ChangeMood("regular");
            ConsoleShef.Say("to start choose an option");
            _isTutorialPassed["Edit"] = true;
        }
        private static void EditCaloriesIntro()
        {
            if (_isTutorialPassed["EditCalories"])
            {
                BlankScript();
                return;
            }
            System.Threading.Thread.Sleep(100);
            ConsoleShef.ChangeMood("puzzled");
            ConsoleShef.Say("here you can change Recipe calories");
            ConsoleShef.ChangeMood("regular");
            ConsoleShef.Say("to continue choose an option");
            _isTutorialPassed["EditCalories"] = true;
        }
    }
}
