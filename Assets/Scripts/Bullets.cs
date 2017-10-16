using System.Collections;
using System.Collections.Generic;
using UnityEngine;



/// <summary>
/// Bullet has To check for collision with asteroids, not the asteroid!!!!
/// </summary>
public class Bullets : MonoBehaviour {
    public Vehicle status;
    public GameObject bullet;
    const float bul_acc = 0.14f;
    Vector3 bul_Direction;
    public Stack stack;
    GameObject obj;
    float nextFire = 0f;
    public AudioClip[] bloo_sounds;
    CollisionDetection colission;
    private AudioSource source_shot;
	// Use this for initialization
	void Start () {
        stack = new Stack();
        colission = FindObjectOfType<CollisionDetection>();
        source_shot = GetComponent<AudioSource>();
	}
	// Update is called once per frame

	void Update () {
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
                //bullet.transform.position = status.velocity;
            bul_Direction = status.dir;
            StartCoroutine(moveBullet(bul_Direction));
        }
	}
    IEnumerator moveBullet(Vector3 dir)
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
