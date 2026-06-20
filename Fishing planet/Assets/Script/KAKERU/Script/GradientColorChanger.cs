using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class GradientColorChanger : MonoBehaviour
{
    [SerializeField] SkillData targetSkill;
    [Header("グラデーションカラー")]
    public Color colorStart = Color.white;
    public Color colorEnd = Color.black;

    [Header("変化のスピード")]
    public float speed = 1.0f;

    private Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (SkillManager.Instance.IsLearned(targetSkill))
        {
            return;
        }
        ////時間経過で0.0から1.0の間を往復する値（サイン波を利用）
        float time = Mathf.Sin(Time.time * speed) * 0.5f + 0.5f;
        image.color = Color.Lerp(colorStart, colorEnd, time);

    }

    void SetGradient(Image img,Color start,Color end)
    {
        if (img == null || img.canvasRenderer == null) return;

        List<UIVertex> vertices = new List<UIVertex>();

        UIVertex v0 = new UIVertex();
        v0.color = start;
        vertices.Add(v0);

        UIVertex v1 = new UIVertex();
        v1.color = start;
        vertices.Add(v1);

        UIVertex v2 = new UIVertex();
        v2.color = end;
        vertices.Add(v2);

        UIVertex v3 = new UIVertex();
        v3.color = end;
        vertices.Add(v3);

        img.canvasRenderer.SetVertices(vertices);
    }
}
