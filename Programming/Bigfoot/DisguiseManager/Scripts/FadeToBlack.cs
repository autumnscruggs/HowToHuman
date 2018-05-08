using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeToBlack : MonoBehaviour
{
    public Texture2D fadeTexture;

    [Range(0.1f, 1f)]
    public float fadeSpeed;
    public int drawDepth = -1000;

    [SerializeField] private float alpha = 1.0f;
    private int fadeDir = -1;

    private bool isFading = true;

    private void Awake()
    {
        isFading = true;
    }

    private void OnGUI()
    {
        if(isFading)
        {
            alpha -= fadeDir * fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            Color thisAlpha = GUI.color;
            thisAlpha.a = alpha;
            GUI.color = thisAlpha;

            GUI.depth = drawDepth;
            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
        }
    }

    public void FadeOut()
    {
        isFading = true;
    }
}
