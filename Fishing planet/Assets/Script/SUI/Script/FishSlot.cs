using System.Collections.Generic;

using UnityEngine;

using UnityEngine.UI;

public class FishSlot : MonoBehaviour
{


    GameManager gameManager;

    //public Image fishImage;

    public Sprite[] sprite;

    public SpriteRenderer[] spriteRenderer;

    private void Start()
    {
        gameManager = GameManager.instance;
        Setup(sprite, spriteRenderer);

        for (int i = 0; i < gameManager.GetFishList.Count; i++)
        {
            spriteRenderer[i].sprite = gameManager.GetFishList[i].fishSprite;
        }
    }

    public void Setup(Sprite[] sprite, SpriteRenderer[] spriteRenderer)
    {
        for (int i = 0; i < sprite.Length; i++)
        {
            if (sprite[i].name.Contains("Small"))
            {

            }

            if (sprite[i].name.Contains("Medium"))
            {

            }

            if (sprite[i].name.Contains("Large"))
            {

            }
            spriteRenderer[i].sprite = sprite[i];
        }
    }
}
