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

    void Start()
    {
        movePosition = moveRandomPosition();
        Lure = GameObject.Find("Lure").transform;//ルアーの位置を取得
        LureRange = GameObject.Find("LureRange").GetComponent<Lurerange>();//ルアーの当たり判定を取得
        MaxNumFish = GameObject.Find("Lure").GetComponent<Fishing>();


    }



    void Update()
    {
        speed = Random.Range(0f, 1f);
        if (LureRange.LureMove == true)//ルアーが近づいたときの処理
        {
            GetComponent<BoxCollider2D>().enabled = false;//ルアーに近づいたときは当たり判定を消す
            float distance = Vector2.Distance(transform.position,Lure.position);

            if (distance < 1f)//&& NumFish <= MaxNumFish.MaxNumFish
            {
                transform.position = Lure.position;//距離が近いときはルアーの位置に移動
            }
            else
            {
                transform.position = Vector2.MoveTowards(transform.position,Lure.position,speed * Time.deltaTime//移動先に向かって移動
                );
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, movePosition) < 0.1f)//ここで移動先に近づいたら新しい移動先を決める
            {
                movePosition = moveRandomPosition();
            }
            transform.position = Vector2.MoveTowards(transform.position,movePosition,speed * Time.deltaTime);//移動先に向かって移動
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
