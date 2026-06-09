using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/SkillData")]
public class SkillData : ScriptableObject
{
    public int cost;

    // ★【追加】このスキルがどの能力に影響を与えるか
    public SkillEffectType effectType;

    // ★【追加】このスキルの効果量（例: 1.5 なら「1.5倍」または「+50%」）
    public float effectValue = 1.0f;
    
    // 全部必要な前提スキル（AND条件）
    public SkillData[] required;
    // どれか一つのグループが全部揃えばOK（OR条件）
    public SkillGroup[] requiredGroups;

    private void OnEnable()
    {
        if (required == null) required = new SkillData[0];
        if (requiredGroups == null) requiredGroups = new SkillGroup[0];
    }
}

[System.Serializable]
public class SkillGroup
{
    public SkillData[] skills;

    public SkillGroup()
    {
        skills = new SkillData[0];
    }
}
