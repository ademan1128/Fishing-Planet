using UnityEngine;
using UnityEngine.EventSystems;

public class DragScrollClean : MonoBehaviour
{
    [Header("設定")]
    public float scrollSpeed = 0.5f; // 最初は小さめの値（0.5など）を推奨
    public bool reverse = false;

    [Header("制限範囲")]
    public float minX = -10f;
    public float maxX = 10f;

    private bool isDragging = false;

    void Update()
    {
        // 1. クリックした瞬間
        if (Input.GetMouseButtonDown(0))
        {
            // UIの上でなければドラッグ開始
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isDragging = true;
            }
        }

        // 2. 指を離したら終了
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // 3. ドラッグ中の処理
        if (isDragging && Input.GetMouseButton(0))
        {
            // Input.GetAxis は「前フレームからの移動量」のみを返すのでワープしにくい
            float mouseX = Input.GetAxis("Mouse X");

            // マウスが動いている時だけ処理
            if (Mathf.Abs(mouseX) > 0)
            {
                float direction = reverse ? -1f : 1f;
                // 移動量を計算
                float move = mouseX * scrollSpeed * direction;

                // 現在の座標に加算して制限
                float targetX = transform.position.x + move;
                targetX = Mathf.Clamp(targetX, minX, maxX);

                transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
            }
        }
    }
}
