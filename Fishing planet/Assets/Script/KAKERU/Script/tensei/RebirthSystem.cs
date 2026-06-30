using UnityEngine;
using UnityEngine.UI;

public class RebirthSystem : MonoBehaviour
{
    public static RebirthSystem Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private Text rebirthPointText;         // 現在の恒常ポイント表示用
    [SerializeField] private Text nextThresholdText;        // 次のポイント獲得に必要な金額表示用
    [SerializeField] private RebirthSkillParam[] rebirthSkillParams; // 恒常スキルボタン群の配列

    [Header("Rebirth Settings")]
    [SerializeField] private float baseTargetMoney = 10000f; // 最初の1ポイントに必要な金額
    [SerializeField] private float costIncreaseRate = 1.5f;  // 次のポイントに必要な金額の倍率（1.5倍ずつ上昇）

    // 現在保持している、消費可能な恒常ポイント（永続セーブ）
    public int CurrentRebirthPoints
    {
        get => PlayerPrefs.GetInt("RebirthPoints", 0);
        private set
        {
            PlayerPrefs.SetInt("RebirthPoints", value);
            PlayerPrefs.Save();
            UpdateUI();
        }
    }

    // これまでに累計何ポイント獲得したか（次の目標金額の計算に使用、永続セーブ）
    public int TotalEarnedPoints
    {
        get => PlayerPrefs.GetInt("TotalEarnedPoints", 0);
        private set
        {
            PlayerPrefs.SetInt("TotalEarnedPoints", value);
            PlayerPrefs.Save();
        }
    }

    // 次にポイントがもらえる目標金額（獲得するたびにコストが跳ね上がる計算）
    public float NextThresholdMoney
    {
        get
        {
            return baseTargetMoney * Mathf.Pow(costIncreaseRate, TotalEarnedPoints);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        UpdateUI();
        CheckAllButtons();
    }

    /// <summary>
    /// GameManager等でお金が増えた際に呼び出し、しきい値判定を行う
    /// </summary>
    public void CheckMoneyThreshold(float currentMoney)
    {
        bool earned = false;

        // 目標金額を超えている間、ループしてポイントを付与（一気に大量のお金が入った場合にも対応）
        while (currentMoney >= NextThresholdMoney)
        {
            CurrentRebirthPoints += 1;
            TotalEarnedPoints += 1;
            earned = true;
            Debug.Log($"【恒常ポイント獲得】目標金額達成！ 現在のポイント: {CurrentRebirthPoints} / 次の目標: {NextThresholdMoney}");
        }

        if (earned)
        {
            UpdateUI();
            CheckAllButtons();
        }
    }

    /// <summary>
    /// 恒常スキルの購入処理
    /// </summary>
    public void PurchaseRebirthSkill(RebirthSkillData skill)
    {
        if (skill == null) return;
        if (IsSkillUnlocked(skill)) return;

        if (CurrentRebirthPoints >= skill.pointCost)
        {
            // ポイントを消費
            CurrentRebirthPoints -= (int)skill.pointCost;

            // 解放フラグを永続保存 (1 = 解放済み)
            PlayerPrefs.SetInt(skill.SaveKey, 1);
            PlayerPrefs.Save();

            Debug.Log($"恒常スキル 【{skill.skillName}】 を解放しました。");

            // UIとボタンの一括更新
            UpdateUI();
            CheckAllButtons();
        }
        else
        {
            Debug.LogWarning("恒常ポイントが足りません。");
        }
    }

    /// <summary>
    /// 指定したスキル（データ型）が解放済みかどうか
    /// </summary>
    public bool IsSkillUnlocked(RebirthSkillData skill)
    {
        if (skill == null) return false;
        return PlayerPrefs.GetInt(skill.SaveKey, 0) == 1;
    }

    /// <summary>
    /// 指定したスキル（enum型）が解放済みかどうか（外部のGameManagerなどから呼ぶ用）
    /// </summary>
    public bool IsSkillUnlocked(RebirthSkillType type)
    {
        return PlayerPrefs.GetInt("RebirthSkill_" + type.ToString(), 0) == 1;
    }

    /// <summary>
    /// テキストUIの更新
    /// </summary>
    public void UpdateUI()
    {
        if (rebirthPointText != null) rebirthPointText.text = "恒常ポイント：" + CurrentRebirthPoints;
        if (nextThresholdText != null) nextThresholdText.text = "次まで：" + Mathf.CeilToInt(NextThresholdMoney);
    }

    /// <summary>
    /// すべての恒常スキルボタンの状態を一括更新する
    /// </summary>
    public void CheckAllButtons()
    {
        foreach (var param in rebirthSkillParams)
        {
            if (param != null) param.CheckButtonOnOff();
        }
    }
}