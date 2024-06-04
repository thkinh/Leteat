using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class Food : MonoBehaviour
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
        public FoodName name { get; set; }

        public Food(FoodName name)
        {
            this.name = name;
        }

        public Food(int name)
        {
            this.name = (FoodName)name;
        }

        public Food(string name)
        {
            this.foodName = name;
            this.foodIndex = (int)(FoodName)Enum.Parse(typeof(FoodName), name);

        }  

        public byte[] Convert_to_Data()
        {
            return Encoding.UTF8.GetBytes(this.name.ToString());
        }

        public static byte[] Convert_to_Data(FoodName _name)
        {
            return Encoding.UTF8.GetBytes(_name.ToString());
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
