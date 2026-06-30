using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [SerializeField] private Sprite ê‡ñæ1;
    [SerializeField] private Sprite ê‡ñæ2;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] Fishing fishing;
    void Start()
    {
        if (GameManager.instance.PlayerArea != 1)
        {
            gameObject.SetActive(false);
            return;
        }
        sr.sprite = ê‡ñæ1;

    }
    void Update()
    {
        if (fishing.isMove == true)
        {
            sr.sprite = ê‡ñæ2;
        } else
        {
            sr.sprite = ê‡ñæ1;
        }
    }
}
