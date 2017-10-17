using System.Collections;
using UnityEngine;
/// <summary>
/// this script is used specifically for player respawns
/// </summary>
public class Spawn_Management : MonoBehaviour {
    public Vehicle player;
    public GameObject prefab;
    private Bullets bullets;
    private float spawnDelay = 0f;
    private float moveDelay = 0f;
    private float actually_start_working_timer = 0f;
    public bool alive = true;
    public SpriteRenderer arrow_rend;
    private AudioSource badunk;
    /// <summary>
    /// Sound effect that will play when the player dies and respawns
    /// </summary>
    private void Start()
    {
        badunk = GetComponent<AudioSource>();
    }
	/// <summary>
    /// If the player died, execute the following orders:
    ///     -disable the player body
    ///     -center him
    ///     -play sound
    ///     -blink until timer runs out and reenable the object
    /// </summary>
	private void Update () {
        if (!alive)
        {
            arrow_rend.enabled = false;
            spawnDelay = Time.time + 4f;
            moveDelay = Time.time + 1f;
            player.self.GetComponent<CollisionDetection>().enabled = false;
            bullets = FindObjectOfType<Bullets>();
            bullets.enabled = false;
            float height = 2f * Camera.main.orthographicSize;
            float width = height * Camera.main.aspect;
            player.self.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 10f));
            alive = true;
            badunk.Play();
            Blink(player.self);                  
        }
	}
    /// <summary>
    /// Pass on the object sprite that will be blinking
    /// </summary>
    /// <param name="spawnedObject">
    /// Player that died
    /// </param>
    private void Blink(GameObject spawnedObject)
    {
        SpriteRenderer renderer = spawnedObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Blinker(renderer, spawnedObject, arrow_rend));
    }
    /// <summary>
    /// This Virtual Thread will blink the player's sprite until the timer runs out 
    /// Once he's done, renable the player and stop the blinking
    /// </summary>
    /// <param name="renderer">
    /// Player's render
    /// </param>
    /// <param name="obj">
    /// Player's object
    /// </param>
    /// <param name="arrow_rend">
    /// The arrow of where the player points at
    /// </param>
    /// <returns></returns>
    private IEnumerator Blinker(SpriteRenderer renderer, GameObject obj, SpriteRenderer arrow_rend)
    {
        renderer.color = Color.grey;
        obj.GetComponent<Vehicle>().enabled = false;
        renderer.enabled = false;
        while (Time.time < moveDelay)
        {
            yield return new WaitForSeconds(1f);
        }
        renderer.enabled = true;
        obj.GetComponent<Vehicle>().enabled = true;
        arrow_rend.enabled = true;
        while (Time.time < spawnDelay)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(0.2f);
        }
        renderer.color = Color.white;
        renderer.enabled = true;
        obj.GetComponent<CollisionDetection>().enabled = true;
        bullets.enabled = true;
    }
}
