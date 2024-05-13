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
        public enum FoodName
        {
            Bacon,
            Bread,
            Cheese,
            Corn,
            Egg,
            Fish,
            Shrimp,
            Tomato,
            Watermelon
        }
        public FoodName name { get; set; }

        public Food(FoodName name)
        {
            this.name = name;
        }

        public Food(int nameIndex)
        {
            this.name = (FoodName)nameIndex;
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


        [SerializeField] GameObject[] FoodPrefab;
        [SerializeField] float secondSpawn = 0.5f;
        [SerializeField] float minTrans;
        [SerializeField] float maxTrans;

        void Start()
        {
            StartCoroutine(FoodSpawn());    
        }

        IEnumerator FoodSpawn()
        {
            while(true)
            {
                var wanted = Random.Range(minTrans, maxTrans);
                var position = new Vector3(wanted, transform.position.y);
                GameObject gameObject = Instantiate(FoodPrefab[Random.Range(0, FoodPrefab)], position, Quaternion.identity);
                yield return new WaitForSeconds(secondSpawn);
                Destroy(gameObject, 5f);
            }
        }
    }
}
