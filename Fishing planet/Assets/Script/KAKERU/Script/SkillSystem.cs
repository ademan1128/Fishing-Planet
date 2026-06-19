using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SkillSystem : MonoBehaviour
{
    // ★削除: [SerializeField] private int skillMoney; // 相手のMoneyを使うため廃止
    public Text MoneyText;
    [SerializeField] private SkillParam[] skillParams;
    [SerializeField] private SkillData[] allSkills;
    [SerializeField] private int gachaCost;

    // 変更前：GameManager.Instance.PlayerMoney を見に行っていた処理
    // 変更後：相手のコードにInstanceができるまでの「臨時の代役」にする
    private int CurrentMoney
    {
        get
        {
            // FindObjectOfType を使うことで、Instance(窓口)がなくても
            // シーン上から強引に GameManager を見つけ出すことができます
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                return gm.PlayerMoney; // ★「PlayerMoney」の変数名が合っているかだけ確認してください
            }
            return 0;
        }
        set
        {
            GameManager gm = FindObjectOfType<GameManager>();
            if (gm != null)
            {
                gm.PlayerMoney = value; // ★ここも同様です
            }
        }
    }

    private void Start()
    {
        if (SkillManager.Instance == null)
        {
            Debug.LogError("SkillManagerがsceneに存在しません");
        }
        SetText();
        CheckOnOff();
    }

    // スキルを覚える
    public void LearnSkill(SkillData skill)
    {
        if (!CanLearnSkill(skill)) return;

        // ★安全策: 明示的にSkillManagerに保存する
        SkillManager.Instance.AddSkill(skill);

        // ★変更: 相手のお金を減らす
        CurrentMoney -= skill.cost;

        SetText();
        CheckOnOff();
    }

    public SkillData DrawGacha()
    {
        // ★変更: 相手のお金でチェック
        if (CurrentMoney < gachaCost) return null;

        List<SkillData> candidates = new List<SkillData>();
        foreach (var skill in allSkills)
        {
            if (!IsSkill(skill)) candidates.Add(skill);
        }

        if (candidates.Count == 0) return null;

        int index = UnityEngine.Random.Range(0, candidates.Count);
        SkillData result = candidates[index];

        SkillManager.Instance.AddSkill(result);

        // ★変更: 相手のお金を減らす
        CurrentMoney -= gachaCost;

        SetText();
        CheckOnOff();
        return result;
    }

    // 習得済みかチェック
    public bool IsSkill(SkillData skill)
    {
        return SkillManager.Instance.IsLearned(skill);
    }

    public bool CanLearnSkill(SkillData skill)
    {
        if (skill == null) return false;

        // ★変更: 相手のお金でチェック
        if (CurrentMoney < skill.cost) return false;

        // AND条件：全部習得済みかチェック
        foreach (var req in skill.required)
        {
            if (!IsSkill(req)) return false;
        }

        // OR条件：どれか一つのグループが全部揃っているかチェック
        if (skill.requiredGroups.Length > 0)
        {
            bool anyGroupCleared = false;
            foreach (var group in skill.requiredGroups)
            {
                bool groupCleared = true;
                foreach (var req in group.skills)
                {
                    if (!IsSkill(req))
                    {
                        groupCleared = false;
                        break;
                    }
                }
                if (groupCleared) anyGroupCleared = true;
            }
            if (!anyGroupCleared) return false;
        }

        return true;
    }

    void SetText()
    {
        // ★変更: 相手のお金を表示
        MoneyText.text = "金額：" + CurrentMoney;
    }

    public int GetSkillMoney() => CurrentMoney;
    public int GetGachaCost() => gachaCost;

    void CheckOnOff()
    {
        foreach (var skillParam in skillParams)
        {
            skillParam.CheckButtonOnOff();
        }
    }

    public SkillData DrawGachaFromList(SkillData[] targetSkills)
    {
        // ★変更: 相手のお金でチェック
        if (CurrentMoney < gachaCost) return null;

        List<SkillData> candidates = new List<SkillData>();
        foreach (var skill in targetSkills)
        {
            if (!IsSkill(skill)) candidates.Add(skill);
        }

        if (candidates.Count == 0) return null;

        int index = UnityEngine.Random.Range(0, candidates.Count);
        SkillData result = candidates[index];

        SkillManager.Instance.AddSkill(result);

        // ★変更: 相手のお金を減らす
        CurrentMoney -= gachaCost;

        SetText();
        CheckOnOff();
        return result;
    }

    public bool CanRebirth()
    {
        return CurrentMoney >= rebirthCost;
    }
}