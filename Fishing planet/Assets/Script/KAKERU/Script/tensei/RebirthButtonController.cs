using UnityEngine;
using UnityEngine.UI;

public class RebirthButtonController : MonoBehaviour
{
    private Button rebirthButton;

    private void Awake()
    {
        // 自身のアタッチされているButtonコンポーネントを取得
        rebirthButton = GetComponent<Button>();

        if (rebirthButton != null)
        {
            // ボタンがクリックされたときにRebirthManagerの転生処理を呼ぶ
            rebirthButton.onClick.AddListener(OnRebirthButtonClicked);
        }
    }

    private void Start()
    {
        UpdateButtonInteractable();
    }

    /// <summary>
    /// 条件（目標金額）を満たしているかチェックし、ボタンを押せるか決める
    /// </summary>
    public void UpdateButtonInteractable()
    {
        if (rebirthButton == null) return;
        if (RebirthManager.Instance == null || GameManager.instance == null) return;

        // 現在の転生回数から必要な目標金額を計算
        int count = RebirthManager.Instance.permanent.rebirthCount;
        float targetMoney = 10000f * Mathf.Pow(10f, count);
        float currentMoney = GameManager.instance.PlayerMoney;

        // 目標金額に達していればボタンを活性化（白く押せる状態）、達していなければ非活性（グレー）
        rebirthButton.interactable = (currentMoney >= targetMoney);
    }

    private void OnRebirthButtonClicked()
    {
        if (RebirthManager.Instance != null)
        {
            Debug.Log("右下の転生ボタンが押されました。転生を実行します。");
            RebirthManager.Instance.ExecuteRebirth();

            // 転生直後にボタンの状態を再更新（お金がリセットされるので押せなくなるはず）
            UpdateButtonInteractable();
        }
    }
}