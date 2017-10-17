using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Here I use an API for changin the colors for the pizza prefabs
/// </summary>
public class ChangeColor : MonoBehaviour {
    public SpriteRenderer[] renders;
    /// <summary>
    /// Grab all the sprites and the output from the API and apply the color to the sprites
    /// </summary>
    public void ChangeSpriteColor()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Result");//The tag of the output of the API
        Image rend = obj.GetComponent<Image>();
        foreach (var item in renders)
        {
            item.color = rend.color;
        }
    }
}
