using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The scene to load when the player clicks the start button.")]
    private string sceneToLoad = "BetaLevel";
    public void StartGame()
    {
        Debug.Log("Starting game...");
        SceneManager.LoadScene(sceneToLoad);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game...");
        #if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
        #else
        Application.Quit();
        #endif
    }
}
