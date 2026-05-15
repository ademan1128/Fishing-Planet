using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class FishList : MonoBehaviour
{
    public List<GameObject> SmallfishList = new List<GameObject>();//小さい魚のリスト
    public List<GameObject> NormalfishList = new List<GameObject>();//普通の魚のリスト
    public List<GameObject> BigfishList = new List<GameObject>();//大きい魚のリスト
    public List<GameObject> RarefishList = new List<GameObject>();//レアな魚のリスト

    void Start()
    {
        //魚のリストに魚を追加
        SmallfishList.Add(GameObject.Find("Fish1"));//ここにそれぞれの深さで釣れる魚を追加していく
        SmallfishList.Add(GameObject.Find("Fish2"));
        SmallfishList.Add(GameObject.Find("Fish3"));

        NormalfishList.Add(GameObject.Find("Fish4"));
        NormalfishList.Add(GameObject.Find("Fish5"));
        NormalfishList.Add(GameObject.Find("Fish6"));

        BigfishList.Add(GameObject.Find("Fish7"));
        BigfishList.Add(GameObject.Find("Fish8"));
        BigfishList.Add(GameObject.Find("Fish9"));

        RarefishList.Add(GameObject.Find("Fish10"));
    }


    void Update()
    {

    }
}
