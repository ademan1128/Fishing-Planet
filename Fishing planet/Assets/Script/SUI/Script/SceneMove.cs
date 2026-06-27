using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Å©í«â¡

public class SceneMove : MonoBehaviour
{
    [SerializeField]
    Text timerText;

    float BaselimitTimer = 10;
    float limitTimer;

    void Start()
    {
        limitTimer = BaselimitTimer * GameManager.instance.AddTimemagni;
    }

    void Update()
    {
        limitTimer -= Time.deltaTime;

        if (limitTimer < 0)
        {
            limitTimer = 0;

            // ÉVÅ[ÉìëJà⁄
            SceneManager.LoadScene("Result");
        }

        timerText.text = limitTimer.ToString("F0");
    }
}