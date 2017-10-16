using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChangeColor : MonoBehaviour {
    public SpriteRenderer[] renders;
    public void ChangeSpriteColor()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Result");
        Image rend = obj.GetComponent<Image>();
        foreach (var item in renders)
        {
            item.color = rend.color;
        }
        Debug.Log(renders[0].color);
    }
}
