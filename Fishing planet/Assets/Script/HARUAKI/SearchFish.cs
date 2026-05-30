using UnityEngine;
using System.Collections.Generic;

public class SearchFish : MonoBehaviour
{
    Fishing Fishing;
    public float SearchDistance = 10f;
    public List<GameObject> nearestFishList =new List<GameObject>();//検索範囲内の魚を保存するリスト
    public GameObject[] fishObject;
    List<GameObject> fishList = new List<GameObject>();//検索範囲内の魚を一時的に保存するリスト
    public int MaxFish;
    void Start()
    {
        Fishing = GameObject.Find("Lure").GetComponent<Fishing>();
        MaxFish = Fishing.MaxNumFish;

    }
    void Update()
    {
        if (Fishing.CanFishGet && !Fishing.isReeling)
        {
            fishObject = GameObject.FindGameObjectsWithTag("Fish");
            nearestFishList.Clear();
            fishList.Clear();
            for (int i = 0; i < fishObject.Length; i++)
            {
                if (fishObject[i] != null)
                {
                    float distance =
                        Vector2.Distance(transform.position, fishObject[i].transform.position);
                    if (distance <= SearchDistance)
                    {
                        fishList.Add(fishObject[i]);
                    }
                    else
                    {
                        fishList.Remove(fishObject[i]);
                    }
                }
            }
            fishList.Sort((a, b) =>
            {
                float distanceA = Vector2.Distance(transform.position, a.transform.position);
                float distanceB = Vector2.Distance(transform.position, b.transform.position);
                return distanceA.CompareTo(distanceB);
            });

            for (int i = 0; i < Mathf.Min(MaxFish, fishList.Count); i++)
            {
                nearestFishList.Add(fishList[i]);
                Debug.Log(MaxFish);
            }


        }
    }
}