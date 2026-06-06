using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    int rnd;
    //public List<Sprite> Sea1List = new List<Sprite>();
    public List<GameObject> fishCloneList = new List<GameObject>();//生成した魚を保存するリスト
    [SerializeField] Transform Lure;
    Fishing fishing;
    [SerializeField] GameObject prefabObj;
    [SerializeField] Sprite fishSprite;
    [SerializeField]List<FishDataSO> fishDataList = new List<FishDataSO>();//魚のデータを保存するリスト
    public List<GameObject> fishtracked = new List<GameObject>();//釣り上げるために追跡している魚を保存するリスト
    public List<FishDataSO> GetFishList = new List<FishDataSO>();//最終的に釣れた魚のデータを保存するリスト
    public Vector2[] areaMin;
    public Vector2[] areaMax;
    public static int ALLFish = 10;
    int area = 1;
    int PlayerMoney;
    public int PlayerArea;
    public class AreaSizeRate 
    {
        public float small;
        public float medium;
        public float large;
    }

    private List<AreaSizeRate> areaSizeRates;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {

        Lure = GameObject.Find("Lure").transform;
        fishing = Lure.GetComponent<Fishing>();
        for (int i = 0; i < ALLFish; i++)
        {
            if (i >= 5)
            {
                area = 2;
            }
            CreateFish(i, area);
        }
    }

    public void Reset()
    {
        GetFishList.Clear();
    }

    public void AllReset()
    {
        GetFishList.Clear();
        PlayerMoney = 0;
        PlayerArea = 1;
    }
    //FishSize GetRandomFishSize(int area)

    //    {
    //        AreaSizeRate rate = areaSizeRates[area - 1];
    //        List<float> weights = new List<float>
    //    {
    //        rate.small,
    //        rate.medium,
    //        rate.large
    //    };

    //        int index = GetRandomIndex(weights);

    //        return (FishSize)index;

    //    }

    void Update()
    {
        if (fishing.GotFish)
        {
            //foreach (int area in FishMove.GetFishArea)
            //{
            //    List<float> weights = new List<float>();//重みを入れるリストを作成してるよ

            //    foreach (FishDataSO fish in fishDataList)//fishDataListにあるFishDataSOの中に入ってる魚を重みで抽選してるよ多分
            //    {
            //        weights.Add(fish.areafishWeight[area-1]);
            //        //Sprite catchFish = Sea1List[rnd];
            //    }
            //    rnd = GetRandomIndex(weights);
            //    FishDataSO catchFish = fishDataList[rnd];//ここで重みで抽選した魚を取得してる
               
            //    if (sr != null && catchFish.fishSprite != null) // ※SOにfishSpriteがあると仮定
            //    {
            //        sr.sprite = catchFish.fishSprite;
            //    }
            //    Debug.Log("釣れた魚：" + catchFish.fishName);
            //}
            FishMove.GetFishArea.Clear();
            fishing.GotFish = false;
        }
    }

    void CreateFish(int index,int area)
    {
        GameObject obj = Instantiate(prefabObj);
        obj.name = "Fish" + (index + 1);
        int areaindex = area - 1;//indexは配列の要素数
        float rndX = Random.Range(areaMin[areaindex].x, areaMax[areaindex].x);
        float rndY = Random.Range(-5, 0);
        obj.transform.position = new Vector2(rndX, rndY);
        FishMove fishMove = obj.GetComponent<FishMove>();
        fishMove.area = area;
        if (PlayerArea <= 5)//先に魚にサイズだけ決める
        {
            fishMove.fishSize = FishSize.Small;
        }
        else if (PlayerArea <= 10)
        {
            fishMove.fishSize = FishSize.Medium;
        }
        else
        {
            fishMove.fishSize = FishSize.Large;
        }
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.sprite = fishSprite;
        fishCloneList.Add(obj);
    }

    int GetRandomIndex(List<float> weights)
    {
        float total = 0f;

        foreach (var w in weights)
        {
            total += w;
        }
        float r = Random.Range(0, total);
        for (int i = 0; i < weights.Count; i++)
        {
            r -= weights[i];
            if (r <= 0)
            {
                return i;
            }
        }
        return weights.Count - 1;
    }

    public void ChangeformFish(FishMove caughtFish)
    {
        if (caughtFish == null) return;
        int area = caughtFish.area;
        List<FishDataSO> targetFish = new List<FishDataSO>();
        foreach (FishDataSO fish in fishDataList)
        {
            if (fish.fishSize == caughtFish.fishSize)
            {
                targetFish.Add(fish);
            }
        }
        List<float> weights = new List<float>();
        foreach (FishDataSO fish in targetFish)
        {
            weights.Add(fish.areafishWeight[area - 1]);
        }
        rnd = GetRandomIndex(weights);
        FishDataSO catchFish = targetFish[rnd];
        caughtFish.currentFishData = catchFish;//釣れた魚のデータをFishMoveに保存
        SpriteRenderer sr = caughtFish.GetComponent<SpriteRenderer>();
        if (sr != null && catchFish.fishSprite != null)
        {
            sr.sprite = catchFish.fishSprite;
            //GetFishList.Add(catchFish);
            Debug.Log("釣れた");
        }
    }

    public void AddMoney(int FishMoney)
    {
        PlayerMoney += FishMoney;
        Debug.Log("現在の所持金：" + PlayerMoney);
    }
}

