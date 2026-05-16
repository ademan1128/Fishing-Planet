using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;


public class result : MonoBehaviour
{
    List<string> results = new List<string>();
    private void Start()
    {
        results.Add("マグロ");
        results.Add("サメ");
        results.Add("ブリ");

        // 1つずつ表示
        foreach (string fish in results)
        {
            Debug.Log(fish);
        }
    }
}
