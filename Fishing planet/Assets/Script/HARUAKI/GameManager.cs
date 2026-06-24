using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Burst.Intrinsics.Arm;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    int rnd;
    //public List<Sprite> Sea1List = new List<Sprite>();
    public List<GameObject> fishCloneList = new List<GameObject>();//生成した魚を保存するリスト
    [SerializeField] Transform Lure;
    Fishing fishing;
    [SerializeField] private MoneyUI moneyUI;
    [SerializeField] GameObject prefabObj;
    [SerializeField] Sprite[] fishSprite;
    [SerializeField]List<FishDataSO> fishDataList = new List<FishDataSO>();//魚のデータを保存するリスト
    public List<GameObject> fishtracked = new List<GameObject>();//釣り上げるために追跡している魚を保存するリスト
    public List<FishDataSO> GetFishList = new List<FishDataSO>();//最終的に釣れた魚のデータを保存するリスト
    public Vector2[] areaMin;
    public Vector2[] areaMax;

    int area = 1;
    public int PlayerMoney;


    //スキルツリーの効果倍率などをここで設定する
    //スキルツリーの効果範囲倍率をここで設定する
    public float SearchDistanceMultiplier = 1f;

    //桟橋追加したらここの値を直接スキルツリーで＋
    public int PlayerArea = 1;

    //海にいる魚の数を増やすなら
    public int ALLFish = 10;


    public enum StageTime
    {
        Noon,
        Evening,
        Night,
        Rain,
        Snow,
        Thunder,
    }
    public StageTime stageTime;
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
        // イベントにイベントハンドラーを追加
        SceneManager.sceneLoaded += SceneLoaded;
        // シーンの読み込み
        SceneManager.LoadScene("Main game");
        moneyUI.UpdateMoney(PlayerMoney);
        Lure = GameObject.Find("Lure").transform;
        fishing = Lure.GetComponent<Fishing>();

    }

    void SceneLoaded(Scene nextScene, LoadSceneMode mode)
    {
        if (PlayerArea == 1 ||PlayerArea == 2)
        {
            stageTime = StageTime.Noon;
        }
        else if (PlayerArea == 3 ||PlayerArea == 4)
        {
            stageTime = StageTime.Evening;
        }
        else if (PlayerArea == 5 || PlayerArea == 6)
        {
            stageTime = StageTime.Night;
        }
        else if (PlayerArea == 7 || PlayerArea == 8)
        {
            stageTime = StageTime.Rain;
        }
        else if (PlayerArea == 9 || PlayerArea == 10)
        {
            stageTime = StageTime.Snow;
        }
        else if (PlayerArea == 11 || PlayerArea == 12)
        {
            stageTime = StageTime.Thunder;
        }
        if (SceneManager.GetActiveScene().name == "Main game")
        {
            moneyUI = FindFirstObjectByType<MoneyUI>();
            moneyUI.UpdateMoney(PlayerMoney);

            for (int i = 0; i < ALLFish; i++)
            {
                if (i >= ALLFish/2)
                {
                    CreateFish(i, area+1);
                }
                else
                {
                    CreateFish(i, area);
                }
            }
            Debug.Log(nextScene.name);
            Debug.Log(mode);
        }
    }



    public void Reset()
    {
        GetFishList.Clear();
        fishtracked.Clear();

    }

    public void AllReset()
    {
        GetFishList.Clear();
        PlayerMoney = 0;
        PlayerArea = 1;
        SearchDistanceMultiplier = 1f;
    }


    void Update()
    {
        if (fishing.GotFish)
        {
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
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        if (GameManager.instance.stageTime == GameManager.StageTime.Noon)//先に魚にサイズだけ決める
        {
            fishMove.fishSize = FishSize.Small;
            sr.sprite = fishSprite[0];
        }
        else if (GameManager.instance.stageTime == GameManager.StageTime.Evening)
        {
            fishMove.fishSize = FishSize.Medium;
            sr.sprite = fishSprite[1];
        }
        else if(GameManager.instance.stageTime == GameManager.StageTime.Night)
        {
            fishMove.fishSize = FishSize.Large;
            sr.sprite = fishSprite[2];
        }

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
        moneyUI.UpdateMoney(PlayerMoney);
        Debug.Log("現在の所持金：" + PlayerMoney);
    }
}

