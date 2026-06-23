using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;
    void Start()
    {
        UpdateMoney(GameManager.instance.PlayerMoney);
    }
    public void UpdateMoney(int money)
    {
        moneyText.text = "MONEY:" + money;
    }
}
