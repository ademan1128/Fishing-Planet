using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using System;





public class SkillSystem : MonoBehaviour
{
    [SerializeField] private int skillMoney;
    public Text MoneyText;
    [SerializeField] private SkillParam[] skillParams;
    [SerializeField] private SkillData[] allSkills;
    [SerializeField] private int gachaCost;

    // 習得済みスキルのセット
    private HashSet<SkillData> learnedSkills = new HashSet<SkillData>();

    // スキルを覚える
    public void LearnSkill(SkillData skill)
    {
        if (!CanLearnSkill(skill)) return;

        learnedSkills.Add(skill);
        skillMoney -= skill.cost;
        SetText();
        CheckOnOff();
    }
   
    public SkillData DrawGacha()
    {
        if(skillMoney < gachaCost) return null;

        //未習得スキルを対象にする
        List<SkillData> candidates = new List<SkillData>();
        foreach (var skill in allSkills)
        {
            if(!IsSkill(skill)) candidates.Add(skill);
        }

        if(candidates.Count == 0) return null;

        //完全ランダムで１つ選ぶ
        int index = UnityEngine.Random.Range(0,candidates.Count);
        SkillData result = candidates[index];

        learnedSkills.Add(result);
        skillMoney -= gachaCost;
        SetText();
        CheckOnOff();
        return result;
    }

    // 習得済みかチェック
    public bool IsSkill(SkillData skill)
    {
        return learnedSkills.Contains(skill);
    }

    public bool CanLearnSkill(SkillData skill)
    {
        if (skill == null) return false;
        if (skillMoney < skill.cost) return false;

        // AND条件：全部習得済みかチェック
        foreach (var req in skill.required)
        {
            if (!learnedSkills.Contains(req)) return false;
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
                    if (!learnedSkills.Contains(req))
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
        MoneyText.text = "金額：" + skillMoney;
    }

    public int GetSkillMoney()
    {
        return skillMoney;
    }

    public int GetGachaCost()
    {
        return gachaCost;
    }

    void CheckOnOff()
    {
        foreach (var skillParam in skillParams)
        {
            skillParam.CheckButtonOnOff();
        }
    }

    public SkillData DrawGachaFromList(SkillData[] targetSkills)
    {
        if (skillMoney < gachaCost) return null;

        List<SkillData> candidates = new List<SkillData>();
        foreach (var skill in targetSkills)
        {
            if (!IsSkill(skill)) candidates.Add(skill);
        }

        if (candidates.Count == 0) return null;

        int index = UnityEngine.Random.Range(0, candidates.Count);
        SkillData result = candidates[index];
        learnedSkills.Add(result);
        skillMoney -= gachaCost;
        SetText();
        CheckOnOff();
        return result;
    }
}
