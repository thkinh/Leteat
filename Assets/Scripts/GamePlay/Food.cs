using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class Food 
    {
        public string foodName;
        public int foodIndex;
        public enum FoodName
        {
            bacon,
            bread,
            cheese,
            corn,
            egg,
            fish,
            shrimp,
            tomato,
            watermelon
        }
        public FoodName fname { get; set; }
        public int FoodIndex { get; private set; }

        public Food(FoodName name)
        {
            this.fname = name;
            foodIndex = (int)name;
        }

        public Food(int name)
        {
            this.fname = (FoodName)name;
            this.foodIndex = name;
        }
        public static string getName(int nameIndex)
        {
            FoodName foodName = (FoodName)nameIndex;

            // Convert the enum value to lowercase string
            return foodName.ToString();
        }
        public Food(string name)
        {
            this.foodName = name;
            this.foodIndex = (int)(FoodName)Enum.Parse(typeof(FoodName), name);

        }  



        public static Food Convert_to_Food(byte[] raw_data)
        {
            int nameLength = Enum.GetNames(typeof(FoodName)).Length;
            string nameString = Encoding.UTF8.GetString(raw_data, 0, nameLength);
            FoodName name = (FoodName)Enum.Parse(typeof(FoodName), nameString);


            // Create and return Food object
            return new Food(name);
        }
    }
}
