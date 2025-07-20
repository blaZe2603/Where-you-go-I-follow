using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelmenu : MonoBehaviour
{
    // Start is called before the first frame update
    public void OpenLevel(int levelid)
    {
        SceneManager.LoadScene(levelid);

    }
    public void menu()
    {
        SceneManager.LoadSceneAsync(0);
    }
}
