using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class will check the collision for the bullet and the player against all the asteroids in the level
/// It will also add points to the player when his bullet collides with an asteroid
/// </summary>
public class CollisionDetection : MonoBehaviour {
    /*public start*/
    public bool output = false;
    public GameObject deadPiz;
    public SpriteRenderer bulRend;
    public Animator anim_ref;
    public GameObject ship;
    public Asteroids_Management ast_Ref;
    /*public end*/
    private Stack<GameObject> deadPizzas = new Stack<GameObject>();
    private AudioSource snap;
    private float time_before_part_removal = 0f;
    private bool allowedDestruction = false;
    private Vector3 radius1;
    private Asteroids_Management asteroid_Ref;
    private Vector3 bulRadius;
    private bool collide = false;
    /// <summary>
    /// Keep a reference of the asteroid manager script
    /// Sound effect that will be used whenever the bullet collides with a Pizza
    /// </summary>
    private void Start()
    {
        asteroid_Ref = FindObjectOfType<Asteroids_Management>();
        snap = GetComponent<AudioSource>();
    }
    /// <summary>
    /// Run the CircleCollision Algorithm
    /// REmove any ununesd particles from the level
    /// </summary>
    private void Update()
    {
       output = CirclesCollision();
       collide = false;
       removeParticles();
    }
    /// <summary>
    /// Using the circular algorithm for precise collisions with the player and the bullets against all pizza, both small and large
    /// delete the pizza that the bullet collides with 
    /// </summary>
    /// <returns>
    /// Return the result of whether the player collides with any pizza or not
    /// </returns>
    public bool CirclesCollision()
    {
        SpriteRenderer rendS = ship.GetComponent<SpriteRenderer>();//Keep a sprite ref for calculating the radius
        List<SpriteRenderer> astRenders = new List<SpriteRenderer>();//list of all sprites of pizzas
        for (int i = 0; i < ast_Ref.asteroids.Count; i++)
        {
            astRenders.Add(ast_Ref.asteroids[i].GetComponent<SpriteRenderer>());//get the sprites
        }
        List<Vector3> radii = new List<Vector3>();//all radii of the pizzas
        radius1 = (rendS.bounds.extents)/2;
        for (int i = 0; i < ast_Ref.asteroids.Count; i++)
        {
            radii.Add(astRenders[i].bounds.extents / 2);
        }
        for (int i = 0; i < ast_Ref.asteroids.Count; i++)//main check is inside this loop
        {
            if (ast_Ref.asteroids.Count <= i || radii.Count <= i)
            {
                continue;
            }
            if (bulRend != null)
            {
                bulRadius = (bulRend.bounds.extents)/ 2;
                if ((bulRadius + radii[i]).magnitude > (bulRend.bounds.center - astRenders[i].bounds.center).magnitude)//Check with bullet colliding
                {
                    Destroy((GameObject)FindObjectOfType<Bullets>().stack.Peek());//Destroy the latest bullet
                    if (radii[i].magnitude > 0.5f)//add points in here for large asteroids
                    {
                        asteroid_Ref.SpawnChild(ast_Ref.asteroids[i], i);//Spawn the child pizzas on top of the just destroyed parent
                        ast_Ref.score += 20;
                        ast_Ref.total_Asteroids_in_lvl--;
                    }//add points here for CHild asteriods
                    else
                    {
                        ast_Ref.score += 50;
                    }
                    anim_ref.Play(0);//play the adding to score animation
                    deadPizzas.Push(Instantiate(deadPiz, ast_Ref.asteroids[i].transform.position, Quaternion.Euler(0f,0f,0f)));
                    time_before_part_removal = Time.time + 1f;
                    allowedDestruction = true;
                    Destroy(ast_Ref.asteroids[i]);//Remove all references of the just destroyed asteroid, whether small or large            
                    ast_Ref.asteroids.RemoveAt(i);
                    ast_Ref.directions.RemoveAt(i);
                    ast_Ref.acc_Constants.RemoveAt(i);
                    ast_Ref.rotationDir.RemoveAt(i);
                    radii.RemoveAt(i);
                    astRenders.RemoveAt(i);
                    snap.Play();//play crunch sound effect
                }
            }
            if (astRenders.Count <= i)
            {
                continue;
            }
            if ((radius1 + radii[i]).magnitude > (rendS.bounds.center - astRenders[i].bounds.center).magnitude)//Check here for pizza to player collision
            {
                collide = true;
                return collide;
            }
        }
        return collide;
    }
    /// <summary>
    /// This method is called to remove particles that appear after the pizza was hit by a bullet 
    /// (Y doesn't unity have a garbage collector for that sort of stuff ;(((( )
    /// </summary>
    private void removeParticles()
    {
        if (Time.time > time_before_part_removal && allowedDestruction)
        {     
            Destroy(deadPizzas.Peek());
            deadPizzas.Pop();
            allowedDestruction = false;
        }
        
    }
}
