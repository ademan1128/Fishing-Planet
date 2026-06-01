using UnityEngine;

[CreateAssetMenu(menuName = "Fish/FishData")]//インスペクターの右クリックでつくれるように
public class FishDataSO : ScriptableObject
{

    public string fishName;
    public Sprite fishSprite;
    public int fishPrice;
    //public float fishWeight;
    public float[] areafishWeight;

}
