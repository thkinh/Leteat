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
    private int indexFood = -1;

    public int IndexFood
    {
        get => indexFood;
        private set
        {
            indexFood = value;
        }
    }
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
            IndexFood = draggable.foodNumber;
            DropAreaManager.Instance.UpdateIndexFood(this, indexFood);
            DropAreaManager.Instance.GetIndexFoods();
            DropAreaManager.Instance.CodeJoinRoom();
        }
    }


    // Hàm để loại bỏ đối tượng khi cần thiết
    public void RemoveDraggable()
    {
        if (currentDraggable != null)
        {
            IndexFood = -1;
            currentDraggable = null;
        }
    }
}
