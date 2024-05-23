using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class ClickObjects : MonoBehaviour
{
    public GameObject[] G_Object;
    public Sprite[] S_Object;
    public Button[] buttons; // Mảng chứa các Button UI
    public static bool m_created = false;
    public List<int> TakeList = new List<int>();
    private int randomNumber;
    private int currentIndex = 0; // Biến để theo dõi đối tượng hiện tại
    public Position position;
    private void Start()
    {
        TakeList = new List<int>(new int[G_Object.Length]);
            // Gán sự kiện OnClick cho các button
            for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Lưu trữ giá trị của i để sử dụng trong lambda expression
            buttons[i].onClick.AddListener(() => OnButtonClicked(index));
        }
    }

    // Phương thức được gọi khi button được nhấn
    public void OnButtonClicked(int index)
    {
        if (index >= 0 && index < G_Object.Length)
        {
            currentIndex = index;
            UpdateTakeListUI();
        }
        else
        {
            Debug.LogError("Index out of range: " + index);
        }
    }

    // Phương thức để cập nhật UI hiển thị TakeList
    private void UpdateTakeListUI()
    {
        G_Object[currentIndex].GetComponent<SpriteRenderer>().sprite = S_Object[TakeList[currentIndex]];
        Debug.Log("Displayed " + S_Object[TakeList[currentIndex]].name + " at index " + currentIndex);
    }
}
