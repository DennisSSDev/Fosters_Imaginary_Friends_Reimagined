using System.Collections;
using UnityEngine;
/// <summary>
/// Optimized version of the bullet class for PvP shooting
/// Post mortem: I kinda realized that using inheritance would've been great with overriding some methods,
/// will do that in the future
/// </summary>
public class Bullets_PVP : MonoBehaviour
{
    /*start of public*/
    public Player_Script status;
    public GameObject bullet;
    public Stack stack;
    public PvPCollision colission;
    public KeyCode shootButton;
    /*end of public*/
    private const float bul_acc = 0.14f;
    private Vector3 bul_Direction;
    private GameObject obj;
    private float nextFire = 0f;
    /// <summary>
    /// Check Bullet Class
    /// </summary>
    private void Start()
    {
        stack = new Stack();
    }
    /// <summary>
    /// Here we do similar stuff as in Bullet, but the shooting button can be adjusted through the editor
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(shootButton) && Time.time > nextFire)
        {
            nextFire = Time.time + 1.2f;
            Quaternion bul_Rot = Quaternion.Euler(status.child_Rotation.x, status.child_Rotation.y, status.child_Rotation.z - 90f);
            GameObject obj = Instantiate(bullet, status.transform.position, bul_Rot);
            stack.Push(obj);
            //bullet.transform.position = status.velocity;
            bul_Direction = status.dir;
            StartCoroutine(moveBullet(bul_Direction));
        }
    }
    /// <summary>
    /// Check Bullet Class
    /// </summary>
    private IEnumerator moveBullet(Vector3 dir)
    {
        GameObject obj = (GameObject)stack.Peek();
        colission.bulRend = obj.GetComponent<SpriteRenderer>();
        while (Time.time < nextFire)
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
