using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticelEmission : MonoBehaviour {


    public GameObject particles;
    public Spawn_Management spwn_Ref;
    List<GameObject> sys = new List<GameObject>();
    bool canPlay = false;
    GameObject shipLoc;
    float timer = 5f;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        
        if (spwn_Ref.alive == false)
        {
            canPlay = true;
        }
        
        if (canPlay)
        {
            Instantiate(particles, spwn_Ref.player.transform.position, Quaternion.identity);
            
            canPlay = false;
        }
        FindAndDestroy();
	}

    void FindAndDestroy()
    {
        GameObject[] allParticleEmitters = GameObject.FindGameObjectsWithTag("Particles");
        for (int i = 0; i < allParticleEmitters.Length; i++)
        {
            if (!allParticleEmitters[i].GetComponentInChildren<ParticleSystem>().IsAlive())
            {
                Destroy(allParticleEmitters[i]);
            }
        }
    }
}
