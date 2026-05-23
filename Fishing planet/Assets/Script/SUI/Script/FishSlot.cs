using UnityEngine;
using UnityEngine.UI;

public class FishSlot : MonoBehaviour
{
    public Image fishImage;

    public void Setup(Sprite sprite)
    {
        fishImage.sprite = sprite;
    }
}