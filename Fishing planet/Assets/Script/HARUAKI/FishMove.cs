using Unity.VisualScripting;
using UnityEngine;

public class FishMove : MonoBehaviour
{
    public Transform Lure;
    public int area;
    Vector2 movePosition;
    float speed;
    Fishing MaxNumFish;
    bool isCatch;
    public static int NumFish;
    public bool Eating;
    public float SearchDistance = 1f;
    SearchFish searchFish;
    void Start()
    {
        movePosition = moveRandomPosition();
        Lure = GameObject.Find("Lure").transform;
        MaxNumFish = GameObject.Find("Lure").GetComponent<Fishing>();
        searchFish =GameObject.Find("Lure").GetComponent<SearchFish>();
        isCatch = false;
        Eating = false;
        //NumFish = 0;
        MaxNumFish.MaxNumFish = 3;
    }



    void Update()
    {
        speed = Random.Range(0.5f, 1f);
        if (searchFish.nearestFishList.Contains(gameObject)&& MaxNumFish.CanFishGet && MaxNumFish.CanFishGet==true)
        {
            float distance = Vector2.Distance(transform.position, Lure.position);//ここで魚とルアーの距離を測る
            if (distance < 0.5f)
            {
                transform.position = Lure.position;//距離が近いときはルアーの位置に移動
                GetComponent<BoxCollider2D>().enabled = false;
                if (isCatch == false)
                {
                    isCatch = true;
                    Eating = true;
                    NumFish++;
                    Debug.Log("NumFish" + NumFish);
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, Lure.position, speed * Time.deltaTime);//ここでルアーに近づいているときはルアーの位置に向かって移動
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, movePosition) < 0.1f)//ここで移動先に近づいたら新しい移動先を決める
            {
                movePosition = moveRandomPosition();
            }
            transform.position = Vector2.MoveTowards(transform.position, movePosition, speed * Time.deltaTime);//移動先に向かって移動
        }
    }

    Vector2 moveRandomPosition()//移動先をランダムに決める
    {
        if (area == 1)
            {
                int rndX = Random.Range(0, 10);
                int rndY = Random.Range(-5, 0);
                return new Vector2(rndX, rndY);
            }
    return Vector2.zero;
    }

}
