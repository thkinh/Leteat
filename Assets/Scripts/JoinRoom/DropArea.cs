using Assets.Scripts.GamePlay;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Assets.Scripts.GamePlay.Food;

public abstract class DropCondition : ScriptableObject
{
    public abstract bool Check(DraggableFood draggable);
}

public class DropArea : MonoBehaviour
{
    public List<DropCondition> DropConditions = new List<DropCondition>();
    public event Action<DraggableFood> OnDropHandler;
    private DraggableFood currentDraggable;
    int codeJoinRoom;

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
            Debug.Log($"Object '{draggable.name}' has been dropped in {name}");
        }
        if (currentDraggable.name == "d1")
        {
            //codeJoinRoom = 
        }
        Debug.Log(message: "Code Join Room: " + codeJoinRoom);
    }

    // Hàm để loại bỏ đối tượng khi cần thiết
    public void RemoveDraggable()
    {
        currentDraggable = null;
    }

}
