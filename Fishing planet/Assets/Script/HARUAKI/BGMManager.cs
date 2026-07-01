using UnityEngine;
using UnityEngine.SceneManagement;

public class BGMManager : MonoBehaviour
{
    public static BGMManager instance;

    [SerializeField] AudioSource audioSource;

    [SerializeField] AudioClip titleBGM;
    [SerializeField] AudioClip gameBGM;
    [SerializeField] AudioClip shopBGM;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case "Title":
                ChangeBGM(titleBGM);
                break;

            case "Main game":
                ChangeBGM(gameBGM);
                break;

            case "Result":
                ChangeBGM(shopBGM);
                break;
        }
    }

    void ChangeBGM(AudioClip clip)
    {
        if (audioSource.clip == clip) return;

        audioSource.clip = clip;
        audioSource.Play();
    }
}
