using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum SkilltypeEnum
{
    Cooltime1,
    Cooltime2,
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
    
    //スキルを覚える
    public void LearnSkill(SkilltypeEnum type, int money)
    {
        skills[(int)type] = true;
        SetSkillMoney(money);
        SetText();
        //CheckOnOff();
    }

    //スキルを覚えているかチェック
    public bool IsSkill(SkilltypeEnum type)
    {
        return skills[(int)type];
    }

    //金額を減らす
    public void SetSkillMoney(int money)
    {
        skillMoney -= money;
    }

    //お金を取得
    public int GetSkillMoney()
    {
        return skillMoney;
    }

    //スキルを覚えられるかチェック
    public bool CanLearnSkill(SkilltypeEnum type,int spendMoney = 0)
    {
        //持ってる金額が足りない
        if (skillMoney < 0)
        {
            return false;
        }

        //キャスト間隔Down２はキャスト間隔Down１を覚えてないとだめ
        if(type == SkilltypeEnum.Cooltime2)
        {
            return skills[(int)SkilltypeEnum.Cooltime1];
        }
        //桟橋増加２は桟橋増加１を覚えてないとダメ
        else if (type == SkilltypeEnum.Pier2)
        {
            return skills[(int)SkilltypeEnum.Pier1];
        }
        //釣り針増加２は釣り針増加１を覚えてないとダメ
        else if (type == SkilltypeEnum.Fishinghook2)
        {
            return skills[(int)SkilltypeEnum.Fishinghook1];
        }
        //店舗強化は全てのスキルを覚えていなければダメ
        else if (type == SkilltypeEnum.StrengthenStore)
        {
            return skills[(int)SkilltypeEnum.Cooltime1]&& skills[(int)SkilltypeEnum.Cooltime2] 
                && skills[(int)SkilltypeEnum.Pier1] && skills[(int)SkilltypeEnum.Pier2] && skills[(int)SkilltypeEnum.Fishinghook1]
                && skills[(int)SkilltypeEnum.Fishinghook2];
        }
        return true;
    }

    //スキル毎にボタンのオン・オフをする処理を実行させる

    void CheckOnOff()
    {
        foreach (var skillParam in skillParams)
        {
            skillParam.CheckButtonOnOff();
        }
    }

    void SetText()
    {
        MoneyText.text = "金額：" + skillMoney;
    }
}
