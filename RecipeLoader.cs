using System;
using System.IO;
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
            ingredientList = il;
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
            ingredientList.Clear();
            prepStyle = "";
            iceStyle = "";
            garnish = "";
            glass = "";
            instructions = "";
        }

        public static bool operator < (Recipe lhs, Recipe rhs)
        {
            if(String.Compare(lhs.getName(), rhs.getName()) < 0)
                return true;
            else
                return false;
        }
        public static bool operator > (Recipe lhs, Recipe rhs)
        {
            if(String.Compare(lhs.getName(), rhs.getName()) > 0)
                return true;
            else
                return false;
        }

        public int getID() {    return ID;  }
        public string getName() {    return name;  }
        public List<Ingredient> getIngredientList() {return ingredientList;  }
        public string getPrepStyle() {    return prepStyle;    }
        public string getIceStyle() {    return iceStyle;  }
        public string getGarnish() {    return garnish;    }
        public string getGlass() {    return glass;    }
        public string getInstructions() {    return instructions;  }

        public void setName(string n) { name = n;   }
        public void setIngredientList(List<Ingredient> i) {    ingredientList = i; }
        public void setPrepStyle(string p) {    prepStyle = p;  }
        public void setIceStyle(string i) { iceStyle = i;   }
        public void setGarnish(string g) {  garnish = g;    }
        public void setGlass(string g) {    glass = g;  }
        public void setInstructions(string i) { instructions = i;   }

        
        public override string ToString()
        {
            string ret = "";
            ret += String.Format("{0,-20}{1}\n", "ID:", ID);
            ret += String.Format("{0,-20}{1}\n", "Name:", name);
            ret += String.Format("{0,-20}{1}\n", "Ingredient List:", ingredientList[0]);
            for(int i = 1; i < ingredientList.Count; i++)
            {
                ret += String.Format("{0,-20}{1}\n", "", ingredientList[0]);
            }
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

        public double getAmount(){ return amount;  }
        public string getUnit(){   return unit;    }
        public string getIngredient(){ return ingredient;  }

        public void setAmount(double a){   amount = a; }
        public void setUnit(string u){ unit = u;   }
        public void setIngredient(string i){   ingredient = i; }

        public int setString(string i)
        {
            char[] delim = {' '};
            int count = 3;
            string[] parts = null;
            parts = i.Split(delim, count);
            try
            {
                amount = Convert.ToDouble(parts[0]);
            }
            catch (FormatException)
            {
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
        void loadRecipeList(List<Recipe> full, string filename)
        {
            try
            {
                using(StreamReader fin = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
                {
                    char c;
                    string temp = "";
                    string[] tempRecipe = new string[7];
                    int quote = 0;
                    int oops = 0;
                    int comma = 0;
                    full.Clear();
                    List<Ingredient> tempIngredient = new List<Ingredient>();
                    Ingredient tempIngredientObject;

                    while(fin.Peek() >= 0)
                    {
                        while(comma < 8)
                        {
                            c = (char)fin.Read();
                            switch(c)
                            {
                                case '"':
                                    if(quote == 0)
                                        quote++;
                                    else
                                        quote--;
                                    break;
                                case ',':
                                    if(quote == 0)
                                    {
                                        if(comma == 2)
                                        {
                                            tempIngredientObject = new Ingredient();
                                            oops += tempIngredientObject.setString(temp);
                                            tempIngredient.Add(tempIngredientObject);
                                        }
                                        else
                                            tempRecipe[comma] = temp;
                                        temp = "";
                                        comma++;
                                    }
                                    break;
                                case '\n':
                                    if(quote != 0)
                                    {
                                        tempIngredientObject = new Ingredient();
                                        oops += tempIngredientObject.setString(temp);
                                        tempIngredient.Add(tempIngredientObject);
                                        temp = "";
                                    }
                                    else
                                    {
                                        tempRecipe[2] = temp;
                                        comma = 8;
                                    }
                                    break;
                                default:
                                    temp += c;
                                    break;
                            }
                        }
                        if(oops != 0)
                        {
                            //oops = # of problems in that recipe
                            oops = 0;
                        }
                        else
                            full.Add(new Recipe(Convert.ToInt32(temp[0]), tempRecipe[1], tempIngredient, tempRecipe[3], tempRecipe[4], tempRecipe[5], tempRecipe[6], tempRecipe[2]));

                        comma = 0;
                        quote = 0;
                        temp = "";
                        tempIngredient.Clear();
                    }
                }
            }
            catch(FileNotFoundException)
            {
                Console.WriteLine("Error: File does not exist.");
                return;
            }
        }

        static void Main(string[] args)
        {

        }
    }
}