using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
//Class for fading out
public class FadeOut : MonoBehaviour {
   private bool allowed = false;
	// Use this for initialization
	private void Start () {
        StartCoroutine(PerformFade());
	}
    /// <summary>
    /// A coroutine that would perform the fade out and load the next scende once it's done
    /// </summary>
    /// <returns></returns>
    private IEnumerator PerformFade()
    {
        yield return new WaitForSeconds(3f);
        float fadeTime = GetComponent<FadeIn>().BeginFade(1);
        yield return new WaitForSeconds(fadeTime+2f);//Wait for this amount of time and continue running
        SceneManager.LoadScene(1);
    }

}
