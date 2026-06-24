using UnityEngine;

public class BackGround : MonoBehaviour
{

    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Sprite Noon;
    [SerializeField] private Sprite Evening;
    [SerializeField] private Sprite Night;
    void Awake()
    {

    }
    void Start()
    {
        if (GameManager.instance.stageTime == GameManager.StageTime.Noon)
        {
            sr.sprite = Noon;
        }
        else if (GameManager.instance.stageTime == GameManager.StageTime.Evening)
        {
            sr.sprite = Evening;
        }
        else if (GameManager.instance.stageTime == GameManager.StageTime.Night)
        {
            sr.sprite = Night;
        }
        else if(GameManager.instance.stageTime == GameManager.StageTime.Rain)
        {
            sr.sprite = Night;
        }
        else if (GameManager.instance.stageTime == GameManager.StageTime.Snow)
        {
            sr.sprite = Noon;
        }
        else if (GameManager.instance.stageTime == GameManager.StageTime.Thunder)
        {
            sr.sprite = Night;
        }
    }
    void Update()
    {

    }
}
