using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RebirthSystem : MonoBehaviour
{
    [SerializeField] Text rebirthPointText;
    [SerializeField] Text rebirthCountText;
    [SerializeField] Button rebirthButton;

    private void Start()
    {
        if (RebirthManager.Instance == null)
        {
            Debug.LogError("RebirthManagerÇ™sceneÇ…ë∂ç›ÇµÇ‹ÇπÇÒ");
        }
        rebirthPointText.text = RebirthManager.Instance.permanent.rebirthPoints.ToString();
        rebirthCountText.text = RebirthManager.Instance.permanent.rebirthCount.ToString();
        SkillSystem skillSystem = FindObjectOfType<SkillSystem>();
        if (skillSystem != null)
        {
            rebirthButton.interactable = skillSystem.CanRebirth();
        }
    }

    void OnRebirthButton()
    {
        SkillSystem skillSystem = FindObjectOfType<SkillSystem>();
        if(skillSystem == null|| !skillSystem.CanRebirth())
        {
            return;
        }
        RebirthManager.Instance.Rebirth();
        SceneManager.LoadScene("Main game");
    }

}