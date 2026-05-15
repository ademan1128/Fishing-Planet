using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sea1 : MonoBehaviour
{   int rnd;
    List<string> Sea1List = new List<string>();
    Lurerange Lurerange;
    Fishing MaxNumFish;
    bool onsea;
    [SerializeField] Transform Lure;
    void Start()
    {
            Sea1List.Add("イワシ");
            Sea1List.Add("サバ");
            Sea1List.Add("アジ");
        Lurerange = GameObject.Find("LureRange").GetComponent<Lurerange>();
        onsea = false;
        Lure = GameObject.Find("Lure").transform;
        MaxNumFish = GameObject.Find("Lure").GetComponent<Fishing>();
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
            List<float> weights = new List<float> { 50, 30, 20 };
            int area = Mathf.FloorToInt(Lure.position.x / 10f);
            if (area == 0)
            {
                weights = new List<float> { 70, 20, 0 };
            }
            else if (area == 1)
            {
                weights = new List<float> { 10, 60, 20 };
            }
            else if (area == 2)
            {
                weights = new List<float> { 0, 20, 60 };
            }
            Debug.Log("area: " + area);
            Debug.Log("Lureが海から出た");
            if (Lurerange.targetFish.Count > 0)
            {
                for (int i = 0; i < Lurerange.targetFish.Count; i++)
                {
                    rnd = GetRandomIndex(weights);
                    Debug.Log(Sea1List[rnd] + "が釣れた");
                }

            }else
            {
                Debug.Log("何も釣れなかった");
            }
        }
    }
    int GetRandomIndex(List<float> weights)
    {
        float total = 0f;
        foreach (var w in weights) total += w;
        float r = Random.Range(0, total);
        for (int i = 0; i < weights.Count; i++)// 重みの合計からランダムな値を引いていき、0以下になったときのインデックスを返す
        { r -= weights[i];
            if (r <= 0) return i; 
        }
        return weights.Count - 1; 
    } 
}

