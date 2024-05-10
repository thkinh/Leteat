using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.GamePlay
{
    public class UIManager
    {
        public UIManager() { }

        public static Color LoadColor(Food food)
        {
            Color m_newcolor = Color.white;
            if (food.name == Food.FoodName.Tomato)
            {
                m_newcolor = new Color(1f, 0f, 0f); // Red
            }
            else if (food.name == Food.FoodName.Shrimp)
            {
                m_newcolor = new Color(233f / 255f, 197f / 255f, 113f / 255f, 255f / 255f); 
            }
            else if (food.name == Food.FoodName.Bacon)
            {
                m_newcolor = new Color(200f / 255f, 115f / 255f, 113f / 255f, 255f / 255f); 
            }
            else if (food.name == Food.FoodName.Bread)
            {
                m_newcolor = new Color(255f / 255f, 255f / 255f, 0f, 1); 
            }
            else if (food.name == Food.FoodName.Corn)
            {
                m_newcolor = new Color(133f / 255f, 255f / 255f, 0f, 240f / 255f); 
            }
            return m_newcolor;
        }
    }
}
