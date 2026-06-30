using UnityEngine;
using UnityEngine.UI;

public class RebirthSkillParam : MonoBehaviour
{
    [Header("Skill Data")]
    [SerializeField] private RebirthSkillData targetSkillData;

    [Header("UI Parts")]
    [SerializeField] private Text skillNameText;
    [SerializeField] private Text costText;
    [SerializeField] private Button purchaseButton;

    private void Start()
    {
        if (targetSkillData != null && skillNameText != null)
        {
            skillNameText.text = targetSkillData.skillName;
        }

        if (purchaseButton != null)
        {
            purchaseButton.onClick.AddListener(OnButtonClicked);
        }

        CheckButtonOnOff();
    }

    // RebirthSkillParam.cs の修正が必要な部分のイメージ

    /// <summary>
    /// ボタンの見た目（白か黒か）をチェックする関数
    /// </summary>
    public void CheckButtonOnOff()
    {
        if (RebirthManager.Instance == null || GameManager.instance == null) return;

        // 現在の転生回数に応じた目標金額を計算
        int count = RebirthManager.Instance.permanent.rebirthCount;
        float targetMoney = 10000f * Mathf.Pow(10f, count);

        // ★修正：個別のスキル解放チェック（IsSkillUnlocked）ではなく、
        // 「今のお金が目標金額に達しているか（転生できるか）」でボタンの色を変える
        if (GameManager.instance.PlayerMoney >= targetMoney)
        {
            // ボタンを白にする（クリック可能）
            // ※お使いのコンポーネントに合わせて色やInteractableを制御してください
        }
        else
        {
            // ボタンを黒（またはグレー）にする
        }
    }

    /// <summary>
    /// ボタンが押されたときの処理
    /// </summary>
    private void OnButtonClicked()
    {
        // ★修正：個別スキルの購入ではなく、RebirthManagerの「転生実行関数」をそのまま呼ぶ
        if (RebirthManager.Instance != null)
        {
            RebirthManager.Instance.ExecuteRebirth();
        }
    }

    public void ChangeButtonColor(Color color)
    {
        Image image = gameObject.GetComponent<Image>();
        if (image != null)
        {
            image.color = color;
        }

        if (purchaseButton != null)
        {
            ColorBlock cb = purchaseButton.colors;
            cb.normalColor = Color.white;
            cb.highlightedColor = Color.white;
            cb.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
            cb.selectedColor = Color.white;
            cb.disabledColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            purchaseButton.colors = cb;
        }
    }
}