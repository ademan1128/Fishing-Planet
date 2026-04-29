using UnityEngine;

[System.Serializable]
public class SkillParam
{
    public SkillType skillType;  // スキルの種類
    public int Moneycost;        // 消費ポイント
    public int value;            // 効果量（獲得量とか）
}