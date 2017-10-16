using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetection : MonoBehaviour {
    bool collide = false;
    public bool output = false;
    Vector3 radius1;

    private Asteroids_Management asteroid_Ref;
    Vector3 bulRadius;

    public GameObject deadPiz;
    public SpriteRenderer bulRend;
    public Animator anim_ref;
    // Use this for initialization
    public GameObject ship;
    public Asteroids_Management ast_Ref;
    private Stack<GameObject> deadPizzas = new Stack<GameObject>();
    private AudioSource snap;
    float time_before_part_removal = 0f;
    bool allowedDestruction = false;
    /// <summary>
    /// Finds Collision using the AABB algorithm
    /// </summary>
    /// <returns></returns>
    /// 
    private void Start()
    {
        asteroid_Ref = FindObjectOfType<Asteroids_Management>();
        snap = GetComponent<AudioSource>();
    }
    private void Update()
    {
       output = CirclesCollision();
       collide = false;
       removeParticles();
    }
    /// <summary>
    /// Using the circular algorithm for precise collisions
    /// </summary>
    /// <returns></returns>
    public bool CirclesCollision()
    {
        
        SpriteRenderer rendS = ship.GetComponent<SpriteRenderer>();
        List<SpriteRenderer> astRenders = new List<SpriteRenderer>();
        for (int i = 0; i < ast_Ref.asteroids.Count; i++)
        {
            astRenders.Add(ast_Ref.asteroids[i].GetComponent<SpriteRenderer>());
        }
        List<Vector3> radii = new List<Vector3>();
        radius1 = (rendS.bounds.extents)/2;
        for (int i = 0; i < ast_Ref.asteroids.Count; i++)
        {
            radii.Add(astRenders[i].bounds.extents / 2);
        }
        for (int i = 0; i < ast_Ref.asteroids.Count; i++)
        {
            if (ast_Ref.asteroids.Count <= i || radii.Count <= i)
            {
                continue;
            }
            if (bulRend != null)
            {
                bulRadius = (bulRend.bounds.extents)/ 2;
                if ((bulRadius + radii[i]).magnitude > (bulRend.bounds.center - astRenders[i].bounds.center).magnitude)
                {
                    Destroy((GameObject)FindObjectOfType<Bullets>().stack.Peek());

                    if (radii[i].magnitude > 0.5f)
                    {
                        asteroid_Ref.SpawnChild(ast_Ref.asteroids[i], i);
                        ast_Ref.score += 20;
                        ast_Ref.total_Asteroids_in_lvl--;
                    }
                    else
                    {
                        ast_Ref.score += 50;
                    }
                    anim_ref.Play(0);
                    deadPizzas.Push(Instantiate(deadPiz, ast_Ref.asteroids[i].transform.position, Quaternion.Euler(0f,0f,0f)));
                    time_before_part_removal = Time.time + 1f;
                    allowedDestruction = true;
                    Destroy(ast_Ref.asteroids[i]);            
                    ast_Ref.asteroids.RemoveAt(i);
                    ast_Ref.directions.RemoveAt(i);
                    ast_Ref.acc_Constants.RemoveAt(i);
                    ast_Ref.rotationDir.RemoveAt(i);
                    radii.RemoveAt(i);
                    astRenders.RemoveAt(i);
                    snap.Play();
                }
            }
            if (astRenders.Count <= i)
            {
                continue;
            }
            if ((radius1 + radii[i]).magnitude > (rendS.bounds.center - astRenders[i].bounds.center).magnitude)
            {
                collide = true;
                Debug.Log("Hit object");
                return collide;
            }
        }
        return collide;
    }
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
