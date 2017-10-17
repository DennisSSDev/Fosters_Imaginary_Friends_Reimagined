using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// This class is specifically to spawn the particles of the player when he dies and respawns
/// </summary>
public class ParticelEmission : MonoBehaviour {
    public GameObject particles;
    public Spawn_Management spwn_Ref;
    private List<GameObject> sys = new List<GameObject>();
    private bool canPlay = false;
    private GameObject shipLoc;
    private float timer = 5f;
	
	/// <summary>
    /// Check for player's death, If he is, spawn the particles
    /// Once done, Destroy the particle
    /// </summary>
	private void Update () {
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
    /// <summary>
    /// Method that looks for unused particles in the level and destroys them (According to their tag) 
    /// </summary>
    private void FindAndDestroy()
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
