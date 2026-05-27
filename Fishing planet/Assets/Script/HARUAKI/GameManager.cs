using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{   int rnd;
    List<Sprite> Sea1List = new List<Sprite>();
    Fishing MaxNumFish;
    Fishing Fishing;
    [SerializeField] Transform Lure;
    public List<GameObject> fishCloneList = new List<GameObject>();
    void Start()
    {
            //Sea1List.Add();
            //Sea1List.Add();
            //Sea1List.Add();
        Lure = GameObject.Find("Lure").transform;
        MaxNumFish = GameObject.Find("Lure").GetComponent<Fishing>();
        Fishing = GameObject.Find("Lure").GetComponent<Fishing>();

    }


    void Update()
    {


        if (Fishing.GotFish == true)
        {

            foreach (int area in FishMove.GetFishArea)
            {
                List<float> weights = new List<float> { 50, 30, 20 };
                //int area = Mathf.FloorToInt(Lure.position.x / 10f); いつか使う
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
                    rnd = GetRandomIndex(weights);
                    Sprite catchFish = Sea1List[rnd];
                    Debug.Log("釣れた魚：" + catchFish);
            }
            FishMove.GetFishArea.Clear();
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

