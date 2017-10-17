using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// script for loading the level with a 2d Colllider(not for buttons)
/// </summary>
public class TransitionLevel : MonoBehaviour {
    /// <summary>
    /// Simply load the level with the specified index
    /// </summary>
    /// <param name="level_num">
    /// Index of the level in the build order
    /// </param>
    public void LoadLevel(int level_num)
    {
        SceneManager.LoadScene(level_num);
    }
    /// <summary>
    /// Once the player clicks on the box, load the scene with the index of 1
    /// </summary>
    private void OnMouseDown()
    {
        SceneManager.LoadScene(1);
    }
}
