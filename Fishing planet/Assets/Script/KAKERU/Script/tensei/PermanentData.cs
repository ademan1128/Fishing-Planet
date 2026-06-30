using System;
using System.Collections.Generic;

[System.Serializable]
public class PermanentData
{
    public float rebirthPoints; // 転生ポイント
    public int rebirthCount;    // 転生回数
    public List<int> unlockedSkills; // ★初期化式はここに書かない

    // 引数なしのコンストラクタを明示的に用意する
    public PermanentData()
    {
        rebirthPoints = 0f;
        rebirthCount = 0;
        unlockedSkills = new List<int>(); // 確実にここでインスタンスを生成する
    }
}