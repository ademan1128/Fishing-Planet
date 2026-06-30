using TMPro;
using UnityEngine;

public class EffctMoney : MonoBehaviour
{
    [SerializeField] TMP_Text priceText;

    [SerializeField] float moveSpeed = 50f;
    [SerializeField] float lifeTime = 1.5f;

    float timer;
    Color color;
    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        color = priceText.color;
    }

    public void SetPrice(float price)
    {
        priceText.text = $"<sprite=0> {price} G";
    }

    void Update()
    {
        // 上に移動
        rect.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;

        // 時間
        timer += Time.deltaTime;

        // フェード
        color.a = Mathf.Lerp(1f, 0f, timer / lifeTime);
        priceText.color = color;
    }

    public bool IsFinished()
    {
        return timer >= lifeTime;
    }
}