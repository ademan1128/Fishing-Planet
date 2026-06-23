using UnityEngine;
using UnityEngine.UIElements;
using static GameManager;


public class Cloud : MonoBehaviour
{
    //[SerializeField] private GameObject cloudPrefab;
    private SpriteRenderer sr;
    [SerializeField] private Sprite[] NoonClouds;
    [SerializeField] private Sprite[] EvenigClouds;
    [SerializeField] private Sprite[] NigthClouds;


    [SerializeField] private float minSpeed = 0.5f;
    [SerializeField] private float maxSpeed = 2.0f;

    private float speed;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

        if (GameManager.instance.stageTime == GameManager.StageTime.Noon)
        {
            int index = Random.Range(0, NoonClouds.Length);
            sr.sprite = NoonClouds[index];
        }
        else if (GameManager.instance.stageTime == GameManager.StageTime.Evening)
        {
            int index = Random.Range(0, EvenigClouds.Length);
            sr.sprite = EvenigClouds[index];
        }
        else if (GameManager.instance.stageTime == GameManager.StageTime.Night)
        {
            int index = Random.Range(0, NigthClouds.Length);
            sr.sprite = NigthClouds[index];
        }


        speed = Random.Range(minSpeed, maxSpeed);
    }

    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }
}
