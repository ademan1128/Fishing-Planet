using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RebirthManager : MonoBehaviour
{
    public static RebirthManager Instance;
    public PermanentData permanent;
    private string savePath;

    [Header("UI設定")]
    public Button rebirthButton;
    public Image buttonImage;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        savePath = Application.persistentDataPath + "/permanent.json";
        Load();
    }

    void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            JsonUtility.FromJsonOverwrite(json, permanent); // 直接上書きするほうが安全です
        }
    }

    private void Update()
    {
        if (GameManager.instance == null) return;

        // 転生に必要な金額（例：100万で固定、または計算式に応じて変更可能）
        float targetMoney = 10000f;
        float currentMoney = GameManager.instance.PlayerMoney;
        Debug.Log(currentMoney);

        if (currentMoney >= targetMoney)
        {
            // 100万以上：白と黒で点滅
            float alpha = Mathf.PingPong(Time.time * 2f, 1f);
            buttonImage.color = Color.Lerp(Color.black, Color.white, alpha);
            rebirthButton.interactable = true;
        }
        else
        {
            // 100万未満：薄灰色で固定
            buttonImage.color = new Color(0.8f, 0.8f, 0.8f, 1f);
            rebirthButton.interactable = false;
        }
    }

    /// <summary>
    /// 転生を実行する
    /// </summary>
    public void ExecuteRebirth()
    {
        Debug.Log("転生ボタンが押された！");
        if (GameManager.instance == null) return;

        float targetMoney = 10000f; // 転生ライン
        float currentMoney = GameManager.instance.PlayerMoney;

        if (currentMoney < targetMoney) return;

        float excessMoney = currentMoney - targetMoney;

        permanent.rebirthCount++;
        permanent.rebirthPoints += 1f;
        Save();

        GameManager.instance.InSceneRebirthReset(excessMoney);
        RefreshAllSkillButtons();
        GameManager.instance.ReassignReferences();
    }

    /// <summary>
    /// 【維持】全機能2倍のロジック
    /// </summary>
    public float GetRebirthMultiplier()
    {
        return permanent.rebirthCount > 0 ? 2f : 1f;
    }

    public void RefreshAllSkillButtons()
    {
        RebirthSkillParam[] typeParams = FindObjectsByType<RebirthSkillParam>(FindObjectsSortMode.None);
        foreach (var param in typeParams)
        {
            if (param != null) param.CheckButtonOnOff();
        }
    }

    void Save()
    {
        string json = JsonUtility.ToJson(permanent);
        File.WriteAllText(savePath, json);
    }
}