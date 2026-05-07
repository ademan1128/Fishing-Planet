using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SkillSystemOnOff : MonoBehaviour
{
    [SerializeField]
    public GameObject skillSystem;
    //最初にフォーカスするゲームオブジェクト
    [SerializeField]
    public GameObject firstSelect;

    //更新は１フレームごとに呼び出される
    private void Update()
    {
        if ((Input.GetKeyDown("s")))
        {
            skillSystem.SetActive(!skillSystem.activeSelf);
            EventSystem.current.SetSelectedGameObject(firstSelect);
        }
    }
}
