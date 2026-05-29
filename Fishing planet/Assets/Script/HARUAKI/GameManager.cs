using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    int rnd;
    //public List<Sprite> Sea1List = new List<Sprite>();
    public List<GameObject> fishCloneList = new List<GameObject>();
    [SerializeField] Transform Lure;
    Fishing fishing;
    [SerializeField] GameObject prefabObj;
    [SerializeField] Sprite fishSprite;
    [SerializeField]List<FishDataSO> fishDataList = new List<FishDataSO>();
    public static int ALLFish = 5;
    void Start()
    {
        Lure = GameObject.Find("Lure").transform;
        fishing = Lure.GetComponent<Fishing>();
        for (int i = 0; i < ALLFish; i++)
        {
            CreateFish(i);
        }
    }
    void Update()
    {
        if (fishing.GotFish)
        {
            foreach (int area in FishMove.GetFishArea)
            {
                List<float> weights = new List<float>();//重みを入れるリストを作成してるよ

                foreach (FishDataSO fish in fishDataList)//fishDataListにあるFishDataSOの中に入ってる魚を重みで抽選してるよ多分
                {

                    if (area == 1)
                    {
                        weights.Add(fish.area1fishWeight);
                    }
                    else if (area == 2)
                    {
                        weights.Add(fish.area2fishWeight);

                    }
                    else if (area == 3)
                    {
                        weights.Add(fish.area3fishWeight);
                    }
                    //Sprite catchFish = Sea1List[rnd];
                }
                rnd = GetRandomIndex(weights);
                FishDataSO catchFish = fishDataList[rnd];//ここで重みで抽選した魚を取得してる
                                                         //画像のデータを読みこめる
                                                         //でもどうやってここの魚に画像を反映させる？
                Debug.Log("釣れた魚：" + catchFish.fishName);
            }
            FishMove.GetFishArea.Clear();
            fishing.GotFish = false;
        }
    }

    void CreateFish(int index)
    {
        GameObject obj = Instantiate(prefabObj);
        obj.name = "Fish" + (index + 1);
        int area = 1;
        int rndX = Random.Range(0, 10);
        int rndY = Random.Range(-5, 0);
        obj.transform.position = new Vector2(rndX, rndY);
        FishMove fishMove = obj.GetComponent<FishMove>();
        fishMove.area = area;
        SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
        sr.sprite = fishSprite;
        fishCloneList.Add(obj);
    }

    int GetRandomIndex(List<float> weights)
    {
        float total = 0f;

        foreach (var w in weights)
        {
            total += w;
        }
        float r = Random.Range(0, total);
        for (int i = 0; i < weights.Count; i++)
        {
            r -= weights[i];
            if (r <= 0)
            {
                return i;
            }
        }
        return weights.Count - 1;
    }
}

