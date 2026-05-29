using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public enum SkilltypeEnum
{
    Cooltime1,Cooltime2,Cooltime3,Cooltime4,Cooltime5,

    Pier1,Pier2,Pier3,Pier4,Pier5,

    Fishinghook1,Fishinghook2,Fishinghook3,
    Fishinghook4,Fishinghook5,

    StrengthenStore1,StrengthenStore2,StrengthenStore3,
    StrengthenStore4,StrengthenStore5,

    feed1,feed2,feed3,feed4,feed5,

    Repop1,Repop2,Repop3,Repop4,Repop5,

    Addtime1,Addtime2,Addtime3,Addtime4,Addtime5,
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
        PlayerPrefs.DeleteAll();
        Debug.Log("Awake called");
        //スキル数分の配列を確保
        skills = new bool[System.Enum.GetValues(typeof(SkilltypeEnum)).Length];
        Debug.Log("skills.Length =" + skills.Length);
        for (int i = 0; i < skills.Length; i++)
        {
            SkilltypeEnum type = (SkilltypeEnum)i;
            int s = PlayerPrefs.GetInt("SkillType" + type);
            bool b = Convert.ToBoolean(s);
            skills[i] = b;
        }
        SetText();
    }
    
    //スキルを覚える
    public void LearnSkill(SkilltypeEnum type, int money)
    {
        Debug.Log("LearnSkill called:" + type);
        skills[(int)type] = true;
        Debug.Log("skills[Fishinghook1] = " + skills[(int)SkilltypeEnum.Fishinghook1]);
        SetSkillMoney(money);
        SetText();
        CheckOnOff();
        PlayerPrefs.SetInt("SkillType"+type, 1);
        PlayerPrefs.Save();
    }

    //スキルを覚えているかチェック
    public bool IsSkill(SkilltypeEnum type)
    {
        int index = (int)type;
        if(index < 0 || index >= skills.Length)return false;
        return skills[index];
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
        if (skillMoney < spendMoney)
        {
            return false;
        }

        //キャスト間隔Down２はキャスト間隔Down１を覚えてないとだめ
        else if(type == SkilltypeEnum.Cooltime2)
        {
            return skills[(int)SkilltypeEnum.Cooltime1];
        }
        //キャスト間隔Down3はキャスト間隔Down2を覚えてないとだめ
        else if (type == SkilltypeEnum.Cooltime3)
        {
            return skills[(int)SkilltypeEnum.Cooltime2];
        }
        //店舗強化２は店舗強化１を覚えてないとダメ
        else if (type == SkilltypeEnum.StrengthenStore2)
        {
            return skills[(int)SkilltypeEnum.StrengthenStore1];
        }
        //店舗強化3は店舗強化2を覚えてないとダメ
        else if (type == SkilltypeEnum.StrengthenStore3)
        {
            return skills[(int)SkilltypeEnum.StrengthenStore2];
        }
        //釣り針増加２は釣り針増加１を覚えてないとダメ
        else if (type == SkilltypeEnum.Fishinghook2)
        {
            return skills[(int)SkilltypeEnum.Fishinghook1];
        }
        //釣り針増加3は釣り針増加2を覚えてないとダメ
        else if (type == SkilltypeEnum.Fishinghook3)
        {
            return skills[(int)SkilltypeEnum.Fishinghook2];
        }
        //桟橋1はどれか一列のスキルを覚えていなければダメ
        else if (type == SkilltypeEnum.Pier1)
        {
            bool Cooltime = skills[(int)SkilltypeEnum.Cooltime1]&& skills[(int)SkilltypeEnum.Cooltime2]
                && skills[(int)SkilltypeEnum.Cooltime3];
            bool StrengthenStore = skills[(int)SkilltypeEnum.StrengthenStore1]&& skills[(int)SkilltypeEnum.StrengthenStore2]
                && skills[(int)SkilltypeEnum.StrengthenStore3];
            bool Fishinghook = skills[(int)SkilltypeEnum.Fishinghook1]&& skills[(int)SkilltypeEnum.Fishinghook2]
                && skills[(int)SkilltypeEnum.Fishinghook3];
            return Cooltime || StrengthenStore || Fishinghook;
        }
        //餌１は桟橋１と釣り針を覚えていないとダメ
        else if(type == SkilltypeEnum.feed1)
        {
            bool Fishinghook = skills[(int)SkilltypeEnum.Fishinghook1] && skills[(int)SkilltypeEnum.Fishinghook2]
                && skills[(int)SkilltypeEnum.Fishinghook3];
            return skills[(int)SkilltypeEnum.Pier1]&& Fishinghook;
        }
        //餌2は餌１を覚えていないとダメ
        else if (type == SkilltypeEnum.feed2)
        {
            return skills[(int)SkilltypeEnum.feed1];
        }
        //餌3は餌2を覚えていないとダメ
        else if (type == SkilltypeEnum.feed3)
        {
            return skills[(int)SkilltypeEnum.feed2];
        }
        //再出現１は桟橋１とクールタイムを覚えていないとダメ
        else if (type == SkilltypeEnum.Repop1)
        {
            return skills[(int)SkilltypeEnum.Pier1] && skills[(int)SkilltypeEnum.Cooltime1] && skills[(int)SkilltypeEnum.Cooltime2]
                && skills[(int)SkilltypeEnum.Cooltime3];
        }
        //再出現2は再出現１を覚えていないとダメ
        else if (type == SkilltypeEnum.Repop2)
        {
            return skills[(int)SkilltypeEnum.Repop1];
        }
        //再出現3は再出現2を覚えていないとダメ
        else if (type == SkilltypeEnum.Repop3)
        {
            return skills[(int)SkilltypeEnum.Repop2];
        }
        //時間１は桟橋１と金額増加を覚えていないとダメ
        else if (type == SkilltypeEnum.Addtime1)
        {
            return skills[(int)SkilltypeEnum.Pier1] && skills[(int)SkilltypeEnum.StrengthenStore1] && skills[(int)SkilltypeEnum.StrengthenStore2]
                && skills[(int)SkilltypeEnum.StrengthenStore3];
        }
        //時間2は時間１を覚えていないとダメ
        else if (type == SkilltypeEnum.Addtime2)
        {
            return skills[(int)SkilltypeEnum.Addtime1];
        }
        //時間3は時間2を覚えていないとダメ
        else if (type == SkilltypeEnum.Addtime3)
        {
            return skills[(int)SkilltypeEnum.Addtime2];
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
