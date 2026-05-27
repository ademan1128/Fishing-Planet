using UnityEngine;
using UnityEngine.UI;

public class FishSlot : MonoBehaviour
{
    //public Image fishImage;
    public Sprite[] sprite;
    public SpriteRenderer[] spriteRenderer;

    private void Start()
    {
        Setup(sprite,spriteRenderer);
    }
    public void Setup(Sprite[] sprite, SpriteRenderer[] spriteRenderer)
    {
        
        for(int i = 0; i< sprite.Length; i++)
        {
            if (sprite[i].name.Contains("Normal"))
            {
                
            }
                spriteRenderer[i].sprite = sprite[i];
        }
        
    }
}