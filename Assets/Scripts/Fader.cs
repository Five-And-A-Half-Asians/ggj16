using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class Tween
{
    public enum tweenColor
    {
        BLACK, WHITE
    };
    public enum tweenMode
    {
        FADE_IN, FADE_OUT
    };
    public tweenColor tc;
    public tweenMode tm;

    public Tween(tweenColor tc, tweenMode tm)
    {
        this.tc = tc;
        this.tm = tm;
    }
}

public class Fader : MonoBehaviour
{

    public bool active = false;
    public GameObject solid;
    public Color c;
    public float delta;
    public float initialAlpha;
    public float amin = 0f;
    public float amax = 1f;

    public void SetTween(Tween t, float delta)
    {
        this.delta = (t.tm.Equals(Tween.tweenMode.FADE_IN)) ? -delta : delta;
        initialAlpha = (t.tm.Equals(Tween.tweenMode.FADE_IN)) ? 1f : 0f;
        c = (t.tc.Equals(Tween.tweenColor.BLACK)) ? Color.black : Color.white;
        solid.GetComponent<Image>().color = new Color(c.r, c.g, c.b, initialAlpha);
        active = true;
    }

    public void SetTween(Color c, Tween.tweenMode tm, float delta)
    {
        this.delta = (tm.Equals(Tween.tweenMode.FADE_IN)) ? -delta : delta;
        initialAlpha = (tm.Equals(Tween.tweenMode.FADE_IN)) ? 1f : 0f;
        this.c = c;
        solid.GetComponent<Image>().color = new Color(c.r, c.g, c.b, initialAlpha);
        active = true;
    }

    public void SetTween(Color c, float initialAlpha, float amin, float amax, Tween.tweenMode tm, float delta)
    {
        this.delta = (tm.Equals(Tween.tweenMode.FADE_IN)) ? -delta : delta;
        this.initialAlpha = initialAlpha;
        this.c = c;
        solid.GetComponent<Image>().color = new Color(c.r, c.g, c.b, initialAlpha);
        this.amin = amin;
        this.amax = amax;
        active = true;
    }

    void Update()
    {
        if (!active)
            return;
        float nextAlpha = solid.GetComponent<Image>().color.a + delta * Time.deltaTime;
        if (outOfRange(nextAlpha))
        {
            amin = 0f;
            amax = 1f;
            active = false;
        }
        else {
            solid.GetComponent<Image>().color = new Color(c.r, c.g, c.b, nextAlpha);
        }
    }

    bool outOfRange(float a)
    {
        return a < amin || a > amax || a < 0f || a > 1.0f;
    }
}
