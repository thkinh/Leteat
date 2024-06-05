using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.GamePlay;


public class DraggableFoodArrange : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public int foodNumber;
    [HideInInspector] public Transform parentAfterDrag;
    private Canvas canvas;
    private GameObject placeholder;
    private DropAreaArrange originalDropArea;
    private void Start()
    {
        parentAfterDrag = transform.parent; // Đảm bảo rằng biến parentAfterDrag được thiết lập
        canvas = GetComponentInParent<Canvas>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;

        // Tạo placeholder tại vị trí hiện tại
        placeholder = Instantiate(gameObject, transform.position, transform.rotation, parentAfterDrag);
        placeholder.GetComponent<Image>().raycastTarget = true;

        originalDropArea = parentAfterDrag.GetComponent<DropAreaArrange>();
        if (originalDropArea != null)
        {
            originalDropArea.RemoveDraggable();
        }

        parentAfterDrag = transform.parent;
        transform.SetParent(canvas.transform, true);// Sử dụng SetParent với worldPositionStays để giữ nguyên vị trí
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;

        // Kiểm tra tất cả các DropArea trong canvas
        DropAreaArrange[] dropAreas = canvas.GetComponentsInChildren<DropAreaArrange>();
        bool droppedInValidArea = false;

        foreach (DropAreaArrange dropArea in dropAreas)
        {
            if (dropArea.Accepts(this))
            {
                dropArea.Drop(this);
                transform.SetParent(dropArea.transform);
                transform.localPosition = Vector3.zero;
                droppedInValidArea = true;
                break;
            }
        }


        if (!droppedInValidArea)
        {
            // Nếu không thả vào vùng hợp lệ, trả về vị trí ban đầu và phá hủy placeholder
            Destroy(placeholder);

            // Nếu không thả vào vùng hợp lệ, trả về vị trí ban đầu và không khôi phục vị trí cũ
            if (originalDropArea != null)
            {
                // Nếu có vùng gốc, giữ nguyên vị trí hiện tại
                originalDropArea.Drop(this);
                transform.SetParent(originalDropArea.transform);
                transform.localPosition = Vector3.zero;
            }
            else
            {
                // Nếu không có vùng gốc, trả về vị trí ban đầu
                transform.SetParent(parentAfterDrag);
                transform.localPosition = Vector3.zero;
            }
        }
    }
}
