using UnityEngine;
using UnityEngine.UI;

public class GachaButton : MonoBehaviour
{
    [SerializeField] private SkillSystem skillSystem;
    [SerializeField] private Text text;
    [SerializeField] 
    private string skillInformation;

    //ボタンのOnclick()に登録
    public void OnClick()
    {
        SkillData result = skillSystem.DrawGacha();
        if(result == null)
        {
            text.text = "スキルを覚えられません";
        }
        else
        {
            
             text.text = result.name + "を覚えた";
        }
    }

    public void SetText()
    {
        text.text = ":消費スキルポイント" + skillSystem.GetGachaCost() + "\n" + skillInformation;
    }

    public void ReseteText()
    {
        text.text = "";
    }
}
