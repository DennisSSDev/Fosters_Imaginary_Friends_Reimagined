using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// This is an asteroid specific class that determines should they spawn, where and what direction. Also, it 
/// manages the difficulty in the game by increasing the amount of asteroids in the level as you rank up points.
/// This class also secures the directions and accelerations for the asteroids and keeps a reference for them in lists
/// </summary>
public class Asteroids_Management : MonoBehaviour {

    //use the same list for small and large asteroids, but create a separate method for spawining them, as they have a different sprite and size

    public GameObject[] options;
    public GameObject asteroid_small;
    public List<GameObject> asteroids = new List<GameObject>();
    public List<Vector3> directions = new List<Vector3>();
    public List<float> acc_Constants = new List<float>();
    public List<bool> rotationDir = new List<bool>();
    // public GameObject asteroid_to_Spawn_From = null;
    public CountDown counter;
    private float ast_Difficulty = 0f;//this will increase as ypou pay the game
    public int score = 0;
    private int score_dif_Threshold = 500;
    public int total_Asteroids_in_lvl = 0;
    private int max_Allowed = 8;
    private float height;
    private float width;
    private float zRot = 0f;
	/// <summary>
    /// Use this for setting the width and height of the main camera
    /// </summary>
	private void Start () {
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
    }
	
	// Update is called once per frame
	void Update () {
        if (!counter.active)
        {
            if (total_Asteroids_in_lvl < max_Allowed)
            {
                GameObject asteroid_hold;
                float hold = Random.Range(0f, 3f);
                if (hold < 1f)
                {
                    asteroid_hold = options[0];
                }
                else if (hold < 2f)
                {
                    asteroid_hold = options[1];
                }
                else
                {
                    asteroid_hold = options[2];
                }
                asteroid_hold.transform.position = GetScreenPosition();
                float scale = Random.Range(0.45f, 0.89f);
                DetermineRotation();
                asteroid_hold.transform.localScale = new Vector3(scale, scale, 0f);
                directions.Add(GetAsteroidDirection());
                acc_Constants.Add(GetRandomAcceleration());     
                asteroids.Add(Instantiate(asteroid_hold, asteroid_hold.transform.position, Quaternion.identity));//Random Position around the camera view + random asteroid
                                                                                                                 //give a direction  
                total_Asteroids_in_lvl++;
            }
            if (asteroids.Count > 0)
            {
                moveAsteroids(asteroids);//if an asteroid got destroyed spawn
                wrapAsteroids(asteroids);
            }
            if (score > score_dif_Threshold)
            {
                score_dif_Threshold = score + 500;
                max_Allowed++;
                ast_Difficulty += 0.003f;
            }
        }
	}
    void moveAsteroids(List<GameObject> asteroids)//wrap them around the screen + should move at a constant randomly assigned acceleration 
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            asteroids[i].transform.position += directions[i] * acc_Constants[i];
            asteroids[i].transform.rotation = Quaternion.Euler(0f, 0f, zRot);
            if (rotationDir[i])
            {
                zRot += 0.2f;
            }
            else
            {
                zRot -= 0.2f;
            }
        }
        Debug.Log(rotationDir.Count);
    }
    public GameObject[] SpawnChild(GameObject obj, int index)//activate on collision with the player bullet
    {
        GameObject[] small_spawns = new GameObject[2];
        Vector3 direction = directions[index];
        for (int i = 0; i < small_spawns.Length; i++)
        {
            Vector3 offset = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);
            Vector3 small_dir;
            small_spawns[i] = Instantiate(asteroid_small, obj.transform.position, Quaternion.identity);             
            small_dir = direction + offset;
            float small_acceleraton = Random.Range(0.028f, 0.035f);
            small_dir.Normalize();
            asteroids.Add(small_spawns[i]);
            directions.Add(small_dir);
            acc_Constants.Add(small_acceleraton);
            DetermineRotation();
        }
        
        return small_spawns;
        
        //access the objects direction and acceleration through the corresponding lists
        //Instantiate two child pizzas with smaller size
        //the small pizzas should be added to the list of asteroids so that their collision will also be considered
    }
    Vector3 GetScreenPosition()
    {
        float hold = Random.Range(0f, 4f);
        if (hold < 1f)
        {
            return new Vector3(-width/2f-1f, Random.Range(-height/2f, height/2f), 0f);
        }
        else if (hold < 2f)
        {
            return new Vector3(Random.Range(-width/2f, width/2f), -height/2f-1f, 0f);
        }
        else if(hold < 3f)
        {
            return new Vector3((width/2f)+1f, Random.Range(-height/2f, height/2f), 0f);              
        }
        else
        {
            return new Vector3(Random.Range(-width/2f, width / 2f), (height/2f)+1f, 0f);
        }
    }
    Vector3 GetAsteroidDirection()
    {
        Vector3 dir = new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0f);
        dir.Normalize();
        return dir;
    }
    float GetRandomAcceleration()
    {
        float acceleraton = Random.Range(0.017f+ast_Difficulty, 0.021f +ast_Difficulty);
        return acceleraton;
    }
    void wrapAsteroids(List<GameObject> asteroids)
    {
        foreach (var item in asteroids)
        {
            if (item.transform.position.x > (width / 2f)+1.8f)
            {
                Vector3 _newPos = new Vector3(-1 * item.transform.position.x +1.5f, item.transform.position.y, 0f);
                item.transform.position = _newPos;

            }
            else if (item.transform.position.x < (-1 * width / 2f)-1.8f)
            {
                Vector3 _newPos = new Vector3(-1 * item.transform.position.x - 1.5f, item.transform.position.y, 0f);
                item.transform.position = _newPos;
            }
            else if (item.transform.position.y < (-1 * height / 2f)-1.8f)
            {
               
                Vector3 _newPos = new Vector3(item.transform.position.x, -1 * item.transform.position.y - 1.5f, 0f);
                item.transform.position = _newPos;
            }
            else if (item.transform.position.y > (height / 2f)+1.8f)
            {
                Vector3 _newPos = new Vector3(item.transform.position.x, -1 * item.transform.position.y + 1.5f, 0f);
                item.transform.position = _newPos;
            }

        }

    }
    void DetermineRotation()
    {
        int rot_det = Random.Range(0, 2);
        switch (rot_det)
        {
            case 0:
                rotationDir.Add(true);
                break;
            case 1:
                rotationDir.Add(false);
                break;
            default:
                break;
        }
    }
}
