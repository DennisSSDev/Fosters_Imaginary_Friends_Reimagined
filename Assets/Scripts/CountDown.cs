using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



/// <summary>
/// this script is used primarily to start a timer and to kill it as soon as Time.time becomes larger than the arbitrary timer
/// </summary>
public class CountDown : MonoBehaviour {

    public GameObject countdown;
    private TextMeshProUGUI guiElement_;
    public bool active = false;
    private float timer;
    

    /// <summary>
    /// Make sure to set the timer
    /// Grab the component that will showcase the time on screen as I'll be changing it according to the timer
    /// </summary>
    void Start () {
        guiElement_ = countdown.GetComponent<TextMeshProUGUI>();
        active = true;
        timer = 1f;
	}
	
	/// <summary>
    /// Run the update loop until the countdown num is less than 0
    /// Once it's below, destroy this script
    /// </summary>
	void Update () {
        if (active == false)
        {
            Destroy(countdown);
        }
        if (Time.time > timer)
        {
            timer = Time.time + 1f;
            int num = int.Parse(guiElement_.text);//make sure to parse the data to edit it as an int
            num--;
            if (num < 0)
            {
                active = false;
                return;
            }
            guiElement_.text = num.ToString();
        }
	}
}
