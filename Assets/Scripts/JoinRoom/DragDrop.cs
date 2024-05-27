using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject dragableObjectPrefab; // Prefab of the draggable object
    public Transform dragableParent; // Parent object that holds all draggable objects
    public Transform dropableParent; // Parent object that holds all dropable slots

    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Vector2 originalPosition;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // Check if dropped on a valid dropable slot
        if (!IsPointerOverUIObject("DropableSlot"))
        {
            // Return to original position if not dropped in a valid slot
            rectTransform.anchoredPosition = originalPosition;
        }
        else
        {
            // Duplicate the draggable object and keep it in the original parent
            GameObject clone = Instantiate(dragableObjectPrefab, dragableParent);
            clone.GetComponent<RectTransform>().anchoredPosition = originalPosition;
        }
    }

    private bool IsPointerOverUIObject(string tag)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.CompareTag(tag))
            {
                return true;
            }
        }

        return false;
    }

}
