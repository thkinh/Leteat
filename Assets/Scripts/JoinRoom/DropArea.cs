using Assets.Scripts.GamePlay;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Assets.Scripts.GamePlay.Food;
using static UnityEditor.PlayerSettings;

public abstract class DropCondition : ScriptableObject
{
    public abstract bool Check(DraggableFood draggable);
}

public class DropArea : MonoBehaviour
{
    public List<DropCondition> DropConditions = new List<DropCondition>();
    public event Action<DraggableFood> OnDropHandler;
    private DraggableFood currentDraggable;
    private int codeJoinRoom = 0;
    private List<int> indexFoods = new List<int>();

    public bool Accepts(DraggableFood draggable)
    {
        if (currentDraggable != null)
        {
            return false;
        }
        return DropConditions.TrueForAll(cond => cond.Check(draggable));
    }

    public void Drop(DraggableFood draggable)
    {
        // Chỉ nhận đối tượng nếu vùng này chưa có đối tượng nào
        if (currentDraggable == null)
        {
            currentDraggable = draggable;
            OnDropHandler?.Invoke(draggable);
            int indexFood = (int)Enum.Parse(typeof(Food.FoodName), draggable.name);
            indexFoods.Add(indexFood);
            Debug.Log($"Object '{draggable.name}' with index '{indexFood}' has been dropped in {name}");
        }
    }

    public void TypeRoom()
    {
        codeJoinRoom = 0;
        for (int i = 0; i < indexFoods.Count; i++)
        {
            codeJoinRoom = codeJoinRoom * 10 + indexFoods[i];
        }

        Debug.Log(message: "Code Join Room is: " + codeJoinRoom);
    }

    // Hàm để loại bỏ đối tượng khi cần thiết
    public void RemoveDraggable()
    {
        currentDraggable = null;
    }


}
