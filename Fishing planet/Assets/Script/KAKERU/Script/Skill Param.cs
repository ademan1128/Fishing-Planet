using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SkillParam : MonoBehaviour {
    //スキル管理システム
    [SerializeField]
    private SkillSystem skillSystem;
    //このスキルの種類
    [SerializeField]
    private SkillData skill;
    //このスキルを覚えるために必用な金額
    [SerializeField]
    private int spendMoney;
    //スキルのタイトル
    [SerializeField]
    private string skillTitle;
    //スキル情報
    [SerializeField]
    private string skillInformation;
    //スキル情報を載せるテキストUI
    [SerializeField]
    private Text text;
    //初期化にはこれを使用
    void  Start()
    {
       Invoke("DelayCheck",0.5f);
    }

    void DelayCheck()
    {
        CheckButtonOnOff();
    }

    //　スキルボタンを押した時に実行するメソッド
    public void OnClick()
    {
        // GetSkillMoneyの行は削除
        Debug.Log("CanLearn = " + skillSystem.CanLearnSkill(skill));
        Debug.Log("押された");

        if (skillSystem.IsSkill(skill)) return;

        if (skillSystem.CanLearnSkill(skill))
        {
            skillSystem.LearnSkill(skill);  // spendMoneyを消す
            CheckButtonOnOff();
            text.text = skillTitle + "を覚えた";
        }
        else
        {
            text.text = "スキルを覚えられません。";
        }
    }

    //　他のスキルを習得した後の自身のボタンの処理
    public void CheckButtonOnOff()
    {
        Debug.Log(skill + "CheckButtonOnOff called");
        Debug.Log($"{skill.name} - IsSkill:{skillSystem.IsSkill(skill)}, CanLearn:{skillSystem.CanLearnSkill(skill)}");
        //　スキルを覚えられるかどうかチェック
        if (skillSystem.IsSkill(skill))
        {
            //取得済み→青色
            ChangeButtonColor(new Color(0.0f, 0.0f, 1.0f, 0.8f));
        }
        else if (!skillSystem.CanLearnSkill(skill))
        {
            //取得不可→灰色
            ChangeButtonColor(new Color(0.0f, 0.0f, 0.0f, 0.8f));
        }
        else
        {
            ChangeButtonColor(new Color(1.0f, 1.0f, 1.0f, 1.0f));
        }
    }
    //　スキル情報を表示
    public void SetText()
    {
        text.text = skillTitle + "：消費スキルポイント" + spendMoney + "\n" + skillInformation;
    }
    //　スキル情報をリセット
    public void ResetText()
    {
        text.text = "";
    }
    //　ボタンの色を変更する
    public void ChangeButtonColor(Color color)
    {
        // Imageコンポーネントを直接変更（ステートに関係なく即反映）
        Image image = gameObject.GetComponent<Image>();
        image.color = color;

        // ColorBlockも合わせて更新（ホバー時などにズレないように）
        Button button = gameObject.GetComponent<Button>();
        ColorBlock cb = button.colors;
        cb.normalColor = Color.white;       // Imageの色と掛け算されるので白にする
        cb.highlightedColor = Color.white;
        cb.pressedColor = new Color(0.8f, 0.8f, 0.8f, 1f);
        cb.selectedColor = Color.white;
        cb.disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        button.colors = cb;
    }
}
