﻿using Assets.Scripts.GamePlay;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static Assets.Scripts.GamePlay.Food;
public abstract class DropConditionArrange : ScriptableObject
{
    public abstract bool Check(DraggableFoodArrange draggable);
}
public class DropAreaArrange : MonoBehaviour
{
    public List<DropConditionArrange> DropConditions = new List<DropConditionArrange>();
    public event Action<DraggableFoodArrange> OnDropHandler;
    private DraggableFoodArrange currentDraggable;
    private Position pos;
    private int indexFood = -1;
    public bool IsValidDropPosition = false;
    public List<int> FoodList = new List<int>();

    public int IndexFood
    {
        get => indexFood;
        private set
        {
            indexFood = value;
        }
    }
    public bool Accepts(DraggableFoodArrange draggable)
    {
        if (currentDraggable != null)
        {
            return false;
        }
        return 
            DropConditions.TrueForAll(cond => cond.Check(draggable));
    }

    public void Drop(DraggableFoodArrange draggable)
    {
        // Chỉ nhận đối tượng nếu vùng này chưa có đối tượng nào
        if (currentDraggable == null)
        {
            currentDraggable = draggable;
            OnDropHandler?.Invoke(draggable);
            IndexFood = draggable.foodNumber;
            DropAreaManagerArrange.Instance.UpdateIndexFood(this, indexFood);
            DropAreaManagerArrange.Instance.GetIndexFoods();
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
