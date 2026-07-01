using UnityEngine;
using UnityEngine.SceneManagement;

public class RebirthOverride : MonoBehaviour
{
    // メインゲームのシーン名を正確に入力してください（もし名前が違う場合は修正してください）
    public string mainGameSceneName = "Main game";

    void Update()
    {
        // もしメインゲームシーンなら、強制的にmultiplierを1にするような処理が必要だが、
        // 既存コードをいじらないなら、ここで「メインゲームの時はRebirthManager.Instanceを無効にする」という制御を行う
        if (SceneManager.GetActiveScene().name == mainGameSceneName)
        {
            if (RebirthManager.Instance != null)
            {
                // ここでRebirthManagerが持っている計算結果を無視させるためのフラグ管理を行う
                // もしくは、今のエラーを止めるためだけに「メインゲームならInstanceをnullにする」
                RebirthManager.Instance = null;
            }
        }
    }
}