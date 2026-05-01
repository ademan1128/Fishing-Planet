using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sea1 : MonoBehaviour
{   int rnd;
    List<string> Sea1List = new List<string>();
    [SerializeField] Transform Lure;
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
            float x = Lure.position.x / 3;

            List<float> weights = new List<float>
            {
                50,30,10
            };
            rnd = GetRandomIndex(weights);
            Debug.Log("Lureが海から出た");
            Debug.Log(Sea1List[rnd] + "が釣れた");
        }
    }
    int GetRandomIndex(List<float> weights)
    {
        float total = 0f;
        foreach (var w in weights)
            total += w;
        float r = Random.Range(0, total);
        for (int i = 0; i < weights.Count; i++)
        {
            r -= weights[i];
            if (r <= 0)
                return i;
        }
        return weights.Count - 1;
    }

}

