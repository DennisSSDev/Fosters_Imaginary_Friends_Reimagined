using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_Management : MonoBehaviour {

    // Use this for initialization
    public Vehicle player;
    public GameObject prefab;
    Bullets bullets;
    float spawnDelay = 0f;
    float moveDelay = 0f;
    float actually_start_working_timer = 0f;
    public bool alive = true;
    public SpriteRenderer arrow_rend;
    private AudioSource badunk;
    void Start()
    {
        badunk = GetComponent<AudioSource>();
    }
	// Update is called once per frame
	void Update () {
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
    private void Blink(GameObject spawnedObject)
    {
        SpriteRenderer renderer = spawnedObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Blinker(renderer, spawnedObject, arrow_rend));
    }

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
