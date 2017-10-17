using UnityEngine;
/// <summary>
/// Optimized Collision for Multiplayer(In the future will switch to inheritance)
/// </summary>
public class PvPCollision : MonoBehaviour {
    private bool collide = false;
    public bool output = false;
    private Vector3 radius1;
    private Vector3 radius2;
    private Vector3 bulRadius;
    public SpriteRenderer bulRend;
    public Animator anim_ref;
    // Use this for initialization
    public GameObject ship;
    public GameObject ship2;
    /// <summary>
    /// Run the circle algorithm
    /// </summary>
    private void Update()
    {
        output = CirclesCollision();
        collide = false;
    }
    /// <summary>
    /// Using the circular algorithm for precise collisions
    /// In case the bullet of the first palyer collides with the body of the other, deactivate him
    /// </summary>
    /// <returns>
    /// for now, it doesn't return anything substantial, will change to use that bool for elastic bounce between the players
    /// </returns>
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
