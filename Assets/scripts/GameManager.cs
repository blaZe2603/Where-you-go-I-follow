using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject gameoverUI;
    public GameObject gameoverUI1;
    public bool game = false;
    player player;
    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<player>();
    }
    public void gameover()
    {
        gameoverUI.SetActive(true);
        gameoverUI1.SetActive(false);
        player.p2move = true;
    }
    
}
