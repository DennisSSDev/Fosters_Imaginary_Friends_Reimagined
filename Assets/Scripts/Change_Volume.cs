using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Change_Volume : MonoBehaviour {
    public void ChangeVolume()
    {
        AudioListener.volume = FindObjectOfType<Slider>().value;
    }
    
}
