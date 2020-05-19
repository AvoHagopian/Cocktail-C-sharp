using System;
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
        static void Main(string[] args)
        {
            Console.WriteLine("yo");
        }
    }
}