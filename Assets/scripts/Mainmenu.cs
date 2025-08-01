using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject option;

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void StopGame()
    {
        Application.Quit();
    }

    public void Options()
    {
        menu.SetActive(false);
        option.SetActive(true);
    }
    public void back()
    {
        menu.SetActive(true);
        option.SetActive(false);
    }
}