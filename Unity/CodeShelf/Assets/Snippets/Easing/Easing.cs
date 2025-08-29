using System;

/// <summary>Static class providing set of easing functions.<br/>
/// Functions are based on <see href="https://easings.net">easings.net</see> by <see href="https://sitnik.ru/en">Andrey Sitnik</see> and <see href="https://solovev.one">Ivan Solovev</see></summary>
public static class Easing
{
    const float c1 = 1.70158f;
    const float c2 = c1 * 1.525f;
    const float c3 = c1 + 1.0f;
    const float c4 = (2.0f * (float)Math.PI) / 3.0f;
    const float c5 = (2.0f * (float)Math.PI) / 4.5f;
    const float n1 = 7.5625f;
    const float d1 = 2.75f;

    public static float Ease(float x, Ease ease)
    {
        return ease switch
        {
            global::Ease.InSine => InSine(x),
            global::Ease.OutSine => OutSine(x),
            global::Ease.InOutSine => InOutSine(x),
            global::Ease.InQuad => InQuad(x),
            global::Ease.OutQuad => OutQuad(x),
            global::Ease.InOutQuad => InOutQuad(x),
            global::Ease.InCubic => InCubic(x),
            global::Ease.OutCubic => OutCubic(x),
            global::Ease.InOutCubic => InOutCubic(x),
            global::Ease.InQuart => InQuart(x),
            global::Ease.OutQuart => OutQuart(x),
            global::Ease.InOutQuart => InOutQuart(x),
            global::Ease.InQuint => InQuint(x),
            global::Ease.OutQuint => OutQuint(x),
            global::Ease.InOutQuint => InOutQuint(x),
            global::Ease.InExpo => InExpo(x),
            global::Ease.OutExpo => OutExpo(x),
            global::Ease.InOutExpo => InOutExpo(x),
            global::Ease.InCirc => InCirc(x),
            global::Ease.OutCirc => OutCirc(x),
            global::Ease.InOutCirc => InOutCirc(x),
            global::Ease.InBack => InBack(x),
            global::Ease.OutBack => OutBack(x),
            global::Ease.InOutBack => InOutBack(x),
            global::Ease.InElastic => InElastic(x),
            global::Ease.OutElastic => OutElastic(x),
            global::Ease.InOutElastic => InOutElastic(x),
            global::Ease.InBounce => InBounce(x),
            global::Ease.OutBounce => OutBounce(x),
            global::Ease.InOutBounce => InOutBounce(x),
            _ => x,
        };
    }

    public static float InSine(float x)
    {
        return 1.0f - (float)Math.Cos((x * Math.PI) * 0.5f);
    }
    public static float OutSine(float x)
    {
        return (float)Math.Sin((x * Math.PI) * 0.5f);
    }
    public static float InOutSine(float x)
    {
        return -((float)Math.Cos(x * Math.PI) - 1.0f) * 0.5f;
    }
    public static float InQuad(float x)
    {
        return x * x;
    }
    public static float OutQuad(float x)
    {
        return 1.0f - (1.0f - x) * (1.0f - x);
    }
    public static float InOutQuad(float x)
    {
        return x < 0.5f ? 2.0f * x * x : 1.0f - ((-2.0f * x + 2.0f) * (-2.0f * x + 2.0f)) * 0.5f;
    }
    public static float InCubic(float x)
    {
        return x * x * x;
    }
    public static float OutCubic(float x)
    {
        return 1.0f - (1.0f - x) * (1.0f - x) * (1.0f - x);
    }
    public static float InOutCubic(float x)
    {
        return x < 0.5f ? 4.0f * x * x * x : 1.0f - ((-2.0f * x + 2.0f) * (-2.0f * x + 2.0f) * (-2.0f * x + 2.0f)) * 0.5f;
    }
    public static float InQuart(float x)
    {
        return x * x * x * x;
    }
    public static float OutQuart(float x)
    {
        return 1.0f - (1.0f - x) * (1.0f - x) * (1.0f - x) * (1.0f - x);
    }
    public static float InOutQuart(float x)
    {
        return x < 0.5f ? 8.0f * x * x * x * x : 1.0f - ((-2.0f * x + 2.0f) * (-2.0f * x + 2.0f) * (-2.0f * x + 2.0f) * (-2.0f * x + 2.0f)) * 0.5f;
    }
    public static float InQuint(float x)
    {
        return x * x * x * x * x;
    }
    public static float OutQuint(float x)
    {
        return 1.0f - (1.0f - x) * (1.0f - x) * (1.0f - x) * (1.0f - x) * (1.0f - x);
    }
    public static float InOutQuint(float x)
    {
        return x < 0.5f ? 16.0f * x * x * x * x * x : 1.0f - ((-2.0f * x + 2.0f) * (-2.0f * x + 2.0f) * (-2.0f * x + 2.0f) * (-2.0f * x + 2.0f) * (-2.0f * x + 2.0f)) * 0.5f;
    }
    public static float InExpo(float x)
    {
        return x == 0.0f ? 0.0f : (float)Math.Pow(2.0, 10.0 * x - 10.0);
    }
    public static float OutExpo(float x)
    {
        return x == 1.0f ? 1.0f : 1.0f - (float)Math.Pow(2.0, -10.0 * x);
    }
    public static float InOutExpo(float x)
    {
        return x == 0.0f ? 0.0f
            : (x == 1.0f ? 1.0f
            : (x < 0.5f ? (float)Math.Pow(2.0, 20.0 * x - 10.0) * 0.5f
            : (2.0f - (float)Math.Pow(2.0, -20.0 * x + 10.0)) * 0.5f));
    }
    public static float InCirc(float x)
    {
        return 1.0f - (float)Math.Sqrt(1.0 - (x * x));
    }
    public static float OutCirc(float x)
    {
        return (float)Math.Sqrt(1.0 - ((x - 1.0) * (x - 1.0)));
    }
    public static float InOutCirc(float x)
    {
        return x < 0.5f ? (1.0f - (float)Math.Sqrt(1.0 - ((2.0 * x) * (2.0 * x)))) * 0.5f
            : ((float)Math.Sqrt(1.0 - Math.Pow(-2.0 * x + 2.0, 2.0)) + 1.0f) * 0.5f;
    }
    public static float InBack(float x)
    {
        return c3 * x * x * x - c1 * x * x;
    }
    public static float OutBack(float x)
    {
        return 1 + c3 * ((x - 1) * (x - 1) * (x - 1)) + c1 * ((x - 1) * (x - 1));
    }
    public static float InOutBack(float x)
    {
        return x < 0.5f ? (((2.0f * x) * (2.0f * x)) * ((c2 + 1.0f) * 2.0f * x - c2)) * 0.5f
          : ((2.0f * x - 2.0f) * (2.0f * x - 2.0f) * ((c2 + 1.0f) * (x * 2.0f - 2.0f) + c2) + 2.0f) * 0.5f;
    }
    public static float InElastic(float x)
    {
        return x == 0.0f ? 0.0f
            : (x == 1.0f ? 1.0f
            : -((float)Math.Pow(2.0, 10.0 * x - 10.0)) * (float)Math.Sin((x * 10.0 - 10.75) * c4));
    }
    public static float OutElastic(float x)
    {
        return x == 0.0f ? 0.0f
            : (x == 1.0f ? 1.0f
            : ((float)Math.Pow(2.0, -10.0 * x)) * (float)Math.Sin((x * 10.0 - 0.75) * c4) + 1.0f);
    }
    public static float InOutElastic(float x)
    {
        return x == 0.0f ? 0.0f
            : (x == 1.0f ? 1.0f
            : (x < 0.5f ? -((float)Math.Pow(2.0, 20.0 * x - 10.0) * (float)Math.Sin((20.0 * x - 11.125) * c5)) * 0.5f
            : ((float)Math.Pow(2.0, -20.0 * x + 10.0) * (float)Math.Sin((20.0 * x - 11.125) * c5)) * 0.5f + 1.0f));
    }
    public static float InBounce(float x)
    {
        return 1.0f - OutBounce(1.0f - x);
    }
    public static float OutBounce(float x)
    {
        if (x < 1.0f / d1)
        {
            return n1 * x * x;
        }
        else if (x < 2.0f / d1)
        {
            return n1 * (x -= 1.5f / d1) * x + 0.75f;
        }
        else if (x < 2.5f / d1)
        {
            return n1 * (x -= 2.25f / d1) * x + 0.9375f;
        }
        else
        {
            return n1 * (x -= 2.625f / d1) * x + 0.984375f;
        }
    }
    public static float InOutBounce(float x)
    {
        return x < 0.5f ? (1.0f - OutBounce(1.0f - 2.0f * x)) * 0.5f
            : (1.0f + OutBounce(2.0f * x - 1.0f)) * 0.5f;
    }
}

[Serializable]
public enum Ease
{
    /// <summary>Linear interpolation.</summary>
    None,
    InSine,
    OutSine,
    InOutSine,
    InQuad,
    OutQuad,
    InOutQuad,
    InCubic,
    OutCubic,
    InOutCubic,
    InQuart,
    OutQuart,
    InOutQuart,
    InQuint,
    OutQuint,
    InOutQuint,
    InExpo,
    OutExpo,
    InOutExpo,
    InCirc,
    OutCirc,
    InOutCirc,
    InBack,
    OutBack,
    InOutBack,
    InElastic,
    OutElastic,
    InOutElastic,
    InBounce,
    OutBounce,
    InOutBounce
}
