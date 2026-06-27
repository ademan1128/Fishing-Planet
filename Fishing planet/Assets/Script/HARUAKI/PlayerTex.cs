using UnityEngine;
using static FishMove;

public class PlayerTex : MonoBehaviour
{
    [SerializeField] private Sprite idle;
    [SerializeField] private Sprite throwing;
    [SerializeField] private Sprite biting;
    [SerializeField] private Sprite reeling;
    [SerializeField] private Sprite getting;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField]private Fishing Fishing;
    public enum PlayerState
    {
        Idle,
        Throwing,
        Biting,
        Reeling,
        Getting
    }

    public PlayerState State = PlayerState.Idle;
    void Start()
    {

    }

    void Update()
    {
        switch (State)
        {
            case PlayerState.Idle:
                Idle();
                break;

            case PlayerState.Throwing:
                Throwing();
                break;

            case PlayerState.Biting:
                Biting();
                break;

            case PlayerState.Reeling:
                Reeling();
                break;
            case PlayerState.Getting:
                Getting();
                break;
        }
    }

    void Idle()
    {
        sr.sprite = idle;
        if(Fishing.Throwing == true)
        {
            State = PlayerState.Throwing;
        }

    }
    void Throwing()
    {
        sr.sprite = throwing;
        if (Fishing.Throwing == false)
        {
            State = PlayerState.Biting;
        }
    }

    void Biting()
    {
        sr.sprite = biting;
        if (Fishing.isReeling)
        {
            State = PlayerState.Reeling;
        }
    }
    void Reeling()
    {
        sr.sprite = reeling;
        if (Fishing.Underwater == false)
        {
            State = PlayerState.Getting;
        }
    }

    void Getting()
    {
        sr.sprite = getting;
        if (Fishing.isReeling == false)
        {
            State = PlayerState.Idle;
        }
    }
}
