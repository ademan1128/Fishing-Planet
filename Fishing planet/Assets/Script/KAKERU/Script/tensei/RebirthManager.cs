using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;

public class RebirthManager : MonoBehaviour
{
    public static RebirthManager Instance;
    public PermanentData permanent;
    private string savePath;
    bool isProcessing;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        savePath = Application.persistentDataPath + "/permanent.json";

        // 前述のロード処理...（省略）

        // ★追加：シーンがロードされたときに自動でRefreshを実行するイベントを登録
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // オブジェクトが破棄されるときはイベントを解除（メモリリーク対策）
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // シーンが切り替わり、新しいシーンのオブジェクトが全部配置し終わったら呼ばれる
    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // ★追加：シーンが読み込まれた際、すでに目標金額に達しているならポイント状態を確実に反映させる
        CheckAndGrantRebirthPoints();
        RefreshAllSkillButtons();
    }

    void Save()
    {
        string json = JsonUtility.ToJson(permanent);
        File.WriteAllText(savePath, json);
    }

    //public void Rebirth()
    //{
    //    if (isProcessing) return;
    //    if (GameManager.instance == null) return;

    //    float targetMoney = 10000f; // 1万

    //    if (GameManager.instance.PlayerMoney >= targetMoney)
    //    {
    //        isProcessing = true;

    //        // 転生ポイントを 10f（float）付与
    //        permanent.rebirthPoints += 1f;
    //        permanent.rebirthCount++;
    //        Save();

    //        float excessMoney = GameManager.instance.PlayerMoney - targetMoney;
    //        GameManager.instance.ExecuteRebirthReset(excessMoney);

    //        RefreshAllSkillButtons();
    //        isProcessing = false;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("100万ゴールドに達していません。");
    //    }
    //}

    // GameManagerからは今まで通りこれを呼び出すだけでOKにします
    public void Rebirth()
    {
        // 1. まずはお金が達しているかチェックしてポイントを付与する
        CheckAndGrantRebirthPoints();

        // ★自動で画面遷移させないために、ここでは「ExecuteRebirthAndSceneReset()」は呼び出しません。
        // ポイントが入ってUIが白くなる処理（RefreshAllSkillButtons）までがここで実行されます。
    }

    // 1. お金が達しているかチェックして、ポイントだけを付与する関数（自動で呼んでOK）
    public void CheckAndGrantRebirthPoints()
    {
        // 既にリセット待機状態（isProcessing）なら二重処理しない
        if (isProcessing) return;
        if (GameManager.instance == null) return;

        int count = permanent.rebirthCount;
        float targetMoney = 10000f * Mathf.Pow(10f, count);
        float currentMoney = GameManager.instance.PlayerMoney;

        if (currentMoney >= targetMoney)
        {
            // ★防壁：この周回で既にポイントを付与した形跡（例えばボタンがすでに押せる等）があれば多重付与を防ぐ
            // ※もし一度付与したら、プレイヤーがリセットボタンを押すまで rebirthCount の更新をここでロックします
            isProcessing = true;

            permanent.rebirthPoints += 1f;
            permanent.rebirthCount++;
            Save();

            RefreshAllSkillButtons();

            Debug.Log($"【転生成功】目標 {targetMoney} 達成。1Pt付与。リセット待ち状態へ移行。");
        }
    }


    // 2. プレイヤーが「ゲームへ」ボタンを押したときに、手動で呼び出すリセット関数
    public void ExecuteRebirthAndSceneReset()
    {
        if (GameManager.instance == null) return;

        // ★重要：ここに入るということは、CheckAndGrantRebirthPointsによって「isProcessing = true」になっているはずです。
        // カウントアップされた後の「今のカウント」の1つ前（達成した目標金額）を正確に計算します。
        int lastCount = permanent.rebirthCount - 1;
        if (lastCount < 0) lastCount = 0;
        float targetMoney = 10000f * Mathf.Pow(10f, lastCount);

        float currentMoney = GameManager.instance.PlayerMoney;
        float excessMoney = Mathf.Max(0f, currentMoney - targetMoney);

        Debug.Log($"シーンリセットを実行。引き継ぎ超過金: {excessMoney}");

        // ★次の周回へ進むため、リセット待機フラグを解除してからシーンをロードする
        isProcessing = false;

        GameManager.instance.ExecuteRebirthReset(excessMoney);
    }

    public void PurchaseRebirthSkill(RebirthSkillData skill)
    {
        if (skill == null) return;

        int skillId = (int)skill.effectType;
        if (permanent.unlockedSkills.Contains(skillId)) return;

        // ★float同士でそのまま比較して消費
        if (permanent.rebirthPoints >= skill.pointCost)
        {
            permanent.rebirthPoints -= skill.pointCost;
            permanent.unlockedSkills.Add(skillId);
            Save();

            RefreshAllSkillButtons();
        }
    }

    public bool IsSkillUnlocked(RebirthSkillType type)
    {
        if (permanent == null) return false;

        bool hasSkill = permanent.unlockedSkills.Contains((int)type);

        // ★起動時にこれがコンソールにどう表示されるか確認してください
        Debug.Log($"スキル判定: {type} / 要素数: {permanent.unlockedSkills.Count} / 結果: {hasSkill}");

        return hasSkill;
    }

    public void RefreshAllSkillButtons()
    {
        RebirthSkillParam[] typeParams = FindObjectsByType<RebirthSkillParam>(FindObjectsSortMode.None);
        foreach (var param in typeParams)
        {
            if (param != null) param.CheckButtonOnOff();
        }
    }
}