using UnityEngine;

public class FishMove : MonoBehaviour
{
    [SerializeField] float Minspeed = 0.3f; // 最低速度
    [SerializeField] float Maxspeed = 1.0f; // 最高速度
    [SerializeField] float Mindistance = 1.0f; // 最低往復の長さ
    [SerializeField] float Maxdistance = 8.0f; // 最高往復の長さ

    float speed;
    float distance;
    Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        //開始時にランダムな値を決める
        speed = Random.Range(Minspeed, Maxspeed);
        distance = Random.Range(Mindistance, Maxdistance);
    }

    void Update()
    {
        // X軸方向に往復運動
        float movement = Mathf.PingPong(Time.time * speed, distance);
        transform.position = startPos + new Vector3(movement, 0, 0);
    }
}
