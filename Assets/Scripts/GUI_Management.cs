using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

/// <summary>
/// This class helps show the neccessary GUI elements like health, score and game over screens 
/// Also it holds the highscore component that will be written to a text file
/// </summary>
public class GUI_Management : MonoBehaviour {
    /// <summary>
    /// PLayer states
    /// </summary>
    enum PlayState
    {
        Start,
        InPlay,
        Dead
    }
    PlayState state = new PlayState();
    // Use this for initialization
    /*start of public*/
    public GameObject[] lives;
    public GameObject veh;
    public GameObject points;
    public Asteroids_Management ast_Ref;
    public CollisionDetection ref_col;
    public GameObject textResult1;
    public GameObject textResult2;
    public GameObject gameover_Screen;
    /*end of public*/
    private TextMeshProUGUI nums;
    private int life_count = 2;
    private int score = 0;
    private float delay = 0f;
    /// <summary>
    /// Change the player state as he just spawned 
    /// grab the text GUI for displaying the points
    /// </summary>
	private void Start () {
        nums = points.GetComponent<TextMeshProUGUI>();
        state = PlayState.InPlay;
    }
	/// <summary>
    /// The general loop for reducing the player health GUI points when he gets hit
    /// Show Game Over menu once died 3 times
    /// End the countdown
    /// Increment the scores
    /// </summary>
	private void Update () {
        if(life_count < 0 && state != PlayState.Dead)
        {
            state = PlayState.Dead;
            DisableMovement();
            gameover_Screen.SetActive(true);
            RecordScore(score);
            DisplayScores(score);
        }
        if(ref_col.output && delay < Time.time && state != PlayState.Dead)
        {
            delay = Time.time + 3f;
            Destroy(lives[life_count]);
            lives[life_count] = null;
            life_count--;
        }
        score = ast_Ref.score;
        if (score > int.Parse(nums.text))
        {
            nums.text = score.ToString();
        }
	}
    /// <summary>
    /// Disables movement script of the player
    /// </summary>
    private void DisableMovement()
    {
        veh.SetActive(false);
    }
    /// <summary>
    /// The method that will grab the current score and compare it to an existing txt file
    /// if the file doesn't exist, create a new one with the current score
    /// Else, if the current score is greater than the one present in the text file, replace it
    /// if not, keep the old one
    /// </summary>
    /// <param name="score">
    /// THe score that the player achieved during gameplay
    /// </param>
    private void RecordScore(int score)
    {
        string[] numbers;
        int[] scores;
        try//In case of an exception I've put it into a try parse if the file stuff gets totally rekt
        {
            using (StreamReader stream = File.OpenText("scores.txt"))
            {
                numbers = stream.ReadLine().Split('|');
                scores = new int[numbers.Length];
            }
        }     
        catch (System.Exception)
        {
            using (StreamWriter str = File.CreateText("scores.txt"))
            {
                str.WriteLine(score + "|");
            }
            return;
        }     
            int.TryParse(numbers[0], out scores[0]);

            if (score>scores[0])
            {
                using (StreamWriter wrt = new StreamWriter("scores.txt"))
                {
                    wrt.WriteLine(score+"|");               
                }
                return;
            }    
    }
    /// <summary>
    /// Grab teh info from the text file and and display it on the game over tab
    /// Also display the current score, whether if it's less or greater the highscore
    /// </summary>
    /// <param name="score">
    /// Currently earned player score
    /// </param>
    private void DisplayScores(int score)
    {
        TextMeshProUGUI score1_ = textResult1.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI score2_ = textResult2.GetComponent<TextMeshProUGUI>();
        score1_.text = score.ToString();
        using (StreamReader stream = File.OpenText("scores.txt"))
        {
            string[] results = stream.ReadLine().Split('|');//split the data
            score2_.text = results[0];
        }
    }
}
