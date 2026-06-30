using UnityEngine;

public class LineTop : MonoBehaviour
{
    [SerializeField] private PlayerTex playerTex;
    void Start()
    {
        transform.position = new Vector3(0.41f, 4.38f, 0);
    }

    void Update()
    {
        if (playerTex.State == PlayerTex.PlayerState.Idle)
        {
            transform.position = new Vector3(0.41f, 4.38f, 0);
        }

        if (playerTex.State == PlayerTex.PlayerState.Throwing)
        {
            transform.position = new Vector3(-2.44f, 4.75f, 0);
        }

        if (playerTex.State == PlayerTex.PlayerState.Biting)
        {
            transform.position = new Vector3(1.17f, 3.46f, 0);
        }

        if (playerTex.State == PlayerTex.PlayerState.Reeling)
        {
            transform.position = new Vector3(0.53f, 4f, 0);
        }

        if (playerTex.State == PlayerTex.PlayerState.Getting)
        {
            transform.position = new Vector3(-3f, 4.53f, 0);
        }
    }
}
