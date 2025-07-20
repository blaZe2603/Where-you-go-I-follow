using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class inlevelmenu : MonoBehaviour
{
    
    player2 p_2;
    public GameObject[] objectsToDisable;

    public GameObject level_complete_canva;
    public GameObject level_lost_canva;
    public GameObject targetObject;  // drag in Inspector


    // Start is called before the first frame update
    void Start()
    {
        p_2 = targetObject.GetComponent<player2>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(p_2.win)
        {
            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
            level_complete_canva.SetActive(true);
        }
        if(p_2.lose)
        {
            foreach (GameObject obj in objectsToDisable)
            {
                obj.SetActive(false);
            }
            level_lost_canva.SetActive(true);
        }
    }

    
    public void menu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void level_menu()
    {
        SceneManager.LoadSceneAsync(1);
    }
    public void restart()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
    public void next_level()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1);

    }
    public void prev_level()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex -1);
    }
    public void OpenLevel(int levelid)
    {
        SceneManager.LoadScene(levelid);

    }
    public void StopGame()
    {
        Application.Quit();
    }


}
