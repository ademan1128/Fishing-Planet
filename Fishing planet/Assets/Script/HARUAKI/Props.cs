using UnityEngine;


public class Props : MonoBehaviour
{

    [SerializeField] private Vector2 minPos;
    [SerializeField] private Vector2 maxPos;
    [SerializeField] private Vector3 GuidelightPos;
    [Header("Obj")]
    public SpriteRenderer sr;

    public enum StageProps
    { 
        Yacht,
        Guidelight,
    }
    public StageProps props;
    void Start()
    {
        sr.gameObject.SetActive(false);
        if (GameManager.instance.stageTime == GameManager.StageTime.Noon)
        {
            
        }
        else if (GameManager.instance.stageTime == GameManager.StageTime.Evening)
        {
            sr.gameObject.SetActive(true);
            props = StageProps.Yacht;
            float x = Random.Range(minPos.x, maxPos.x);
        }
        else if (GameManager.instance.stageTime == GameManager.StageTime.Night)
        {
            sr.gameObject.SetActive(true);
            props = StageProps.Guidelight;
        }
    }

    void Update()
    {
        if(props == StageProps.Yacht)
        {
            transform.Translate(Vector3.left * 0.1f * Time.deltaTime);

        }
        if (props == StageProps.Guidelight)
        {
            transform.position = GuidelightPos;

        }
    }
}
