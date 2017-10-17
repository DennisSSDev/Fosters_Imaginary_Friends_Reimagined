using System.Collections;
using UnityEngine;
/// <summary>
/// Bullet has To check for collision with asteroids, not the asteroid!!!!
/// This Class helps keep track of the bullets that the player fires and will use IEnumerator for determining for ho long will the bullet go
/// plus the delay until the next shot
/// </summary>
public class Bullets : MonoBehaviour {
    /*public start */
    public Vehicle status;//keep a record of the movement script
    public GameObject bullet;
    public Stack stack;
    public AudioClip[] bloo_sounds;
    /*public end*/
    private GameObject obj;
    private float nextFire = 0f;
    private CollisionDetection colission;
    private AudioSource source_shot;
    private const float bul_acc = 0.14f;
    private Vector3 bul_Direction;
    /// <summary>
    /// initialize the bullet stack
    /// initialize the collision reference
    /// grab the sound component for determining what Bloo is going to say
    /// </summary>
    private void Start () {
        stack = new Stack();
        colission = FindObjectOfType<CollisionDetection>();
        source_shot = GetComponent<AudioSource>();
	}
    /// <summary>
    /// if the timer until the next fire passed execute the followng code:
    ///     -give bloo a random sound effect and play it
    ///     -Instantiate the bullet and set his directiona and rotation
    ///     -add him to the stack of bullets
    ///     -start movement Coroutine
    /// </summary>
	private void Update () {
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > nextFire)
        {
            float rand = Random.Range(0f, 1f);
            if (rand<0.1f)
            {
                source_shot.clip = bloo_sounds[1];
            }
            else
            {
                source_shot.clip = bloo_sounds[0];
            }
            source_shot.Play();
            nextFire = Time.time + 1.2f;
            Quaternion bul_Rot = Quaternion.Euler(status.child_Rotation.x, status.child_Rotation.y, status.child_Rotation.z-90f);
            GameObject obj = Instantiate(bullet, status.transform.position, bul_Rot);
            stack.Push(obj);          
            bul_Direction = status.dir;
            StartCoroutine(moveBullet(bul_Direction));
        }
	}
    /// <summary>
    /// This method will run asynchronosly
    /// Will check the latest bullet shot from the stack and move hom to the already specified direction with the acceleration
    /// If we have nothing in the Stack, stop the virtual thread
    /// In the end the Bullet will get destroyed
    /// </summary>
    /// <param name="dir">
    /// The Specified direction of the bullet
    /// </param>
    /// <returns></returns>
    private IEnumerator moveBullet(Vector3 dir)
    {
        GameObject obj = (GameObject)stack.Peek();
        colission.bulRend = obj.GetComponent<SpriteRenderer>();
        while (Time.time<nextFire)
        {
            if (obj != null)
            {
                obj.transform.position += dir * bul_acc;
            }
            else
            {
                StopCoroutine("moveBullet");
            }                      
            yield return null;
        }
        Destroy(obj);
   }
}
