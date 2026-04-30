using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum SkilltypeEnum
{
    Cooltime1,
    Colltime2,
    Pier1,
    Pier2,
    Fishinghook1,
    Fishinghook2,
    StrengthenStore
};


public class SkillSystem : MonoBehaviour
{
    //スキルを覚えるための金額
    [SerializeField] private int skillMoney;
    //スキルを覚えているかどうかのフラグ
    [SerializeField] private bool[] skills;
    //スキルごとのパラメータ
    [SerializeField] private SkillParam[] skillParams;
    //金額を表示するテキストUI
    public Text MoneyText;
    private void Awake()
    {
        //スキル数分の配列を確保
        skills = new bool[skillParams.Length];
        SetText();
    }

    void SetText()
    {
        MoneyText.text = skillMoney.ToString();
    }
    
    //スキルを覚える
    public void LearnSkill(SkilltypeEnum type, int money)
    {
        skills[(int)type] = true;
        SetSkillMoney(money);
        SetText();
        CheckOnOff();
    }

    //金額を減らす
    public void SetSkillMoney(int money)
    {
        skillMoney -= money;
    }

    //スキル毎にボタンのオン・オフをする処理を実行させる
    void CheckOnOff()
    {
        foreach (var skillParam in skillParams)
        {
            //SkillParam.CheckButtonOnOff();
        }
    }
}
