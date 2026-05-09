using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour
{
    // ƒ{ƒ^ƒ“‚ğ‰Ÿ‚µ‚½‚Æ‚«‚ÉŒÄ‚Î‚ê‚éŠÖ”
    public void OnClickStartButton()
    {
        SceneManager.LoadScene("Main game");
    }
}