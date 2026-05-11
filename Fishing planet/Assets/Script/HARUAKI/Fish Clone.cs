using System.Collections.Generic;
using UnityEngine;

public class FishClone : MonoBehaviour
{
    public GameObject prefabObj;
    int i = 0;
    GameObject obj;
    public int area;
    List<GameObject> fishCloneList = new List<GameObject>();

    void Start()
    {
        CreateObject();
        CreateObject();
        CreateObject();
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

        FishMove fishMove =
        obj.GetComponent<FishMove>();

        fishMove.area = area;

        fishCloneList.Add(obj);
    }
}
