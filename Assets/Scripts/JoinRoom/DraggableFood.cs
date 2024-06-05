using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Assets.Scripts.GamePlay;

public class DraggableFood : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image image;
    public int foodNumber;
    [HideInInspector] public Transform parentAfterDrag;
    private Canvas canvas;
    private GameObject placeholder;
    private DropArea originalDropArea;
    private void Start()
    {
        parentAfterDrag = transform.parent;
        canvas = GetComponentInParent<Canvas>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;

        placeholder = Instantiate(gameObject, transform.position, transform.rotation, parentAfterDrag);
        placeholder.GetComponent<Image>().raycastTarget = true;

        originalDropArea = parentAfterDrag.GetComponent<DropArea>();
        if (originalDropArea != null)
        {
            originalDropArea.RemoveDraggable();
        }

        parentAfterDrag = transform.parent;
        transform.SetParent(canvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;

        DropArea[] dropAreas = canvas.GetComponentsInChildren<DropArea>();
        bool droppedInValidArea = false;

        foreach (DropArea dropArea in dropAreas)
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
            Destroy(placeholder);

            if (originalDropArea != null)
            {
                originalDropArea.Drop(this);
                transform.SetParent(originalDropArea.transform);
                transform.localPosition = Vector3.zero;
            }
            else
            {
                transform.SetParent(parentAfterDrag);
                transform.localPosition = Vector3.zero;
            }
        }
    }
}