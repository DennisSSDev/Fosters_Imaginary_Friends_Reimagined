using UnityEngine;
using UnityEngine.SceneManagement;
/// <summary>
/// Script for changing levels
/// </summary>
public class Launch_Arcade : MonoBehaviour {
    public int scene_to_load;
    /// <summary>
    /// Change the level to the according parameter 
    /// </summary>
    /// <param name="level">
    /// number of the scene in the build order
    /// </param>
    public void LoadLevel(int level)
    {
        SceneManager.LoadScene(level);
    }
}
