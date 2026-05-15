using System.Collections.Generic;
using UnityEngine;

public class Lurerange : MonoBehaviour
{
    public List<FishMove> targetFish;//ここでListを宣言して、複数の魚を管理できるようにする
    public bool GetFish;
    Fishing MaxNumFish;
    void Start()
    {
        MaxNumFish = GameObject.Find("Lure").GetComponent<Fishing>();
        targetFish = new List<FishMove>();
        FishMove.NumFish = 0;
        GetFish = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            FishMove fish = other.GetComponent<FishMove>();
            if (fish != null && !targetFish.Contains(fish))//魚のスクリプトがあって、まだリストに入っていなかったら
            {
                Debug.Log("釣れた");
                targetFish.Add(fish);//リストに追加する
                GetFish = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            FishMove fish = other.GetComponent<FishMove>();


            //if (fish != null && !fish.Eating)
            //{
            //    //targetFish.Remove(fish);
            //}
        }
    }
}
