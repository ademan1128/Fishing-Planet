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
            //中鉢追加
            if (carryOverMoneyCache > 0f)
            {
                PlayerMoney += carryOverMoneyCache;
                carryOverMoneyCache = 0f;
            }
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

    //public void AllReset()
    //{
    //    GetFishList.Clear();
    //    PlayerMoney = 0;
    //    PlayerArea = 1;
    //}
    // GameManager.cs の該当箇所を修正

    /// <summary>
    /// 転生用のリセット処理（目標金額を差し引いた余剰金額を持ち越す）
    /// </summary>
    /// <param name="carryOverMoney">次の周回に持ち越すお金</param>
    public void RebirthReset(float carryOverMoney)
    {
        GetFishList.Clear();

        // 完全に0にするのではなく、超過分を次の周回の初期値にする
        PlayerMoney = carryOverMoney;
        PlayerArea = 1;

        // UIの表示も超過分のアカウントに更新する
        if (moneyUI != null)
        {
            moneyUI.UpdateMoney(PlayerMoney);
        }
    }

    // ここから下は中鉢が編集（Updata手前まで）

    // 転生時に一時的に超過分のお金をキープしておくための変数（クラスの変数宣言欄に置いてもOK）
    private float carryOverMoneyCache = 0f;

    /// <summary>
    /// 転生リセットの実行（RebirthManagerから呼び出される）
    /// </summary>
    /// <param name="excessMoney">次の周回に引き継ぐ超過金額</param>
    public void ExecuteRebirthReset(float excessMoney)
    {
        // 超過分を一度キャッシュに保存しておく
        carryOverMoneyCache = excessMoney;

        // リスト等のクリア
        GetFishList.Clear();
        fishtracked.Clear();

        // 画面内に残っている古い魚のクローンオブジェクトをすべて削除する
        foreach (GameObject fish in fishCloneList)
        {
            if (fish != null)
            {
                // ★追加：もしオブジェクトがシーン上のクローンではなく「アセット（プレハブ）」なら削除をスキップする
                if (UnityEditor.EditorUtility.IsPersistent(fish)) continue;

                Destroy(fish);
            }
        }
        fishCloneList.Clear();

        // エリアを最初のステージに戻す
        PlayerArea = 1;

        // シーンを最初から読み直す
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main game");
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
            //GetFishList.Add(catchFish);
            Debug.Log("釣れた");
        }
    }

    public void AddMoney(float FishMoney)
    {
        PlayerMoney += Mathf.FloorToInt(FishMoney * StrengthenStoresmagni);
        moneyUI.UpdateMoney(PlayerMoney);
        Debug.Log("現在の所持金：" + PlayerMoney);

        // ★安全な一元管理：余計な金額計算をGameManager側でせず、判定をRebirthManagerに一任する
        if (RebirthManager.Instance != null)
        {
            RebirthManager.Instance.CheckAndGrantRebirthPoints();
        }
    }
}

