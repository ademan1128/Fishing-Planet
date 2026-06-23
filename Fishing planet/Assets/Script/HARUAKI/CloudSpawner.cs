using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cloudPrefab;
    [SerializeField] private Vector2 minPos;
    [SerializeField] private Vector2 maxPos;
    [SerializeField] private int cloudCount = 10;

    void Start()
    {
        for (int i = 0; i < cloudCount; i++)
        {
            float x = Random.Range(minPos.x, maxPos.x);
            float y = Random.Range(minPos.y, maxPos.y);

            Instantiate(cloudPrefab, new Vector3(x, y, 0), Quaternion.identity);
        }
    }
}

