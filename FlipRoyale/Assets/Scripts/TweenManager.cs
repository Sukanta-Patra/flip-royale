using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum TweenType
{
    Move,
    Scale
}

public struct Tween
{
    public bool active;
    public TweenType type;
    public RectTransform rect;

    public Vector2 start;
    public Vector2 end;

    public float duration;
    public float elapsed;
}

public class TweenManager : MonoBehaviour
{
    public static TweenManager Instance;

    private const int MAX_TWEENS = 256;
    private Tween[] tweens = new Tween[MAX_TWEENS];

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float dt = Time.deltaTime;

        for (int i = 0; i < MAX_TWEENS; i++)
        {
            if (!tweens[i].active)
                continue;

            Tween t = tweens[i];
            t.elapsed += dt;

            float progress = Mathf.Clamp01(t.elapsed / t.duration);
            Vector2 value = Vector2.LerpUnclamped(t.start, t.end, progress);

            switch (t.type)
            {
                case TweenType.Move:
                    t.rect.anchoredPosition = value;
                    break;

                case TweenType.Scale:
                    t.rect.localScale = new Vector3(value.x, value.y, 1f);
                    break;
            }

            if (t.elapsed >= t.duration)
                t.active = false;

            tweens[i] = t;
        }
    }


    private int GetFreeTween()
    {
        for (int i = 0; i < MAX_TWEENS; i++)
        {
            if (!tweens[i].active)
                return i;
        }

        return -1;
    }

    public void Move(RectTransform rect, Vector2 endPos, float duration)
    {
        int i = GetFreeTween();
        if (i == -1) return;

        tweens[i] = new Tween
        {
            active = true,
            type = TweenType.Move,
            rect = rect,
            start = rect.anchoredPosition,
            end = endPos,
            duration = duration,
            elapsed = 0f
        };
    }

    public void Scale(RectTransform rect, Vector3 endScale, float duration)
    {
        int i = GetFreeTween();
        if (i == -1) return;

        tweens[i] = new Tween
        {
            active = true,
            type = TweenType.Scale,
            start = rect.localScale,
            rect = rect,
            end = endScale,
            duration = duration,
            elapsed = 0f
        };
    }

}
