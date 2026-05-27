using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.Rendering.DebugUI.Table;

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
    public bool isEating;
    public float SearchDistance = 1f;
    SearchFish searchFish;
    public static List<int> GetFishArea = new List<int>();
    float distance;
    float Roddistance;
    Fishing Fishing;
    Fishing isReeling;

    public enum FishState
    {
        Swimming,
        Tracking,
        Eating,
        Reeling,
        GetFish
    }

    public FishState State = FishState.Swimming;//初期状態はswimming

    private void Awake()
    {
        Fishing = GameObject.Find("Lure").GetComponent<Fishing>();
        Lure = GameObject.Find("Lure").transform;
        searchFish = GameObject.Find("Lure").GetComponent<SearchFish>();
        Rodtip = GameObject.Find("Rot tip").transform;
        isCatch = false;
        isEating = false;
        //NumFish = 0;

    }

    void Start()
    {
        movePosition = moveRandomPosition();
    }



    void Update()
    {
        switch (State)
        {
            case FishState.Swimming:Swimming();
                break;

            case FishState.Tracking:Tracking();
                break;

            case FishState.Eating:Eating();
                break;

            case FishState.Reeling:Reeling();
                break;
            case FishState.GetFish: GetFish();
                break;
        }

    }

    void Swimming()
    {
        speed = Random.Range(0.5f, 1f);

        if (Vector2.Distance(transform.position, movePosition) < 0.1f)//ここで移動先に近づいたら新しい移動先を決める
        {
            movePosition = moveRandomPosition();
        }
        transform.position = Vector2.MoveTowards(transform.position, movePosition, speed * Time.deltaTime);//移動先に向かって移動

        if (searchFish.nearestFishList.Contains(gameObject))
        {
            State = FishState.Tracking;
        }
    }

    void Tracking()
    {
        distance = Vector2.Distance(transform.position, Lure.position);//ここで魚とルアーの距離を測る

        if (Fishing.CanFishGet == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, Lure.position, speed * Time.deltaTime);
        }
        if (Fishing.isReeling && isEating == false)
        {
            State = FishState.Swimming;
        }
        else if (distance < 0.5f&& searchFish.nearestFishList.Contains(gameObject))
        {
            transform.position = Lure.position;//距離が近いときはルアーの位置に移動
            isEating = true;
            State = FishState.Eating;
            Debug.Log("食べました");

        }

    }

    void Eating()
    {
        if (isEating == true)
        {
            transform.position = Lure.position;
            if (Fishing.isReeling)
            {
                State = FishState.Reeling;
            }
        }
    }

    void Reeling()
    {
        Roddistance = Vector2.Distance(transform.position, Rodtip.position);
        for (int i = 0; i < searchFish.nearestFishList.Count; i++)
        {
            Debug.Log(searchFish.nearestFishList[i]);
            float interval = 1f; 
            if (searchFish.nearestFishList[i] == null)continue;
            Vector3 dir = (Rodtip.position - searchFish.nearestFishList[i].transform.position).normalized;
            searchFish.nearestFishList[i].transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            searchFish.nearestFishList[i].transform.position = Lure.position - transform.up * (interval * i);
            if (Roddistance < 1f)
            {
                Debug.Log("GetFish");
                searchFish.nearestFishList[i].transform.rotation = Quaternion.identity;
                State = FishState.GetFish;
            }
        }
    }

    void GetFish()
    {
        if (isEating == true && Fishing.GotFish == true)
        {
            transform.position = Rodtip.position;
            if (Roddistance < 1f)
            {
                GetFishArea.Add(area);
                Destroy(gameObject);
            }
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
