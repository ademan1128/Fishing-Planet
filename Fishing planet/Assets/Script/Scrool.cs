using UnityEngine;

public class MouseController : MonoBehaviour
{
    // カメラ移動用
    public float scrollSpeed = 10f;

    // クリック判定用（左クリック）
    void Update()
    {
        // 1. 左クリックした瞬間 (押したとき)
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0; // Z軸をリセット
            Debug.Log("左クリックした座標: " + mousePos);
        }

        // 2. 左クリックしている間 (押し続けている間)
        if (Input.GetMouseButton(0))
        {
            // ここに連打やホールド時の処理
        }

        // 3. マウスホイールでのスクロール操作 (上下移動)
        float scrollY = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scrollY) > 0.01f)
        {
            transform.Translate(Vector3.up * scrollY * scrollSpeed * Time.deltaTime);
        }
    }
}

