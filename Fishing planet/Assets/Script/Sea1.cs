using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting;

public class Sea1 : MonoBehaviour
{   int rnd;
    List<string> Sea1List = new List<string>();

    void Start()
    {

        Sea1List.Add("イワシ");
        Sea1List.Add("サバ");
        Sea1List.Add("アジ");


    }

    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Lure"))
        {
            Debug.Log("Lureが海に入った");
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Lure"))
        {
            Debug.Log("Lureが海から出た");
            rnd = Random.Range(0, Sea1List.Count);
            Debug.Log(Sea1List[rnd] + "が釣れた");
        }
    }
}
