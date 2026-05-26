using UnityEngine;

public class SkillManager : MonoBehaviour
{
    //スキルを覚えているかどうかのフラグ(保持したいデータ）
    [SerializeField] private bool[] skills;

    //シングルトンのインスタンス
    public static SkillManager Instance { get; private set; }

    private void Awake()
    {
        //すでにインスタンスが存在する場合は自信を破棄し、重複を防ぐ
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //シーン遷移時にこのオブジェクトを破壊しない
        DontDestroyOnLoad(gameObject);
    }

    //他のスクリプトからスキル配列を取得・変更するためのプロパティ
    public bool[] Skills => skills;
}
