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
            foreach (Ingredient ing in i)
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
        private bool weird;

        public Ingredient(double a, string u, string i)
        {
            amount = a;
            unit = u;
            ingredient = i;
            weird = false;
        }
        public Ingredient(string w)
        {
            amount = 0;
            unit = "NULL";
            ingredient = w;
            weird = true;
        }
        public Ingredient()
        {
            amount = 0;
            unit = "NULL";
            ingredient = "";
            weird = false;
        }

        public double getAmount() { return amount; }
        public string getUnit() { return unit; }
        public string getIngredient() { return ingredient; }
        public bool getWeird() { return weird; }

        public void setAmount(double a) { amount = a; }
        public void setUnit(string u) { unit = u; }
        public void setIngredient(string i) { ingredient = i; }
        public void setWeird(bool w) { weird = w; }

        public int setString(string i)
        {
            char[] delim = { ' ' };
            int count = 3;
            string[] parts = i.Split(delim, count, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length != 3)
            {
                // Console.WriteLine("Not enough words in ingredient: {0}", i);
                setWeird(true);
                setIngredient(i);
                return 1;
            }
            try
            {
                amount = Convert.ToDouble(parts[0]);
            }
            catch (FormatException)
            {
                // Console.WriteLine("Could not convert {0} to type double.", parts[0]);
                setWeird(true);
                setIngredient(i);
                return 1;
            }
            unit = parts[1];
            ingredient = parts[2];
            return 0;
        }
        public override string ToString()
        {
            if(getWeird())
                return String.Format("{0}", ingredient);
            else
                return String.Format("{0:F} {1} {2}", amount, unit, ingredient);
        }
    }

    class RecipeLoader
    {
        static void loadRecipeList(List<Recipe> full, string filename)
        {
            try
            {
                using (StreamReader fin = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    StreamWriter fout = new StreamWriter(@"ErrorOutput.txt");
                    char c;
                    string temp = "";
                    string[] tempRecipe = new string[8];
                    bool quote = false;
                    int oops = 0;
                    full.Clear();
                    List<Ingredient> tempIngredient = new List<Ingredient>();
                    List<string> errorIngredients = new List<string>();
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
                            if(tempIngredientObject.setString(i) == 1)
                            {
                                errorIngredients.Add(i);
                                oops++;
                            }
                            tempIngredient.Add(tempIngredientObject);
                        }
                        // oops = # of problems in that recipe
                        if (oops > 0)
                        {
                            fout.WriteLine("ID: {0} Name: {1} has {2} errors. Recipe loaded cautiously.", tempRecipe[0], tempRecipe[1], oops);
                            fout.WriteLine("Ingredients with errors:");
                            foreach(string e in errorIngredients)
                                fout.WriteLine("{0}", e);
                            fout.WriteLine();
                            errorIngredients.Clear();
                            oops = 0;
                        }
                        full.Add(new Recipe(Convert.ToInt32(tempRecipe[0]), tempRecipe[1], tempIngredient, tempRecipe[3], tempRecipe[4], tempRecipe[5], tempRecipe[6], tempRecipe[7]));

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
            foreach (Recipe r in full)
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
            while (c[0] != '0')
            {
                switch (c[0])
                {
                    case '1':
                        Console.WriteLine("Please enter the name you would like to search for");
                        searchWord = Console.ReadLine();
                        foreach (Recipe r in full)
                        {
                            if (r.getName().ToLower().Contains(searchWord.ToLower()))
                                narrow.Add(r);
                        }
                        break;
                    case '2':
                        Console.WriteLine("Please enter each ingredient you would like to search for followed by the enter key");
                        Console.WriteLine("When you are finished entering ingredients, type 69 and hit enter");
                        searchWord = Console.ReadLine();
                        while (!searchWord.Equals("69", StringComparison.CurrentCultureIgnoreCase))
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
                                    if (i.ToString().ToLower().Contains(s.ToLower())) // if the ingredient is in the search list
                                    {
                                        count++;
                                        break;
                                    }
                                }
                            }
                            //for exact match of ingredients
                            //if (count == r.getIngredientList().Count)
                            //    narrow.Add(r);

                            //for all ingredients searched for
                            if (count == searchIngredients.Count)
                                narrow.Add(r);

                            //for at least one match
                            //if (count > 0)
                            //    narrow.Add(r);
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

        static void saveRecipeList(List<Recipe> full, string filename)
        {
            int count = 0;
            string ret = "";
            try
            {
                using (StreamWriter fout = new StreamWriter(new FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    foreach (Recipe r in full)
                    {
                        // check ID
                        ret += r.getID().ToString();
                        ret += ',';

                        // check name
                        ret = partToString(ret, r.getName());
                        ret += ',';

                        // check ingredients
                        ret += '"';
                        foreach (Ingredient i in r.getIngredientList())
                        {
                            count++;
                            foreach (char c in i.ToString())
                            {
                                if (c == '"')
                                    ret += c;
                                ret += c;
                            }
                            if (count != r.getIngredientList().Count)
                                ret += '\n';
                        }
                        ret += "\",";
                        count = 0;

                        // check prep
                        ret = partToString(ret, r.getPrepStyle());
                        ret += ',';

                        // check ice
                        ret = partToString(ret, r.getIceStyle());
                        ret += ',';

                        // check garnish
                        ret = partToString(ret, r.getGarnish());
                        ret += ',';

                        // check glass
                        ret = partToString(ret, r.getGlass());
                        ret += ',';

                        // check instructions
                        ret = partToString(ret, r.getInstructions());

                        ret += '\n';
                        fout.Write(ret);
                        Console.Write(ret);
                        ret = "";
                        Console.WriteLine("Recipe: {0} has been saved.", r.getName());
                    }
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: File does not exist.");
                return;
            }
        }

        static string partToString(string ret, string toAdd)
        {
            bool quoteCheck = false;

            if (toAdd.Contains('"') || toAdd.Contains(','))
            {
                quoteCheck = true;
                ret += '"';
            }
            foreach (char c in toAdd)
            {
                if (c == '"')
                    ret += c;
                ret += c;
            }
            if (quoteCheck)
                ret += '"';
            quoteCheck = false;

            return ret;
        }

        static void Main(string[] args)
        {
            List<Recipe> full = new List<Recipe>();
            Console.WriteLine("Enter the name of the file to read from (Must be a .csv file with no spaces in the title)");
            string inputFile = Console.ReadLine();
            string outputFile;
            string c;
            loadRecipeList(full, inputFile);
            Console.WriteLine("Enter the corresponding number to what you would like to do.");
            Console.WriteLine("(1)Print File");
            Console.WriteLine("(2)Search File");
            Console.WriteLine("(3)Save File");
            Console.WriteLine("(0)Quit");
            c = Console.ReadLine();
            while (c[0] != '0')
            {
                switch (c[0])
                {
                    case '1':
                        printRecipeList(full);
                        break;
                    case '2':
                        searchRecipeList(full);
                        break;
                    case '3':
                        //saveRecipeList()
                        Console.WriteLine("Enter the name of the file you would like to save the file as (Must end in .csv)");
                        outputFile = Console.ReadLine();
                        saveRecipeList(full, outputFile);
                        break;
                    default:
                        Console.WriteLine("{0} is not an option, quitting to previous menu", c);
                        break;
                }

                Console.WriteLine("Enter the corresponding number to what you would like to do.");
                Console.WriteLine("(1)Print File");
                Console.WriteLine("(2)Search File");
                Console.WriteLine("(3)Save File");
                Console.WriteLine("(0)Quit");
                c = Console.ReadLine();
            }
        }
    }
}