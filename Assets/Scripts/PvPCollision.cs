using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PvPCollision : MonoBehaviour {
    bool collide = false;
    public bool output = false;
    Vector3 radius1;
    Vector3 radius2;
    Vector3 bulRadius;

    public SpriteRenderer bulRend;
    public Animator anim_ref;
    // Use this for initialization
    public GameObject ship;
    public GameObject ship2;
    /// <summary>
    /// Finds Collision using the AABB algorithm
    /// </summary>
    /// <returns>
    /// 
    /// </returns>
    /// 
    private void Update()
    {
        output = CirclesCollision();
        collide = false;
    }
    /// <summary>
    /// Using the circular algorithm for precise collisions
    /// </summary>
    /// <returns></returns>
    public bool CirclesCollision()
    {
        SpriteRenderer rendS = ship.GetComponent<SpriteRenderer>();
        SpriteRenderer rendS2 = ship2.GetComponent<SpriteRenderer>();
        radius1 = (rendS.bounds.extents) / 2;
        radius2 = (rendS2.bounds.extents) / 2;
            if (bulRend != null)
            {
                bulRadius = (bulRend.bounds.extents) / 2;
                if ((bulRadius + radius2).magnitude > (bulRend.bounds.center - rendS2.bounds.center).magnitude)
                {
                ship2.SetActive(false);
                }
            }
        return collide;
    }


}
