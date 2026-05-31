using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    public int cost;
    // 全部必要な前提スキル（AND条件）
    public SkillData[] required;
    // どれか一つのグループが全部揃えばOK（OR条件）
    public SkillGroup[] requiredGroups;
}

[System.Serializable]
public class SkillGroup
{
    public SkillData[] skills;
}
