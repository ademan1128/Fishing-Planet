using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
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
    private bool Changeform = false;
    GameManager gameManager;

    public FishSize fishSize;
    public FishDataSO currentFishData;
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
        gameManager = FindFirstObjectByType<GameManager>();
        Fishing = GameObject.Find("Lure").GetComponent<Fishing>();
        Lure = GameObject.Find("Lure").transform;
        searchFish = GameObject.Find("Lure").GetComponent<SearchFish>();
        Rodtip = GameObject.Find("Rot tip").transform;
        isCatch = false;
        isEating = false;

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
        else if (distance < 0.25f&& searchFish.nearestFishList.Contains(gameObject))
        {
            transform.position = Lure.position;//距離が近いときはルアーの位置に移動
            isEating = true;
            State = FishState.Eating;
            //searchFish.nearestFishList.Remove(searchFish.fishObject[]);
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

    public static List<Vector3> pathPoints = new List<Vector3>();

    void Reeling()
    {
        if (!searchFish.nearestFishList.Contains(gameObject))
            return;
        if(!Changeform)
        {
            if (Object.FindFirstObjectByType<GameManager>() is GameManager gameManager)
            {
                gameManager.ChangeformFish(this);
            }
            Changeform = true;
        }
        float reelSpeed = 5f;
        int spacing = 10;

        if (searchFish.nearestFishList.Count == 0)return;

        GameObject firstFish = searchFish.nearestFishList[0];

        // 先頭魚だけ移動と軌跡更新
        if (gameObject == firstFish)
        {
            transform.position =Vector2.MoveTowards(transform.position,Rodtip.position,reelSpeed * Time.deltaTime);//先頭の魚はロッドの先端に向かって移動

            pathPoints.Insert(0,transform.position);//先頭に現在位置を追加


            //長い時
            //if (pathPoints.Count > 500)
            //{
            //    pathPoints.RemoveAt(pathPoints.Count - 1);
            //}

            float firstDistance =Vector2.Distance(transform.position,Rodtip.position);//先頭の魚とロッドの先端の距離を測る

            if (firstDistance < 1f)//最初の魚がロッドの先端に近づいたら次の魚も移動開始
            {
                for (int i = 0; i < searchFish.nearestFishList.Count; i++)
                {
                    if (searchFish.nearestFishList[i] == null)continue;
                    FishMove fishMove =searchFish.nearestFishList[i].GetComponent<FishMove>();
                    fishMove.State = FishState.GetFish;
                }
            }
        }

        for (int i = 1; i < searchFish.nearestFishList.Count; i++)//2匹目以降の魚は先頭の魚の軌跡に沿って移動
        {
            GameObject fishObj =　searchFish.nearestFishList[i];
            if (fishObj == null)continue;
            int index = i * spacing;
            if (index >= pathPoints.Count)continue;

            fishObj.transform.position =Vector2.MoveTowards( fishObj.transform.position,pathPoints[index],reelSpeed * Time.deltaTime);

            Vector2 dir =(pathPoints[index] - fishObj.transform.position).normalized;

            fishObj.transform.rotation =Quaternion.FromToRotation(Vector3.up,dir);
        }
    }

    void GetFish()
    {
        if (currentFishData == null)
        {
            gameManager.ChangeformFish(this);
        }

        gameManager.GetFishList.Add(currentFishData);
        gameManager.AddMoney(currentFishData.fishPrice);

        Destroy(gameObject);
    }
    Vector2 moveRandomPosition()//移動先をランダムに決める
    {
        int index = area - 1;//indexは配列の要素数
        float rndX = Random.Range(gameManager.areaMin[index].x, gameManager.areaMax[index].x);
        float rndY = Random.Range(-5, 0);
        return new Vector2(rndX, rndY);
    }

}
