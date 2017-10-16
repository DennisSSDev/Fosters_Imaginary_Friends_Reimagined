using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class FadeOut : MonoBehaviour {

    bool allowed = false;
	// Use this for initialization
	void Start () {
        StartCoroutine(PerformFade());
	}
    IEnumerator PerformFade()
    {
        yield return new WaitForSeconds(3f);
        float fadeTime = GetComponent<FadeIn>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime+2f);
        SceneManager.LoadScene(1);
    }

}
