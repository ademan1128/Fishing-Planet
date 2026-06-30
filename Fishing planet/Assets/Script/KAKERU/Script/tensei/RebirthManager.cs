using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class RebirthManager : MonoBehaviour
{
    public static RebirthManager Instance;
    public PermanentData permanent;
    private string savePath;

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

        // ※ロード処理はここに残す
    }

    void Save()
    {
        string json = JsonUtility.ToJson(permanent);
        File.WriteAllText(savePath, json);
    }

    /// <summary>
    /// 転生を実行する（プレイヤーが「転生ボタン」を押したときに呼び出す）
    /// </summary>
    public void ExecuteRebirth()
    {
        if (GameManager.instance == null) return;

        // 1. 現在の目標金額を計算
        int count = permanent.rebirthCount;
        float targetMoney = 10000f * Mathf.Pow(10f, count);
        float currentMoney = GameManager.instance.PlayerMoney;

        if (currentMoney < targetMoney)
        {
            Debug.LogWarning("目標金額に達していないため、転生できません。");
            return;
        }

        // 2. 超過金（再配布する原資）を計算
        float excessMoney = currentMoney - targetMoney;

        // 3. 転生カウントを更新して保存
        permanent.rebirthCount++;
        permanent.rebirthPoints += 1f;
        Save();

        Debug.Log($"【転生実行】目標金額: {targetMoney} / 超過金: {excessMoney} を再配布します。");

        // 4. GameManager側に超過金を渡して、同一シーン内リセットを実行
        GameManager.instance.InSceneRebirthReset(excessMoney);

        // 5. ボタンのUI更新
        RefreshAllSkillButtons();
    }

    /// <summary>
    /// 通常スキルの効果を倍にするための倍率を取得する
    /// 通常時: 1倍 / 転生1回: 2倍 / 転生2回: 4倍（あるいは一律2倍なら条件分岐）
    /// </summary>
    public float GetRebirthMultiplier()
    {
        if (permanent.rebirthCount > 0)
        {
            // 転生していれば2倍（ここをお好みの計算式に変えられます）
            return 2f;
        }
        return 1f;
    }

    public void RefreshAllSkillButtons()
    {
        // シーン内のスキルボタンに一斉に表示を更新させる
        RebirthSkillParam[] typeParams = FindObjectsByType<RebirthSkillParam>(FindObjectsSortMode.None);
        foreach (var param in typeParams)
        {
            if (param != null) param.CheckButtonOnOff();
        }
    }
}