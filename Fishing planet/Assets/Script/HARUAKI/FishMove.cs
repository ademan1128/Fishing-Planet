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
            gameManager.fishtracked.Add (gameObject);
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
        if (!gameManager.fishtracked.Contains(gameObject))
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

        int myIndex = gameManager.fishtracked.IndexOf(gameObject);
        if (myIndex == -1) return;

        // 【先頭の魚（0番目）の処理】
        if (myIndex == 0)
        {
            // ロッドの先端に向かって移動
            transform.position = Vector2.MoveTowards(transform.position, Rodtip.position, reelSpeed * Time.deltaTime);

            // 自分の現在位置を軌跡（パス）の先頭に追加
            pathPoints.Insert(0, transform.position);

            // 軌跡データが溜まりすぎないように制限（1000件まで）
            if (pathPoints.Count > 1000)
            {
                pathPoints.RemoveAt(pathPoints.Count - 1);
            }

            // ロッドの先端に近づいたら、釣られた確定リストの全員をGetFish状態にする
            float firstDistance = Vector2.Distance(transform.position, Rodtip.position);
            if (firstDistance < 1f)
            {
                for (int i = 0; i < gameManager.fishtracked.Count; i++)
                {
                    GameObject fishObj = gameManager.fishtracked[i];
                    if (fishObj != null)
                    {
                        fishObj.GetComponent<FishMove>().State = FishState.GetFish;
                    }
                }
            }
        }
        // 【2匹目以降の魚の処理】
        else
        {
            // 自分の順番に応じた軌跡の番号を計算
            int targetPathIndex = myIndex * spacing;

            // 軌跡データがそこまで記録されていれば、その位置を追いかける
            if (targetPathIndex < pathPoints.Count)
            {
                transform.position = Vector2.MoveTowards(transform.position, pathPoints[targetPathIndex], reelSpeed * Time.deltaTime);

                // 進む方向を向く
                Vector2 dir = ((Vector2)pathPoints[targetPathIndex] - (Vector2)transform.position).normalized;
                if (dir != Vector2.zero)
                {
                    transform.rotation = Quaternion.FromToRotation(Vector3.up, dir);
                }
            }
            else
            {
                // まだ軌跡が足りない場合は、とりあえず1つ前の魚をそのまま追いかける
                GameObject frontFish = gameManager.fishtracked[myIndex - 1];
                if (frontFish != null)
                {
                    transform.position = Vector2.MoveTowards(transform.position, frontFish.transform.position, reelSpeed * Time.deltaTime);
                }
            }
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

        if (gameManager.fishtracked.Contains(gameObject))
        {
            gameManager.fishtracked.Remove(gameObject);
        }
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
