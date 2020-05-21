using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace RecipeApplication
{
    class Recipe
    {
        private int ID;
        private string name;
        private List<Ingredient> ingredientList;
        private string prepStyle;
        private string iceStyle;
        private string garnish;
        private string glass;
        private string instructions;

        public Recipe(int id, string na, List<Ingredient> il, string pr, string ic, string ga, string gl, string i)
        {
            ID = id;
            name = na;
            ingredientList = new List<Ingredient>();
            setIngredientList(il);
            prepStyle = pr;
            iceStyle = ic;
            garnish = ga;
            glass = gl;
            instructions = i;
        }
        public Recipe()
        {
            ID = 0;
            name = "";
            ingredientList = new List<Ingredient>();
            ingredientList.Clear();
            prepStyle = "";
            iceStyle = "";
            garnish = "";
            glass = "";
            instructions = "";
        }

        public static bool operator <(Recipe lhs, Recipe rhs)
        {
            if (String.Compare(lhs.getName(), rhs.getName()) < 0)
                return true;
            else
                return false;
        }
        public static bool operator >(Recipe lhs, Recipe rhs)
        {
            if (String.Compare(lhs.getName(), rhs.getName()) > 0)
                return true;
            else
                return false;
        }

        public int getID() { return ID; }
        public string getName() { return name; }
        public List<Ingredient> getIngredientList() { return ingredientList; }
        public string getPrepStyle() { return prepStyle; }
        public string getIceStyle() { return iceStyle; }
        public string getGarnish() { return garnish; }
        public string getGlass() { return glass; }
        public string getInstructions() { return instructions; }

        public void setName(string n) { name = n; }
        public void setIngredientList(List<Ingredient> i)
        {
            ingredientList.Clear();
            foreach(Ingredient ing in i)
                ingredientList.Add(ing);
        }
        public void setPrepStyle(string p) { prepStyle = p; }
        public void setIceStyle(string i) { iceStyle = i; }
        public void setGarnish(string g) { garnish = g; }
        public void setGlass(string g) { glass = g; }
        public void setInstructions(string i) { instructions = i; }


        public override string ToString()
        {
            string ret = "";
            ret += String.Format("{0,-20}{1}\n", "ID:", ID);
            ret += String.Format("{0,-20}{1}\n", "Name:", name);
            ret += String.Format("{0,-20}{1}\n", "Ingredient List:", ingredientList[0].ToString());
            foreach (Ingredient i in ingredientList.Skip(1))
                ret += String.Format("{0,-20}{1}\n", "", i.ToString());
            ret += String.Format("{0,-20}{1}\n", "Preperation Style:", prepStyle);
            ret += String.Format("{0,-20}{1}\n", "Ice Style:", iceStyle);
            ret += String.Format("{0,-20}{1}\n", "Garnish:", garnish);
            ret += String.Format("{0,-20}{1}\n", "Glass:", glass);
            ret += String.Format("{0,-20}{1}\n", "Instructions:", instructions);

            return ret;
        }
    }

    class Ingredient
    {
        private double amount;
        private string unit;
        private string ingredient;

        public Ingredient(double a, string u, string i)
        {
            amount = a;
            unit = u;
            ingredient = i;
        }
        public Ingredient()
        {
            amount = 0;
            unit = "NULL";
            ingredient = "";
        }

        public double getAmount() { return amount; }
        public string getUnit() { return unit; }
        public string getIngredient() { return ingredient; }

        public void setAmount(double a) { amount = a; }
        public void setUnit(string u) { unit = u; }
        public void setIngredient(string i) { ingredient = i; }

        public int setString(string i)
        {
            char[] delim = { ' ' };
            int count = 3;
            string[] parts = i.Split(delim, count, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                // Console.WriteLine("Not enough words in ingredient: {0}", i);
                return 1;
            }
            try
            {
                amount = Convert.ToDouble(parts[0]);
            }
            catch (FormatException)
            {
                // Console.WriteLine("Could not convert {0} to type double.", parts[0]);
                return 1;
            }
            unit = parts[1];
            ingredient = parts[2];
            return 0;
        }

        public override string ToString()
        {
            return String.Format("{0:F} {1} {2}", amount, unit, ingredient);
        }
    }

    class RecipeLoader
    {
        // loads all recipes from file filename into vector full
        static void loadRecipeList(List<Recipe> full, string filename)
        {
            try
            {
                using (StreamReader fin = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    char c;
                    string temp = "";
                    string[] tempRecipe = new string[8];
                    bool quote = false;
                    int oops = 0;
                    full.Clear();
                    List<Ingredient> tempIngredient = new List<Ingredient>();
                    Ingredient tempIngredientObject;

                    while (fin.Peek() >= 0)
                    {
                        for (int i = 0; i < 8; i++)
                        {
                            c = (char)fin.Read();

                            if (c == '"')
                            {
                                quote = true;
                                while (quote)
                                {
                                    c = (char)fin.Read();
                                    if (c == '"')
                                    {
                                        if ((char)fin.Peek() == ',' || (char)fin.Peek() == '\n')
                                            quote = false;
                                        else
                                        {
                                            c = (char)fin.Read();
                                            temp += c;
                                        }
                                    }
                                    else
                                        temp += c;
                                }
                                c = (char)fin.Read();
                            }
                            else
                            {
                                if (c != '\n')
                                {
                                    while (c != ',' && c != '\n')
                                    {
                                        temp += c;
                                        c = (char)fin.Read();
                                    }
                                }
                            }
                            tempRecipe[i] = temp;
                            temp = "";
                        }

                        char[] sep = { '\n' };

                        string[] ingredients = tempRecipe[2].Split(sep, StringSplitOptions.RemoveEmptyEntries);
                        tempIngredientObject = new Ingredient();

                        foreach (string i in ingredients)
                        {
                            tempIngredientObject = new Ingredient();
                            oops += tempIngredientObject.setString(i);
                            tempIngredient.Add(tempIngredientObject);
                        }
                        if (oops != 0)
                        {
                            //oops = # of problems in that recipe
                            // Console.WriteLine("ID: {0} Name: {1} has {2} errors. Recipe not loaded.\n", tempRecipe[0], tempRecipe[1], oops);
                            oops = 0;
                        }
                        else
                            full.Add(new Recipe(Convert.ToInt32(tempRecipe[0]), tempRecipe[1], tempIngredient, tempRecipe[3], tempRecipe[4], tempRecipe[5], tempRecipe[6], tempRecipe[7]));

                        List<Ingredient> six = full[0].getIngredientList();
                        tempIngredient.Clear();
                        Array.Clear(tempRecipe, 0, tempRecipe.Length);
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: File does not exist.");
                return;
            }
        }

        static void printRecipeList(List<Recipe> full)
        {
            foreach(Recipe r in full)
                Console.WriteLine(r.ToString());
        }

        static void searchRecipeList(List<Recipe> full)
        {
            string c;
            int count = 0;
            string searchWord = "";
            List<string> searchIngredients = new List<string>();
            List<Recipe> narrow = new List<Recipe>();

            Console.WriteLine("What would you like to search the recipe list by?");
            Console.WriteLine("(1) Name");
            Console.WriteLine("(2) Ingredients");
            Console.WriteLine("(0) Quit");

            c = Console.ReadLine();
            while(c[0] != '0')
            {
                switch (c[0])
                {
                    case '1':
                        Console.WriteLine("Please enter the name you would like to search for");
                        searchWord = Console.ReadLine();
                        foreach (Recipe r in full)
                        {
                            if(r.getName().ToLower().Contains(searchWord.ToLower()))
                                narrow.Add(r);
                        }
                        break;
                    case '2':
                        Console.WriteLine("Please enter each ingredient you would like to search for followed by the enter key");
                        Console.WriteLine("When you are finished entering ingredients, type 69 and hit enter");
                        searchWord = Console.ReadLine();
                        while(!searchWord.Equals("69", StringComparison.CurrentCultureIgnoreCase))
                        {
                            searchIngredients.Add(searchWord);
                            searchWord = Console.ReadLine();
                        }
                        foreach (Recipe r in full)
                        {
                            foreach (Ingredient i in r.getIngredientList())
                            {
                                foreach (string s in searchIngredients)
                                {
                                    if(i.ToString().ToLower().Contains(s.ToLower())) // if the ingredient is in the search list
                                    {
                                        count++;
                                        break;
                                    }
                                }
                            }
                            if(count == r.getIngredientList().Count)
                                narrow.Add(r);
                            count = 0;
                        }
                        break;
                    default:
                        Console.WriteLine("{0} is not an option, quitting to previous menu", c);
                        break;
                }
                printRecipeList(narrow);
                narrow.Clear();
                
                Console.WriteLine("What would you like to search the recipe list by?");
                Console.WriteLine("(1) Name");
                Console.WriteLine("(2) Ingredients");
                Console.WriteLine("(0) Quit");
                c = Console.ReadLine();
            }
        }
        static void Main(string[] args)
        {
            List<Recipe> full = new List<Recipe>();
            loadRecipeList(full, "Cocktails V2.csv");
            searchRecipeList(full);
        }
    }
}