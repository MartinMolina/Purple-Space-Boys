using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void CloseGame()
    {
        Application.Quit();
    }

    public void DisableObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }

    public void EnableObject(GameObject gameObject)
    {
        gameObject.SetActive(true);
    }

}