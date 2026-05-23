using System.Collections.Generic;
using UnityEngine;

public class FishImageDatabase : MonoBehaviour
{
    [System.Serializable]
    public class FishImage
    {
        public string fishName;
        public Sprite fishSprite;
    }

    public List<FishImage> fishImages = new List<FishImage>();

    public Sprite GetFishSprite(string fishName)
    {
        foreach (FishImage fish in fishImages)
        {
            if (fish.fishName == fishName)
            {
                return fish.fishSprite;
            }
        }

        return null;
    }
}