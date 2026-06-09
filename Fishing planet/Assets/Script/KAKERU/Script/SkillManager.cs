using UnityEngine;
using System.Collections.Generic;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance { get; private set; }

    [SerializeField] private List<SkillData> allSkills = new List<SkillData>();
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

        // ※「GameScene」の部分は、実際のゲーム画面のシーン名に書き換えてください
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main game");
    }

    public bool IsLearned(SkillData skill)
    {
        return learnedSkills.Contains(skill);
    }

    public void AddSkill(SkillData skill)
    {
        if (skill == null) return;
        learnedSkills.Add(skill);

        // ★修正: エラー回避のため、Unityオブジェクトのデフォルトの「.name」を使用します
        Debug.Log($"SkillManager: スキル「{skill.name}」の習得を記録しました。");
    }

    public HashSet<SkillData> GetLearnedSkills()
    {
        return learnedSkills;
    }

    // ★重要: あなたのSkillDataに合わせて変数名を書き換える必要があります
    public float GetTotalMultiplier(SkillEffectType type)
    {
        float multiplier = 1.0f;
        foreach (var skill in learnedSkills)
        {
            if (skill != null)
            {
                // ─── 【重要：もしエラーが消えない場合】 ───
                // あなたの「SkillData.cs」を開いて、効果の種類（enum）が入っている変数名と、
                // 効果量（floatやint）が入っている変数名を確認してください。
                // もし変数名が「Type」や「Value」なら、以下のように書き換えます。
                // 
                // if (skill.Type == type) { multiplier *= skill.Value; }
                // ─────────────────────────────────

                // 一旦、仮で前回の命名に合わせておきます
                if (skill.effectType == type)
                {
                    multiplier *= skill.effectValue;
                }
            }
        }
        return multiplier;
    }
}

public enum SkillEffectType
{
    None,
    StrengthenStores,
    AddTime,
    FishingHook,
    Feed,
    Pier,
    Repop
}