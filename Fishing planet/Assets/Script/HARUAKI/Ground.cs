using UnityEngine;
using UnityEngine.UIElements;


public class Ground : MonoBehaviour
{
    [SerializeField] private Sprite TetrablockSprite;
    [SerializeField] private Sprite PierSprite;
    [SerializeField] private SpriteRenderer sr;
    GameManager PlayerArea;
    void Start()
    {
        if (GameManager.instance.PlayerArea == 1)
        {
            transform.position = new Vector3(1, -2.5f, 0);
            transform.localScale = new Vector3(1.2f, 1.2f, 1);
            sr.sprite = TetrablockSprite;
        }
        else
        {
            transform.position = new Vector3(-2.5f, -2.25f, 0);
            transform.localScale = new Vector3(1.2f, 1.2f, 1);
            sr.sprite = PierSprite;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
