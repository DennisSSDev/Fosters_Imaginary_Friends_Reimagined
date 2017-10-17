using UnityEngine;
using TMPro;//Asset used for better UI text
/// <summary>
/// this script is used primarily to start a timer and to kill it as soon as Time.time becomes larger than the arbitrary timer
/// </summary>
public class CountDown : MonoBehaviour {

    public GameObject countdown;
    public bool active = false;
    private TextMeshProUGUI guiElement_;
    private float timer;
    /// <summary>
    /// Make sure to set the timer
    /// Grab the component that will showcase the time on screen as I'll be changing it according to the timer
    /// </summary>
    private void Start () {
        guiElement_ = countdown.GetComponent<TextMeshProUGUI>();
        active = true;
        timer = 1f;
	}
	/// <summary>
    /// Run the update loop until the countdown num is less than 0
    /// Once it's below, destroy this script
    /// </summary>
	private void Update () {
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
