using Assets.Scripts.GamePlay;

using UnityEngine;
using UnityEngine.UIElements;

public class EntityManager : MonoBehaviour
{
    public static EntityManager instance;
    public GameObject trashcan;
    public GameObject SendtoServer;
    [SerializeField] private float timer = 0.0f, previous_time = 0.0f;
    public GameObject new_Food;

    
    private void Awake()
    {
        if (instance == null)
        instance = this; 
    }
    // Start is called before the first frame update
    void Start()
    {
        Spawn_Food();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer - previous_time > 3)
        {
            previous_time = timer;
            Spawn_Food();
        }
    }

    private void Spawn_Food()
    {
        int seed = Random.Range(0, 5);
        Debug.Log($"Seed = {seed}");
        GameObject nf = Instantiate(new_Food, this.transform) as GameObject;
        nf.GetComponent<DragableItem>().food = new Food(seed);
        nf.name = nf.GetComponent<DragableItem>().food.name.ToString();
        nf.GetComponent<SpriteRenderer>().color = UIManager.LoadColor(nf.GetComponent<DragableItem>().food);
    }

    public void Spawn_Food(Food food)
    {
        GameObject nf = Instantiate(new_Food, new Vector2(2,3), Quaternion.identity) as GameObject;
        nf.GetComponent<DragableItem>().food = food;
        nf.name = nf.GetComponent<DragableItem>().food.name.ToString();
        nf.GetComponent<SpriteRenderer>().color = UIManager.LoadColor(nf.GetComponent<DragableItem>().food);
    }

}
