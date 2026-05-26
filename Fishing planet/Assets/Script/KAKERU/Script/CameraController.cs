using UnityEngine;
using System.Collections;   

public class CameraController : MonoBehaviour
{
    public GameObject ButtonPanel;　// ボタンパネル


    private Vector3 offset; //対象からカメラまでの距離

    void Start()
    {
        //対象からカメラまでの距離を計算
        offset = transform.position - ButtonPanel.transform.position;
    }

    void LateUpdate()
    {
        //対象の位置に距離を足してカメラの位置を決定
        transform.position = ButtonPanel.transform.position + offset;
    }
}
