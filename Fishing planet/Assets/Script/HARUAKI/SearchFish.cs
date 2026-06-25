using UnityEngine;
using System.Collections.Generic;

public class SearchFish : MonoBehaviour
{
    Fishing Fishing;
    public float SearchDistance = 10f;
    public float BaseSearchDistance = 10f;//スキルの効果を反映させる前の基本的な検索距離
    public List<GameObject> nearestFishList =new List<GameObject>();//検索範囲内の魚を保存するリスト
    public GameObject[] fishObject;
    List<GameObject> fishList = new List<GameObject>();//検索範囲内の魚を一時的に保存するリスト
    int MaxNumFish;
    [SerializeField]SkillEffectType effectValue;//スキルを保存する変数
    void Start()
    {
        Fishing = GameObject.Find("Lure").GetComponent<Fishing>();
        MaxNumFish = Fishing.MaxNumFish;
       
    }
    void Update()
    {
        SearchDistance = BaseSearchDistance * SkillManager.Instance.GetTotalMultiplier(effectValue);//スキルの効果を反映させる
        if (Fishing.CanFishGet && !Fishing.isReeling)
        {
            fishObject = GameObject.FindGameObjectsWithTag("Fish");
            nearestFishList.Clear();
            fishList.Clear();
            for (int i = 0; i < fishObject.Length; i++)
            {
                if (fishObject[i] != null)
                {
                    if (fishObject[i] != null)
                    {
                        FishMove fishMove = fishObject[i].GetComponent<FishMove>();
                        if (fishMove != null && (fishMove.State == FishMove.FishState.Eating || fishMove.State == FishMove.FishState.Reeling))
                        {
                            continue; // すでに食いついている魚はスルーして次の魚の計算へ
                        }

                        float distance = Vector2.Distance(transform.position, fishObject[i].transform.position);
                        if (distance <= SearchDistance)
                        {
                            fishList.Add(fishObject[i]);
                        }
                    }
                }
            }
            fishList.Sort((a, b) =>
            {
                float distanceA = Vector2.Distance(transform.position, a.transform.position);
                float distanceB = Vector2.Distance(transform.position, b.transform.position);
                return distanceA.CompareTo(distanceB);
            });

            for (int i = 0; i < Mathf.Min(MaxNumFish, fishList.Count); i++)
            {
                nearestFishList.Add(fishList[i]);
                //Debug.Log(MaxNumFish);
            }


        }
        else if (!Fishing.CanFishGet || Fishing.isReeling)
        {
            nearestFishList.Clear();
            fishList.Clear();
        }
    }
}