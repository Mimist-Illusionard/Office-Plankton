using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;


public class Menu : MonoBehaviour
{
    public CanvasGroup Transition;

    public void StartGame()
    {
        StartCoroutine(StartTransition());
    }

    private IEnumerator StartTransition()
    {
        while (Transition.alpha < 1)
        {
            Transition.alpha += Time.deltaTime;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(1, LoadSceneMode.Additive);
        yield return null;
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        SceneManager.SetActiveScene(arg0);
        SceneManager.UnloadSceneAsync(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
