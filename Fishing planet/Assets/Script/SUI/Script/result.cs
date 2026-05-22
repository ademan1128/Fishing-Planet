using System.Collections.Generic;
using UnityEngine;

public class result : MonoBehaviour
{
    // ’Ş‚ê‚½‹›‚ğ•Û‘¶
    public static List<string> ResultFish = new List<string>();

    void Start()
    {
        foreach (string fish in ResultFish)
        {
            Debug.Log("Œ‹‰ÊF" + fish);
        }
    }
}