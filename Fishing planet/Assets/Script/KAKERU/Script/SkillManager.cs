using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーンがロードされたときに、必要に応じてスキルの状態を更新する処理をここに追加できます
        // 例: UIの更新、スキル効果の適用など
        if(scene.name == "Main game")
        {
            // ここでスキルの状態をUIに反映させるなどの処理を行うことができます
            Debug.Log("Main gameシーンがロードされました。習得済みスキルの数: " + learnedSkills.Count);
            Instance = this;
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Main game");
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

    public float GetTotalMultiplier(SkillEffectType type)
    {
        float totalBonus = 0.0f; // ★0からスタート
        foreach (var skill in learnedSkills)
        {
            if (skill != null && skill.effectType == type)
            {
                // 1.02のような「増分」を足し合わせる
                // もしeffectValueが1.02なら、0.02分を足すという考え方にするのが安全です
                totalBonus += (skill.effectValue - 1.0f);
            }
        }
        return 1.0f + totalBonus; // 最後に1を足して「倍率」にする
    }

    public void ClearAllSkills()
    {
        learnedSkills.Clear();
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