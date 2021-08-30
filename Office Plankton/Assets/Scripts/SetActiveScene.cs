using UnityEngine;
using UnityEngine.SceneManagement;


public class SetActiveScene : MonoBehaviour
{   
    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Game"));
        Destroy(gameObject);
    }
}
