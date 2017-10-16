using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
public class GUI_Management : MonoBehaviour {
    enum PlayState
    {
        Start,
        InPlay,
        Dead
    }

    PlayState state = new PlayState();

    // Use this for initialization
    public GameObject[] lives;
    public GameObject veh;
    public GameObject points;
    public Asteroids_Management ast_Ref;
    TextMeshProUGUI nums;
    public CollisionDetection ref_col;

    public GameObject textResult1;
    public GameObject textResult2;

    public GameObject gameover_Screen;

    int life_count = 2;
    int score = 0;
    float delay = 0f;
	void Start () {
        nums = points.GetComponent<TextMeshProUGUI>();
        state = PlayState.InPlay;
        

    }
	
	// Update is called once per frame
	void Update () {
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
    private void DisableMovement()
    {
        veh.SetActive(false);
    }
    private void RecordScore(int score)
    {
        string[] numbers;
        int[] scores;
        try
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
    private void DisplayScores(int score)
    {
        TextMeshProUGUI score1_ = textResult1.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI score2_ = textResult2.GetComponent<TextMeshProUGUI>();

        score1_.text = score.ToString();
        using (StreamReader stream = File.OpenText("scores.txt"))
        {
            string[] results = stream.ReadLine().Split('|');
            score2_.text = results[0];
        }
    }
}
