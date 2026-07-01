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
    public float fishRespawnChance;
    public float BasefishRespawnChance = 30;

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
        // UIが戻ってきたタイミングで、溜まっていた金額を反映
        if (moneyUI == null)
        {
            moneyUI = FindFirstObjectByType<MoneyUI>();
            if (moneyUI != null && pendingMoneyAddition > 0)
            {
                moneyUI.UpdateMoney(PlayerMoney);
                pendingMoneyAddition = 0; // 反映したのでリセット
            }
        }

        if (SkillManager.Instance != null)
        {
            float rebirthBonus = (RebirthDataHandler.Instance != null) ? (RebirthDataHandler.Instance.GetRebirthMultiplier() - 1f) : 0f;

            float skillFeed = SkillManager.Instance.GetTotalMultiplier(SkillEffectType.Feed);
            float skillStores = SkillManager.Instance.GetTotalMultiplier(SkillEffectType.StrengthenStores);
            float skillTime = SkillManager.Instance.GetTotalMultiplier(SkillEffectType.AddTime);
            float skillHook = SkillManager.Instance.GetTotalMultiplier(SkillEffectType.FishingHook);
            float skillPier = SkillManager.Instance.GetTotalMultiplier(SkillEffectType.Pier);
            float skillRepop = SkillManager.Instance.GetTotalMultiplier(SkillEffectType.Repop);
            Feedmagni = skillFeed;
            StrengthenStoresmagni = skillStores;
            AddTimemagni = skillTime;
            FishingHookmagni = skillHook;
            Piermagni = skillPier;
            Repopmagni = skillRepop;
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

    private float pendingMoneyAddition = 0f; // UI反映待ちの金額

    void AddMoneyNow(float fishMoney)
    {
        // 1. まずはお金の計算と加算を確定させる（UIの有無に関係なく）
        float rebirthMultiplier = (RebirthDataHandler.Instance != null) ? RebirthDataHandler.Instance.GetRebirthMultiplier() : 1f;
        Debug.Log($"計算開始: 元金={fishMoney}, 転生倍率={rebirthMultiplier}, ストア倍率={StrengthenStoresmagni}");
        float finalMultiplier = 1.0f + (rebirthMultiplier - 1.0f) + (StrengthenStoresmagni - 1.0f);
        float finalMoney = fishMoney * finalMultiplier;
        Debug.Log($"最終計算結果: {finalMoney}");
        PlayerMoney += Mathf.FloorToInt(finalMoney);
        if (moneyUI != null)
        {
            moneyUI.UpdateMoney(PlayerMoney);
            pendingMoneyAddition = 0; // 反映済みなのでリセット
        }
        else
        {
            // UIがないので、今の全額を「反映待ち」として記録
            pendingMoneyAddition = PlayerMoney;
        }
        if (SceneManager.GetActiveScene().name == "Main game")
        {
            if (Random.Range(0, 100) < (fishRespawnChance * Repopmagni))
            {
                int index = fishCloneList.Count;

                int baseArea = ((PlayerArea - 1) / 2) * 2 + 1;
                int spawnArea = Random.Range(baseArea, baseArea + 2);

                CreateFish(index, spawnArea);
                ALLFish++;
            }
        }

            RebirthButtonController rebirthBtn = FindAnyObjectByType<RebirthButtonController>();
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

            yield return new WaitUntil(() => effect == null || effect.IsFinished());

            AddMoneyNow(price);

            Destroy(obj);
        }

        isShowing = false;
    }


   
    /// <summary>
    /// 転生ボタンから呼ばれる、安全なシーン再読み込み型リセット（バグを完全回避）
    /// </summary>
    public void InSceneRebirthReset(float carryOverMoney)
    {
        // 1. 超過分を一度キャッシュに退避させておく（SceneLoadedでPlayerMoneyに加算されるため）
        carryOverMoneyCache = carryOverMoney;

        // 2. リスト等のクリア
        GetFishList.Clear();
        fishtracked.Clear();

        // 3. 画面内に残っている古い魚のクローンオブジェクトをすべて削除
        foreach (GameObject fish in fishCloneList)
        {
            if (fish != null)
            {
#if UNITY_EDITOR
                if (UnityEditor.EditorUtility.IsPersistent(fish)) continue;
#endif

                Destroy(fish);
            }
        }
        fishCloneList.Clear();

        // 4. エリアを最初のステージに戻す
        PlayerArea = 1;
        area = 1;

        // 所持金自体は一旦0にしておく（SceneLoaded側でcarryOverMoneyCacheが足されるため）
        PlayerMoney = 0;

        Debug.Log($"[転生ロード開始] 超過金 {carryOverMoneyCache} を保持してMain gameシーンを再読み込みします。");

        // 5. シーンを最初から読み直す（これによりUIやFishMove、FishSlotが完璧な順序で初期化されます）
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main game");
    }

    public void ReassignReferences()
    {
        // シーン内に新しく配置されたオブジェクトを再度探し直す
        moneyUI = FindFirstObjectByType<MoneyUI>();
        fishing = FindFirstObjectByType<Fishing>();

        // これでNullでなくなるはずです
        Debug.Log("参照の再接続完了");
    }
}

