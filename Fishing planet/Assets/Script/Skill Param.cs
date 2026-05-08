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
    private SkilltypeEnum type;
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
        //スキルを覚えられる状態で無ければボタンを無効化
        CheckButtonOnOff();
    }

    //　スキルボタンを押した時に実行するメソッド
    public void OnClick()
    {
        Debug.Log("押された");
        //　スキルを覚えていたら何もせずreturn
        if (skillSystem.IsSkill(type))
        {
            return;
        }
        //　スキルを覚えられるかどうかチェック
        if (skillSystem.CanLearnSkill(type, spendMoney))
        {
            //　スキルを覚えさせる
            skillSystem.LearnSkill(type, spendMoney);

            ChangeButtonColor(new Color(0f, 0f, 1f, 1f));

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
        //　スキルを覚えられるかどうかチェック
        if (!skillSystem.CanLearnSkill(type))
        {
            ChangeButtonColor(new Color(0.8f, 0.8f, 0.8f, 0.8f));
            //　スキルをまだ覚えていない
        }
        else if (!skillSystem.IsSkill(type))
        {
            ChangeButtonColor(new Color(1f, 1f, 1f, 1f));
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
        //　ボタンコンポーネントを取得
        Button button = gameObject.GetComponent<Button>();
        //　ボタンのカラー情報を取得（一時変数を作成し、色情報を変えてからそれをbutton.colorsに設定しないとエラーになる）
        ColorBlock cb = button.colors;
        //　取得済みのスキルボタンの色を変える
        cb.normalColor = color;
        cb.pressedColor = color;
        //　ボタンのカラー情報を設定
        button.colors = cb;
    }
}
