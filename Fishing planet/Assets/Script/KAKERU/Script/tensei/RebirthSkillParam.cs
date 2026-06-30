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

    public void CheckButtonOnOff()
    {
        if (targetSkillData == null || RebirthManager.Instance == null) return;

        bool isUnlocked = RebirthManager.Instance.IsSkillUnlocked(targetSkillData.effectType);

        if (isUnlocked)
        {
            if (costText != null) costText.text = "解放済み";
            if (purchaseButton != null) purchaseButton.interactable = false;

            // 取得済み ➔ 青色
            ChangeButtonColor(new Color(0.0f, 0.0f, 1.0f, 0.8f));
        }
        else
        {
            // ★表示する際は「10 Pt」のように表示されます（必要なら .ToString("F0") で整数風に見せることも可）
            if (costText != null) costText.text = targetSkillData.pointCost + " Pt";

            // ★float同士の安全な比較
            bool canAfford = RebirthManager.Instance.permanent.rebirthPoints >= targetSkillData.pointCost;

            if (purchaseButton != null)
            {
                purchaseButton.interactable = canAfford;
            }

            if (canAfford)
            {
                // 購入可能 ➔ 白色
                ChangeButtonColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
            }
            else
            {
                // ポイント不足 ➔ 灰色・黒色
                ChangeButtonColor(new Color(0.0f, 0.0f, 0.0f, 1f));
            }
        }
    }

    private void OnButtonClicked()
    {
        if (RebirthManager.Instance != null && targetSkillData != null)
        {
            RebirthManager.Instance.PurchaseRebirthSkill(targetSkillData);
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