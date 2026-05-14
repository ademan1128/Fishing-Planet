using Unity.VisualScripting;
using UnityEngine;

public class FishMove : MonoBehaviour
{
    public Transform Lure;
    public int area;
    Vector2 movePosition;
    float speed;
    Lurerange LureRange;
    Fishing MaxNumFish;
    bool isCatch;
    public static int NumFish;
    public bool Eating;
    void Start()
    {
        movePosition = moveRandomPosition();
        Lure = GameObject.Find("Lure").transform;
        LureRange = GameObject.Find("LureRange").GetComponent<Lurerange>();
        MaxNumFish = GameObject.Find("Lure").GetComponent<Fishing>();
        isCatch = false;
        Eating = false;
        NumFish = 0;
        MaxNumFish.MaxNumFish = 3;
    }



    void Update()
    {
        speed = Random.Range(0f, 1f);
        if (LureRange.targetFish.Contains(this)&& NumFish < MaxNumFish.MaxNumFish)//この魚がルアーに反応しているか
                                                                                  //この時のthisは当たった魚のスクリプトを指す
        {
            float distance = Vector2.Distance(transform.position, Lure.position);//

            if (distance < 2f && NumFish < MaxNumFish.MaxNumFish)
            {
                transform.position = Lure.position;//距離が近いときはルアーの位置に移動
                GetComponent<BoxCollider2D>().enabled = false;
                if (isCatch == false)
                {
                    isCatch = true;
                    Eating = true;
                    NumFish++;
                    Debug.Log("NumFish" + NumFish);
                    //GetComponent<BoxCollider2D>().enabled = false;
                }
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position, Lure.position, speed * Time.deltaTime);//ここでルアーに近づいているときはルアーの位置に向かって移動
            }
        }
        else if (Eating == true)
        {
                transform.position = Lure.position;//食べているときはルアーの位置に移動
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
