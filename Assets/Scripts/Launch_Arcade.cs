using UnityEngine;
using UnityEngine.SceneManagement;
public class Launch_Arcade : MonoBehaviour {


    public int scene_to_load;
    
    public void LoadLevel()
    {
        SceneManager.LoadScene(scene_to_load);
    }
}
