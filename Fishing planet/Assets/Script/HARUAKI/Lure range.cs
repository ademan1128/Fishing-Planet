using UnityEngine;

public class Lurerange : MonoBehaviour
{
    public bool LureMove;
    public static int NumFish;
    public bool GetFish;
    Fishing MaxNumFish;
    void Start()
    {
        MaxNumFish = GameObject.Find("Lure").GetComponent<Fishing>();
        LureMove = false;
        NumFish = 0;
        GetFish = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            LureMove = true;
            GetFish = true;
            if (NumFish < MaxNumFish.MaxNumFish)
            {
                NumFish++;
            }
        }
    }
}
