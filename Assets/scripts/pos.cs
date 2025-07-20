using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pos : MonoBehaviour
{
    public bool block;
    
    void Start()
    {
        
        block = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("notmovable"))
        {
            block = false;
            Debug.Log("a");
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("notmovable"))
        {
            block = true;
            Debug.Log("b");
        }
    }
    
    

}
