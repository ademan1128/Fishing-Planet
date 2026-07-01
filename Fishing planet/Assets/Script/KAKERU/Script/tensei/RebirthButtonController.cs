using UnityEngine;
using UnityEngine.UI;

public class RebirthButtonController : MonoBehaviour
{
    public Button rebirthButton;
    public Image buttonImage;

    [Header("色設定")]
    public Color baseColor = new Color(0.8f, 0.8f, 0.8f, 1f); // 薄灰色
    public Color highlightColor = Color.white;              // 点滅時の色

    private void Awake() // Start ではなく Awake で取得
    {
        if (rebirthButton == null) rebirthButton = GetComponent<Button>();
        if (buttonImage == null) buttonImage = GetComponent<Image>();
    }

    // 重要なポイント：Startで初期色を適用する
    private void Start()
    {
        if (buttonImage != null)
        {
            buttonImage.color = baseColor;
        }
    }

    private void Update()
    {
        if (rebirthButton == null) Debug.LogError("rebirthButtonのスロットが空です！");
        else Debug.Log("操作対象のボタン名: " + rebirthButton.name);
        // GameManagerやDataHandlerがないなら何もしない
        if (GameManager.instance == null || RebirthDataHandler.Instance == null) return;

        float targetMoney = 10000f;
        float currentMoney = GameManager.instance.PlayerMoney;

        if (currentMoney >= targetMoney)
        {
            float alpha = Mathf.PingPong(Time.time * 2f, 1f);
            // ここでLerpすることで、ベース色から点滅色が計算される
            buttonImage.color = Color.Lerp(baseColor, highlightColor, alpha);
            rebirthButton.interactable = true;
        }
        else
        {
            // 1万円未満：初期の薄灰色を強制的に適用
            buttonImage.color = baseColor;
            rebirthButton.interactable = false;
        }
    }

    public void OnClickRebirth()
    {
        if (RebirthDataHandler.Instance != null)
        {
            RebirthDataHandler.Instance.ExecuteRebirth();
            RefreshAllSkillButtons();
        }
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