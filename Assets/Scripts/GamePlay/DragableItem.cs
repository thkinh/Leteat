using Assets.Scripts;
using Assets.Scripts.GamePlay;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragableItem : MonoBehaviour
{
    private bool dragging;
    private Vector2 offset, original_pos;
    public int idFood;
    public Vector2 velo;
    public bool spawned_by_server = false;

    public DragableItem(Vector2 v)
    {
        velo = v;
        spawned_by_server=true;
    }

    private void Awake()
    {
        float randomX = UnityEngine.Random.Range(0.0f, Screen.width);
        float randomY = UnityEngine.Random.Range(0.0f, Screen.height);
        Vector3 randomPosition = Camera.main.ScreenToWorldPoint(new Vector3(randomX, randomY, Camera.main.nearClipPlane));
        randomPosition.z = 0;
        transform.position = randomPosition; 
        dragging = false;
        original_pos = transform.position;
        offset = new Vector2(0,0);
    }

    void Update()
    {
        if (!dragging) return;
        if (Vector2.Distance(this.transform.position, new Vector2(917,495)) < 5 && spawned_by_server)
        {
            this.transform.position += (Vector3)velo;
        }
        var mousepos = GetMousePos();
        this.transform.position = mousepos;
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
            Debug.Log("Destroy food " + idFood);
            Destroy(gameObject);
        }
        else if (EntityManager.instance.sendprevious.GetComponent<BoxCollider2D>().OverlapPoint(transform.position))
        {
            Debug.Log("Send to next food " + idFood);
            Destroy(gameObject);
            ClientManager.client.SendPacket(new Food(idFood), false);
        }
        else if (EntityManager.instance.sendnext.GetComponent<BoxCollider2D>().OverlapPoint(transform.position))
        {
            Debug.Log("Send to previous food " + idFood);
            Destroy(gameObject);
            ClientManager.client.SendPacket(new Food(idFood), true);
        }
        if (EntityManager.instance.submit.GetComponent<BoxCollider2D>().OverlapPoint(transform.position))
        {
            for (int i = 0; i < 3; i++)
            {
                if (idFood == SampleFood.instance.TakeList[i])
                {
                    Debug.Log("Submit food " + idFood);
                    Destroy(gameObject);
                    SampleFood.instance.SubmitFood(i);
                    EntityManager.instance.time_control.GetComponent<TimerCotroller>().AddTime(3.0f);
                    break;
                }
            }
        }
    }



    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
