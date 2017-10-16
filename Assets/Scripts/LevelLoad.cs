using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelLoad : MonoBehaviour {

    // Use this for initialization
    GameObject[] allObjects;
    List<GameObject> actually_Disabled = new List<GameObject>();
    GameObject canvas;
    GameObject player;
    float time_stamp = 1f;
	void Start()
    {
        allObjects = FindObjectsOfType<GameObject>();
        foreach (var item in allObjects)
        {
            if(item.name == "LevelLoad" || item.name == "Main Camera" || item.name == "Background" || item.name == "Canvas")
            {
                continue;
            }
            else if(item.name == "CountDown")
            {
                canvas = item;
            }
            else if(item.name == "Player")
            {
                player = item;
            }
            else
            {
                actually_Disabled.Add(item);
            }
        }

        player.GetComponent<CollisionDetection>().enabled = false;
        player.GetComponent<Vehicle>().enabled = false;
        player.GetComponent<ParticelEmission>().enabled = false;
        foreach (var item in actually_Disabled)
        {
            item.SetActive(false);
        }

    }

    void Update()
    {
        time_stamp -= Time.deltaTime;
        if (time_stamp < 0)
        {
            TextMeshProUGUI text =  canvas.GetComponent<TextMeshProUGUI>();
            int num = int.Parse(text.text);
            if(num == 0)
            {
                player.GetComponent<CollisionDetection>().enabled = true;
                player.GetComponent<Vehicle>().enabled = true;
                player.GetComponent<ParticelEmission>().enabled = true;
                foreach (var item in actually_Disabled)
                {
                    item.SetActive(true);
                }
                
                Destroy(this);

            }
            num--;
            text.text = num.ToString();
            time_stamp = Time.deltaTime + 1f;
        }
    }
}
