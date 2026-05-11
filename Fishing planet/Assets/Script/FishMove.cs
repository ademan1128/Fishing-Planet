using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Collections;
using UnityEngine;

public class FishMove : MonoBehaviour
{
    public int area;
    Vector2 movePosition;
    void Start()
    {
        movePosition = moveRandomPosition();
    }



    void Update()
    {
        if (Vector2.Distance(transform.position, movePosition) < 0.1f)
        {

            movePosition = moveRandomPosition();
        }
        float speed = Random.Range(0f, 1f); ;
        this.transform.position = Vector2.MoveTowards(transform.position, movePosition, speed * Time.deltaTime);

    }

    Vector2 moveRandomPosition()
    {
        if (area == 1)
        {
            int rndX = Random.Range(0, 10);
            int rndY = Random.Range(-5, 0);
            return new Vector2(rndX, rndY);
        }
        return Vector2.zero;
    }


}
