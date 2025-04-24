using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader2 : MonoBehaviour
{
    [Tooltip("Build Settings index of the scene to load")]
    public int sceneBuildIndex = 0;

    public void LoadScene2()
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }
}
