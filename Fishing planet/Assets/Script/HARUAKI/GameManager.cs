using System.Collections;
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
    [SerializeField] private GameObject fishPricePrefab;
    [SerializeField] private Transform canvas;
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
    private Queue<float> priceQueue = new Queue<float>();
    private bool isShowing;
    public Vector2[] areaMin;
    public Vector2[] areaMax;
    //海の中の魚母数増やす
    public  int ALLFish = 10;
    public int MaxNumFish = 5;
    int area = 1;
    public float PlayerMoney;
    //これこれ桟橋
    public int PlayerArea ;
    //これが倍率変更のやつ
    public float Feedmagni = 1;//エサ
    public float StrengthenStoresmagni = 1;
    public float AddTimemagni = 1;
    public float FishingHookmagni = 1;
    public float Piermagni = 1;
    public float Repopmagni = 1;
    [Header("魚のリポップ確率(%)")]
    [Range(0, 100)]
    //ここで設定した確率で魚がリポップする
    //パーセント
    public int fishRespawnChance = 30;

    private int effectCount = 0;

    public enum StageTime
    {
        Noon,
        Evening,
        Night,
        Rain,
        Snow,
        Thunder

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
        PlayerArea =1;
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
        else if (PlayerArea == 13)
        {
            stageTime = StageTime.Noon;
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
    }


    void Update()
    {
        if (fishing.GotFish)
        {
            FishMove.GetFishArea.Clear();
            fishing.GotFish = false;
        }

        if (SkillManager.Instance != null)
        {
            Feedmagni = SkillManager.Instance.GetTotalMultiplier(SkillEffectType.Feed);
            StrengthenStoresmagni = SkillManager.Instance.GetTotalMultiplier(SkillEffectType.StrengthenStores);
            AddTimemagni = SkillManager.Instance.GetTotalMultiplier(SkillEffectType.AddTime);
            FishingHookmagni = SkillManager.Instance.GetTotalMultiplier(SkillEffectType.FishingHook);
            Piermagni = SkillManager.Instance.GetTotalMultiplier(SkillEffectType.Pier);
            Repopmagni = SkillManager.Instance.GetTotalMultiplier(SkillEffectType.Repop);
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
            total += w * FishingHookmagni;
        }
        float r = Random.Range(0, total);
        for (int i = 0; i < weights.Count; i++)
        {
            r -= weights[i] * FishingHookmagni;
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
            ShowFishPrice(catchFish.fishPrice);
            //GetFishList.Add(catchFish);
            Debug.Log("釣れた");
        }
    }



    void AddMoneyNow(float fishMoney)
    {
        PlayerMoney += Mathf.FloorToInt(fishMoney * StrengthenStoresmagni);
        moneyUI.UpdateMoney(PlayerMoney);

        Debug.Log("現在の所持金：" + PlayerMoney);

        if (Random.Range(0, 100) < fishRespawnChance)
        {
            int index = fishCloneList.Count;

            int baseArea = ((PlayerArea - 1) / 2) * 2 + 1;
            int spawnArea = Random.Range(baseArea, baseArea + 2);

            CreateFish(index, spawnArea);
            ALLFish++;
        }
    }

    public void ShowFishPrice(float price)
    {
        if (canvas == null)
        {
            canvas = FindFirstObjectByType<Canvas>().transform;
        }
        priceQueue.Enqueue(price);

        if (!isShowing)
        {
            StartCoroutine(ShowPriceCoroutine());
        }

    }
    IEnumerator ShowPriceCoroutine()
    {
        isShowing = true;

        while (priceQueue.Count > 0)
        {
            float price = priceQueue.Dequeue();

            GameObject obj = Instantiate(fishPricePrefab, canvas);

            EffctMoney effect = obj.GetComponent<EffctMoney>();
            effect.SetPrice(price);

            // フェードアウトが終わるまで待つ
            yield return new WaitUntil(() => effect == null || effect.IsFinished());

            // ★ここでお金を増やす
            AddMoneyNow(price);

            Destroy(obj);
        }

        isShowing = false;
    }
}

