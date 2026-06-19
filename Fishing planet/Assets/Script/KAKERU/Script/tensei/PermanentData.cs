using UnityEngine;
using System.Collections.Generic;
[System.Serializable]
public class PermanentData
{
    public int rebirthCount;//転生回数
    public float rebirthPoints;//累計転生ポイント
    public List<bool> TrackRecordList;//実績フラグ一覧
}
