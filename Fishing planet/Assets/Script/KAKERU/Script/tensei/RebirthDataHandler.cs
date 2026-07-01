using UnityEngine;
using System.IO;

public class RebirthDataHandler : MonoBehaviour
{
    public static RebirthDataHandler Instance;
    public PermanentData permanent;
    private string savePath;

    private void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        savePath = Application.persistentDataPath + "/permanent.json";
        // ★ここを有効にすると、起動するたびにデータが消えるので確認できます
        if (File.Exists(savePath)) File.Delete(savePath);
        Load();
    }

    void Load()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            JsonUtility.FromJsonOverwrite(json, permanent);
        }
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(permanent);
        File.WriteAllText(savePath, json);
    }

    public void ExecuteRebirth()
    {
        if (GameManager.instance == null) return;
        float targetMoney = 10000f;
        float currentMoney = GameManager.instance.PlayerMoney;

        if (currentMoney < targetMoney) return;

        float excessMoney = currentMoney - targetMoney;
        permanent.rebirthCount++;
        permanent.rebirthPoints += 1f;
        Save();

        GameManager.instance.InSceneRebirthReset(excessMoney);
        GameManager.instance.ReassignReferences();
    }

    public float GetRebirthMultiplier()
    {
        // 転生0回＝1倍、1回＝2倍、2回＝3倍...という計算式
        return 1.0f + (float)permanent.rebirthCount;
    }
}