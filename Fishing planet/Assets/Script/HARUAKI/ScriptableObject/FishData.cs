using UnityEngine;
public enum FishSize
{
    Small,
    Medium,
    Large
}
[CreateAssetMenu(menuName = "Fish/FishData")]//インスペクターの右クリックでつくれるように
public class FishDataSO : ScriptableObject
{
    public FishSize fishSize;
    public string fishName;
    public Sprite fishSprite;
    public int fishPrice;
    //public float fishWeight;
    public float[] areafishWeight;

}
