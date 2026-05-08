using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Å©í«â¡

public class SceneMove : MonoBehaviour
{
    [SerializeField]
    Text timerText;

    float limitTimer = 30;

    void Update()
    {
        limitTimer -= Time.deltaTime;

        if (limitTimer < 0)
        {
            limitTimer = 0;

            // ÉVÅ[ÉìëJà⁄
            SceneManager.LoadScene("HARUAKI");
        }

        timerText.text = limitTimer.ToString("F0");
    }
}