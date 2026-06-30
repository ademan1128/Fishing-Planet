using UnityEngine;

[CreateAssetMenu(fileName = "NewRebirthSkill", menuName = "SkillSystem/RebirthSkillData")]
public class RebirthSkillData : ScriptableObject
{
    public string skillName;             // 画面に表示するスキル名
    [TextArea] public string description; // スキルの説明文
    public float pointCost;               // ★int から float に変更

    // このアセットがどのスキル効果を担当するかをインスペクターで選択
    public RebirthSkillType effectType;

    // enumの名前をベースに、PlayerPrefs用のセーブキーを自動生成するプロパティ
    public string SaveKey => "RebirthSkill_" + effectType.ToString();
}