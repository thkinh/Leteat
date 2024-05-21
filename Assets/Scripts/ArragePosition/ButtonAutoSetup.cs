using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ButtonAutoSetup : MonoBehaviour
{
    public ClickObjects buttonHandler; // Tham chiếu đến script ButtonHandler
    public Button[] buttons; // Mảng chứa tất cả các button trong scene

    void Start()
    {
        // Kiểm tra xem buttonHandler có được gán hay chưa
        if (buttonHandler == null)
        {
            Debug.LogError("ButtonHandler is not assigned.");
            return;
        }

        // Gán sự kiện OnClick cho mỗi button
        for (int i = 0; i < buttons.Length; i++)
        {
            int index = i; // Lưu trữ giá trị của i để sử dụng trong lambda expression
            buttons[i].onClick.AddListener(() => buttonHandler.OnButtonClicked(index));
        }
    }
}
