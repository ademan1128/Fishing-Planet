using UnityEngine;
using System.IO;

public class RebirthManager : MonoBehaviour
{
    public static RebirthManager Instance;
    public PermanentData permanent;
    SessionData session;
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

        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            permanent = JsonUtility.FromJson<PermanentData>(json);
        }
        else
        {
            permanent = new PermanentData();
        }
            session = new SessionData();
    }
   
    void Save()
    {
        string json = JsonUtility.ToJson(permanent);
        File.WriteAllText(savePath, json);
    }

    void Load()
    {
        if(File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            permanent = JsonUtility.FromJson<PermanentData>(json);
        }
        else
        {
            Debug.Log("ÉtÉ@ÉCÉãÇ™ë∂ç›ÇµÇ»Ç¢");
        }
    }

    public void Rebirth()
    {
        if (isProcessing) return;
        isProcessing = true;
        float Currency = session.currency;
        permanent.rebirthPoints += Currency;
        permanent.rebirthCount++;
        Save();
        session = new SessionData();
        isProcessing = false;
    }
}
