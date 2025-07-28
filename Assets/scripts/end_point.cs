using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class end_point : MonoBehaviour
{
    public int button_count = 0;
    [SerializeField] int required_count;
    private BoxCollider2D col;
    private bool isActivated = false;
    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        if (col != null)
            col.enabled = false;
    }

    // Update is called once per frame
        void Update()
    {
        if (button_count >= required_count)
        {
            col.enabled = true;
        }
    }
}
