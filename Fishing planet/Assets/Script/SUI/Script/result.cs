using System.Collections.Generic;
using UnityEngine;

public class Result : MonoBehaviour
{
    public static List<string> ResultFish = new List<string>();

    [SerializeField] GameObject fishSlotPrefab;
    [SerializeField] Transform content;

    FishImageDatabase database;

    void Start()
    {
        database = FindObjectOfType<FishImageDatabase>();

        foreach (string fish in ResultFish)
        {
            Debug.Log("åãâ ÅF" + fish);

            Sprite sprite = database.GetFishSprite(fish);

            GameObject slot = Instantiate(fishSlotPrefab, content);

            FishSlot fishSlot = slot.GetComponent<FishSlot>();

            //fishSlot.Setup(sprite);
        }
    }
}