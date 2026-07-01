using UnityEngine;

public class Tutorialpop : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] SceneMove CanPlay;

    void Start()
    {
        if (GameManager.instance.PlayerArea != 1)
        {
            gameObject.SetActive(false);
            return;
        }
        sr.enabled = false;
    }

    void Update()
    {
        if (CanPlay.CanPlay == true)
        {
            sr.enabled = true;
        }
    }
}
