using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for changing the volume with the slider in the UI
/// </summary>
public class Change_Volume : MonoBehaviour {
    /// <summary>
    /// Simply apply the changes to the audio listener volume according to what you put on the slider
    /// </summary>
    public void ChangeVolume()
    {
        AudioListener.volume = FindObjectOfType<Slider>().value;
    }
    
}
