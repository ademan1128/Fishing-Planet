using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    void Update()
    {
        // EnterƒL[‚ª‰Ÿ‚³‚ê‚½uŠÔ
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Main game");
        }
    }
}