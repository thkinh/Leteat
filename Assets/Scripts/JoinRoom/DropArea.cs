using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class DropCondition : ScriptableObject
{
    public abstract bool Check(DraggableFood draggable);
}

public class DropArea : MonoBehaviour
{
    public List<DropCondition> DropConditions = new List<DropCondition>();
    public event Action<DraggableFood> OnDropHandler;

    private DraggableFood currentDraggable;
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
        }
    }

    // Hàm để loại bỏ đối tượng khi cần thiết
    public void RemoveDraggable()
    {
        currentDraggable = null;
    }
}
