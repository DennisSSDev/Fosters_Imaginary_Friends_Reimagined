using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionLevel : MonoBehaviour {
    public void LoadLevel(int level_num)
    {
        SceneManager.LoadScene(level_num);
    }
    void OnMouseDown()
    {
        SceneManager.LoadScene(1);
    }
}
