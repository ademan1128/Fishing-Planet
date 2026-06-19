using UnityEngine;
using UnityEngine.SceneManagement;

public class SkillButton : MonoBehaviour
{
    public FishSlot fishSlot;
    // ƒ{ƒ^ƒ“‚ğ‰Ÿ‚µ‚½‚Æ‚«‚ÉŒÄ‚Î‚ê‚éŠÖ”
    public void OnClickStartButton()
    {
        GameManager.instance.Reset();
        SceneManager.LoadScene("Skill Tree Scene");
    }
}
