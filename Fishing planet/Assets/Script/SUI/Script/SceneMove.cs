using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneMove : MonoBehaviour
{
    public static SceneMove instance;

    [SerializeField] Text timerText;
    [SerializeField] TMP_Text countText;

    float BaselimitTimer = 10;
    float limitTimer;

    float count = 3f;

    public bool CanPlay { get; private set; }

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        limitTimer = BaselimitTimer * GameManager.instance.AddTimemagni * (RebirthDataHandler.Instance.GetRebirthMultiplier()); 

        CanPlay = false;
        StartCoroutine(StartCount());
    }

    IEnumerator StartCount()
    {
        countText.gameObject.SetActive(true);

        countText.text = ((GameManager.instance.PlayerArea)*100-100)+"m Point";
        yield return new WaitForSeconds(2);

        countText.text = "3";
        yield return new WaitForSeconds(1);

        countText.text = "2";
        yield return new WaitForSeconds(1);

        countText.text = "1";
        yield return new WaitForSeconds(1);

        countText.text = "START!";
        yield return new WaitForSeconds(0.5f);

        countText.gameObject.SetActive(false);

        CanPlay = true;
    }

    void Update()
    {
        if (!CanPlay)
            return;

        limitTimer -= Time.deltaTime;

        if (limitTimer < 0)
        {
            limitTimer = 0;
            SceneManager.LoadScene("Result");
        }

        timerText.text = limitTimer.ToString("F0");
    }
}