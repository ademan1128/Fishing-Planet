using UnityEngine;
using UnityEngine.SceneManagement;

public class MainButton : MonoBehaviour
{
    public FishSlot fishSlot;
    //public GameManager gameManager;

    public void OnClickStartButton()
    {
        GameManager.instance.Reset();
        SceneManager.LoadScene("Main game");
    }
}