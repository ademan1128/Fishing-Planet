using System.Collections.Generic;
using UnityEngine;

public class FishClone : MonoBehaviour
{
    public GameObject prefabObj;
    int i = 0;
    GameObject obj;
    public int area;
    public static int ALLFish = 5;
    GameManager gameManager;
    public Sprite sprite;
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        for (int a = 0; a < ALLFish; a++)
        {
            CreateObject();
        }
        SpriteChange();
    }

    void CreateObject()
    {
        obj = Instantiate(prefabObj, Vector2.zero, Quaternion.identity);

        i++;
        obj.name = "Fish" + i;
        if (0 <= i && i <= 5)
        {
            area = 1;
            int rndX = Random.Range(0, 10);
            int rndY = Random.Range(-5, 0);
            obj.transform.position = new Vector2(rndX, rndY);

        }

        FishMove fishMove = obj.GetComponent<FishMove>();

        fishMove.area = area;
        gameManager.fishCloneList.Add(obj);
    }

    void SpriteChange()
    {
        SpriteRenderer fish;
        for (int i = 0; i < gameManager.fishCloneList.Count; i++)
        {
            fish = gameManager.fishCloneList[i].GetComponent<SpriteRenderer>();
            fish.sprite = sprite;
        }
    }
}