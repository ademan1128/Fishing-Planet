using TMPro;
using UnityEngine;

public class moneyviewer : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        if (moneyText == null)
        {
            Debug.LogError("Money Text‚ªİ’è‚³‚ê‚Ä‚¢‚Ü‚¹‚ñI");
            return;
        }

        if (GameManager.instance == null)
        {
            Debug.LogError("GManager‚ªŒ©‚Â‚©‚è‚Ü‚¹‚ñI");
            moneyText.text = "MONEY: 0";
            return;
        }

        moneyText.text = "GET MONEY: " + GameManager.instance.PlayerMoney;
    }
}