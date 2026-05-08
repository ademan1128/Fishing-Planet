using UnityEngine;
using UnityEngine.UI; // UIを使うときに必要
using UnityEngine.SceneManagement;

public class SceneMove : MonoBehaviour
{
    [SerializeField]
    Text TimerText;
    float LimitTimer = 30;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        LimitTimer -= Time.deltaTime;

        if(LimitTimer < 0)
        {
            LimitTimer = 0;

            //シーン遷移
            SceneManager.LoadScene("HARUAKI");
        }
        TimerText.text = LimitTimer.ToString("F0"); // 残り時間を整数で表示
    }
}
