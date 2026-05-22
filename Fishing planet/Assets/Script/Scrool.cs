using UnityEngine;
using UnityEngine.EventSystems;

public class DragScrollWithLimit : MonoBehaviour
{
    public float scrollSpeed = 1.5f;
    public bool reverse = false;
    public float minX = -10f;
    public float maxX = 10f;

    private bool isDragging = false; // ドラッグ可能かどうかのフラグ

    void Update()
    {
        // 左クリックした瞬間
        if (Input.GetMouseButtonDown(0))
        {
            // UIの上でなければドラッグ開始を許可
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                isDragging = true;
            }
        }

        // 左クリックを離したらドラッグ終了
        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        // ドラッグ許可状態のときだけ動かす
        if (isDragging && Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            float direction = reverse ? -1f : 1f;
            float move = mouseX * scrollSpeed * direction;

            float targetX = transform.position.x + move;
            targetX = Mathf.Clamp(targetX, minX, maxX);

            transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        }
    }
}
