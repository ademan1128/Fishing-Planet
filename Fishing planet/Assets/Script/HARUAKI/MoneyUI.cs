using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    public void UpdateMoney(int money)
    {
        moneyText.text = "MONEY:" + GameManager.instance.PlayerMoney;
    }
}
