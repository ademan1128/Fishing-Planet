using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    // シーンをまたいで保持する習得済みスキルセット
    private HashSet<SkillData> learnedSkills = new HashSet<SkillData>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public bool IsLearned(SkillData skill)
    {
        return learnedSkills.Contains(skill);
    }

    public void AddSkill(SkillData skill)
    {
        learnedSkills.Add(skill);
    }

    public HashSet<SkillData> GetLearnedSkills()
    {
        return learnedSkills;
    }
}