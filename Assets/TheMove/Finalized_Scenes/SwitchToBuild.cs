using UnityEngine;
using UnityEngine.SceneManagement;

public class SimpleSceneLoader : MonoBehaviour
{
    [Tooltip("Build Settings index of the scene to load")]
    public int sceneBuildIndex = 1;

    public void LoadScene()
    {
        SceneManager.LoadScene(sceneBuildIndex);
    }
}
