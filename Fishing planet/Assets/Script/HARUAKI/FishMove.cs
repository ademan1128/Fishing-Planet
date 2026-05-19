using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class FishMove : MonoBehaviour
{
    public Transform Lure;
    public Transform Rodtip;
    public int area;
    Vector2 movePosition;
    float speed;
    Fishing MaxNumFish;
    bool isCatch;
    public static int NumFish;
    public bool Eating;
    public float SearchDistance = 1f;
    SearchFish searchFish;
    public static List<int> GetFishArea = new List<int>();
    float distance;
    float Roddistance;
    Fishing Fishing;
    void Start()
    {
        movePosition = moveRandomPosition();
        Lure = GameObject.Find("Lure").transform;
        MaxNumFish = GameObject.Find("Lure").GetComponent<Fishing>();
        searchFish =GameObject.Find("Lure").GetComponent<SearchFish>();
        Rodtip = GameObject.Find("Rot tip").transform;
        isCatch = false;
        Eating = false;
        //NumFish = 0;
        MaxNumFish.MaxNumFish = 3;
        Fishing = GameObject.Find("Lure").GetComponent<Fishing>();
    }



    void Update()
    {
        speed = Random.Range(0.5f, 1f);
        if (searchFish.nearestFishList.Contains(gameObject))
        {
            distance = Vector2.Distance(transform.position, Lure.position);//ここで魚とルアーの距離を測る
            Roddistance = Vector2.Distance(transform.position, Rodtip.position);
            if (Eating == true && Fishing.GotFish == true)
            {
                transform.position = Rodtip.position;
                if (Roddistance < 1f)
                {
                    GetFishArea.Add(area);
                    Destroy(gameObject);
                }
            } 
            else
            if(distance < 0.5f)
            {
                transform.position = Lure.position;//距離が近いときはルアーの位置に移動
                if (isCatch == false)
                {
                    isCatch = true;
                    NumFish++;
                    Debug.Log("NumFish" + NumFish);
                    Eating = true;
                }
            }
            else
            {
                if ( MaxNumFish.CanFishGet == true)
                transform.position = Vector2.MoveTowards(transform.position, Lure.position, speed * Time.deltaTime);//ここでルアーに近づいているときはルアーの位置に向かって移動
            }

        } else
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
