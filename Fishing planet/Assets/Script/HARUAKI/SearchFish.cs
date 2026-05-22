using UnityEngine;
using System.Collections.Generic;

public class SearchFish : MonoBehaviour
{
    Fishing Fishing;
    public float SearchDistance = 10f;
    public List<GameObject> nearestFishList =new List<GameObject>();//検索範囲内の魚を保存するリスト
    GameObject[] fishObject;
    List<GameObject> fishList = new List<GameObject>();//検索範囲内の魚を一時的に保存するリスト
    public int MaxFish;
    void Start()
    {
        Fishing = GameObject.Find("Lure").GetComponent<Fishing>();
        MaxFish = Fishing.MaxNumFish;
        Debug.Log(MaxFish);
    }
    void Update()
    {
        fishObject = GameObject.FindGameObjectsWithTag("Fish");
        nearestFishList.Clear();
        fishList.Clear();
        foreach (GameObject fish in fishObject)
        {
            if (fish != null)
            {
                float distance =
                    Vector2.Distance(transform.position,fish.transform.position);
                if (distance <= SearchDistance)
                {
                    fishList.Add(fish);
                }
            }
        }
        fishList.Sort((a, b) =>
        {
            float distanceA =Vector2.Distance(transform.position,a.transform.position);
            float distanceB =Vector2.Distance(transform.position,b.transform.position);
            return distanceA.CompareTo(distanceB);
        });

        for (int i = 0;i < Mathf.Min(MaxFish, fishList.Count);i++)
        {
            nearestFishList.Add(fishList[i]);
        }
    }
}