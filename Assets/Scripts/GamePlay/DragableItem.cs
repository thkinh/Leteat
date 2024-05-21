using Assets.Scripts;
using Assets.Scripts.GamePlay;
using Unity.VisualScripting;
using UnityEngine;

public class DragableItem : MonoBehaviour
{
    private bool dragging;
    private Vector2 offset, original_pos;
    public Food food;
    private void Awake()
    {
        float randomX = UnityEngine.Random.Range(0.0f, Screen.width);
        float randomY = UnityEngine.Random.Range(0.0f, Screen.height);
        Vector3 randomPosition = Camera.main.ScreenToWorldPoint(new Vector3(randomX, randomY, Camera.main.nearClipPlane));
        randomPosition.z = 0;
        transform.position = randomPosition; dragging = false;
        original_pos = transform.position;
        offset = new Vector2(0,0);
    }

    void Update()
    {
        if (!dragging) return;

        var mousepos = GetMousePos();
        transform.position = mousepos - offset;
    }

    void OnMouseDown()
    {
        dragging = true;
        offset = GetMousePos() - (Vector2)transform.position;

    }

    private void OnMouseUp()
    {
        dragging = false;
        if (!GetComponent<Renderer>().isVisible)
        {
            transform.position = original_pos;
        }

        if (EntityManager.instance.trashcan.GetComponent<BoxCollider2D>().OverlapPoint(transform.position))
        {
            Debug.Log("Destroy this food");
            Destroy(this.GameObject());
        }

        if (EntityManager.instance.SendtoServer.GetComponent<BoxCollider2D>().OverlapPoint(transform.position))
        {
            Destroy(this.gameObject);
            ClientManager.client.SendPacket(this.food, true);
        }
    }



    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
