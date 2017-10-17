using UnityEngine;
/// <summary>
/// This is class for simply fading into a scene
/// </summary>
public class FadeIn : MonoBehaviour {
    public Texture2D fadeOutTexture;
    public float fadeSpeed = 0.8f;
    private int drawDepth = -1000;
    private float alpha = 1.0f;
    private int fadeDir = -1;
    /// <summary>
    /// The method that will determine the fade strarting color
    /// Pretty much covers up the entire screen with iteslf
    /// </summary>
    private void OnGUI()
    {
        alpha += fadeDir * fadeSpeed * Time.deltaTime;
        alpha = Mathf.Clamp01(alpha);
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
        GUI.depth = drawDepth;
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
    }
    /// <summary>
    /// This will begin the fade into lightness
    /// </summary>
    /// <param name="direction">
    /// Whether we're going to gor from light to dark or dark to light
    /// </param>
    /// <returns></returns>
    public float BeginFade(int direction)
    {
        fadeDir = direction;
        return (fadeSpeed);
    }
    /// <summary>
    /// Call the fading once the level loads
    /// </summary>
    private void OnLevelWasLoaded()
    {
        BeginFade(-1);
    }
}
