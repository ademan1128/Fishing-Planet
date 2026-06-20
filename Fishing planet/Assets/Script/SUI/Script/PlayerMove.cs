using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private Sprite[] fishermanSprites; // 0～4を設定
    [SerializeField] private SpriteRenderer spriteRenderer;

    private int state = 0;

    void Start()
    {
        spriteRenderer.sprite = fishermanSprites[0];
    }

    void Update()
    {
        // 左クリック
        if (Input.GetMouseButtonDown(0))
        {
            if (state == 0)
            {
                StartCoroutine(LeftAction());
            }
        }

        // 右クリック
        if (Input.GetMouseButtonDown(1))
        {
            if (state == 2)
            {
                StartCoroutine(RightAction());
            }
        }
    }

    private System.Collections.IEnumerator LeftAction()
    {
        state = 1;

        spriteRenderer.sprite = fishermanSprites[1];
        yield return new WaitForSeconds(0.2f);

        spriteRenderer.sprite = fishermanSprites[2];
        state = 2; // ここで停止
    }

    private System.Collections.IEnumerator RightAction()
    {
        state = 3;

        spriteRenderer.sprite = fishermanSprites[3];
        yield return new WaitForSeconds(0.2f);

        spriteRenderer.sprite = fishermanSprites[4];
        yield return new WaitForSeconds(0.2f);

        spriteRenderer.sprite = fishermanSprites[0];
        state = 0; // 初期状態へ戻る
    }
}