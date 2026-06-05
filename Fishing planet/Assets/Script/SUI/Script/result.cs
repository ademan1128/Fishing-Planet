using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public static List<string> ResultFish = new List<string>();

    GameManager gameManager;

    [SerializeField] GameObject fishSlotPrefab;
    [SerializeField] Transform content;

    FishImageDatabase database;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();

        foreach (string fish in ResultFish)
        {
            Debug.Log("結果：" + fish);

            // すでに取得したリストを使う
            FishDataSO data = gameManager.GetFishList.Find(x => x.fishName == fish);

            if (data == null)
            {
                Debug.LogWarning("魚データが見つからない: " + fish);
                continue;
            }

            Sprite sprite = data.fishSprite;

            GameObject slot = Instantiate(fishSlotPrefab, content);

            FishSlot fishSlot = slot.GetComponent<FishSlot>();

            //fishSlot.Setup(sprite, spriteRenderer);
        }
    }
}

//    void Start()
//    {

//        gameManager = FindFirstObjectByType<GameManager>();
//        database = FindObjectOfType<FishImageDatabase>();

//        List<FishDataSO> fishList = gameManager.GetFishList;

//        foreach (string fish in ResultFish)
//        {
//            Debug.Log("結果：" + fish);

//            // リストから該当データを探す
//            FishDataSO data = gameManager.GetFishList
//                .Find(x => x.fishName == fish);

//            if (data == null)
//            {
//                Debug.LogWarning("魚データが見つからない: " + fish);
//                continue;
//            }

//            Sprite sprite = data.fishSprite;

//            GameObject slot = Instantiate(fishSlotPrefab, content);

//            FishSlot fishSlot = slot.GetComponent<FishSlot>();

//            fishSlot.Setup(sprite);
//        }
//    }
//}