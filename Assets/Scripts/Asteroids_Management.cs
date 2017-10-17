using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This is an asteroid specific class that determines if they should spawn, where and what direction they go. Also, it 
/// manages the difficulty in the game by increasing the amount of asteroids in the level as you rank up points.
/// This class also secures the directions and accelerations for the asteroids and keeps a reference for them in lists.
/// It als makes sure to wrap the asteroids around the screen and sets a random scale for them on spawn
/// The class also nests a method for spawning two child pizzas
/// </summary>
public class Asteroids_Management : MonoBehaviour {
    /*start of public */
    public GameObject[] options;
    public GameObject asteroid_small;
    public List<GameObject> asteroids = new List<GameObject>();
    public List<Vector3> directions = new List<Vector3>();
    public List<float> acc_Constants = new List<float>();
    public List<bool> rotationDir = new List<bool>();
    public int score = 0;
    public CountDown counter;
    /*end of public*/
    private float ast_Difficulty = 0f;//this will increase as you pay the game
    private int score_dif_Threshold = 500;
    public int total_Asteroids_in_lvl = 0;
    private int max_Allowed = 8;//how many big asteroids are allowed in the game
    private float height;
    private float width;
    private float zRot = 0f;//This variable will hold the rotation of a pizza
	/// <summary>
    /// Use this for setting the width and height of the main camera
    /// </summary>
	private void Start () {
        height = 2f * Camera.main.orthographicSize;
        width = height * Camera.main.aspect;
    }
	/// <summary>
    /// Use this for atual management of the asteroids
    /// if the counter at the beginning of the game ends, start the main loop
    /// if we have less asteroids in the level than the max allowed, spawn till hit the max and start moving them around
    /// if the player exceeds the score threshold add to the difficulty of the game
    /// </summary>
	private void Update () {
        if (!counter.active)//the countdown at the beginning of the game
        {
            if (total_Asteroids_in_lvl < max_Allowed)//allowance
            {
                GameObject asteroid_hold;
                float hold = Random.Range(0f, 3f);
                if (hold < 1f)//here I pick out a random pizza prefab to have a variaty of them in the level 
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
                asteroid_hold.transform.position = GetScreenPosition();//set the position before instantiation
                float scale = Random.Range(0.45f, 0.89f);
                DetermineRotation();//determine whether the pizza will rotate clockwise
                asteroid_hold.transform.localScale = new Vector3(scale, scale, 0f);//set random scale
                directions.Add(GetAsteroidDirection());//get a direction for the specific asteroid
                acc_Constants.Add(GetRandomAcceleration());//get a specific acceleration for the asteroid
                asteroids.Add(Instantiate(asteroid_hold, asteroid_hold.transform.position, Quaternion.identity));//spawn the asteroid
                total_Asteroids_in_lvl++;
            }
            if (asteroids.Count > 0)//as long as we have at least one asteroid in the game, keep them moving
            {
                moveAsteroids(asteroids);//if an asteroid got destroyed spawn
                wrapAsteroids(asteroids);
            }
            if (score > score_dif_Threshold)//increase the difficulty once the threshold is breached 
            {
                score_dif_Threshold = score + 500;
                max_Allowed++;
                ast_Difficulty += 0.003f;
            }
        }
	}
    /// <summary>
    /// Method for making the asteroid move based on the location in the list
    /// </summary>
    /// <param name="asteroids">
    /// Feed the whole list as I'll make sure that all asteroids that are in the level get to move
    /// </param>
    private void moveAsteroids(List<GameObject> asteroids)
    {
        for (int i = 0; i < asteroids.Count; i++)
        {
            asteroids[i].transform.position += directions[i] * acc_Constants[i];//alg for movement
            asteroids[i].transform.rotation = Quaternion.Euler(0f, 0f, zRot);
            if (rotationDir[i])//rotate the pizza according to their specified rotation direction in the rotation method
            {
                zRot += 0.2f;
            }
            else
            {
                zRot -= 0.2f;
            }
        }
    }
    /// <summary>
    /// Method that will spawn children Pizzas from the asteroid that Collided with the projectile
    /// They will have a a little offset to their direction, but will go in the general direction of the parent pizza
    /// Will have a random acceleration
    /// </summary>
    /// <param name="obj">
    /// THe parent Pizza
    /// </param>
    /// <param name="index">
    /// Necessary index from the directions list to determine the apprximate direction of the Child Pizzas
    /// </param>
    /// <returns>
    /// Can return the 2 spawned pizzas as an array of GameObjects
    /// </returns>
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
    }
    /// <summary>
    /// This method simply gives a random location for the pizzas on the borders of the screen
    /// </summary>
    /// <returns>
    /// New Vector position that will be set to the specified pizza once it's outside the border
    /// </returns>
    private Vector3 GetScreenPosition()
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
    /// <summary>
    /// Determines an completely random direction for the pizza
    /// </summary>
    /// <returns>
    /// New direction of the pizza
    /// </returns>
    private Vector3 GetAsteroidDirection()
    {
        Vector3 dir = new Vector3(Random.Range(-1f,1f), Random.Range(-1f,1f), 0f);
        dir.Normalize();
        return dir;
    }
    /// <summary>
    /// Method that Brings a random acceleration in the given range
    /// It will change as the player will do better in the game
    /// </summary>
    /// <returns>
    /// New acceleration float
    /// </returns>
    private float GetRandomAcceleration()
    {
        float acceleraton = Random.Range(0.017f+ast_Difficulty, 0.021f +ast_Difficulty);
        return acceleraton;
    }
    /// <summary>
    /// This method will wrap the pizzas around the screen and will bring them back around with the same speed and direction
    /// </summary>
    /// <param name="asteroids">
    /// The wrap check is going to be done with each pizza member on the active list
    /// </param>
    private void wrapAsteroids(List<GameObject> asteroids)
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
    /// <summary>
    /// a method for quickly determining which direction will the pizza rotate
    /// </summary>
    private void DetermineRotation()
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
