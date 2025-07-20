using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tile : MonoBehaviour
{

    private Tilemap tilemap;

    void Start()
    {
        // Get the Tilemap component
        tilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(collision.gameObject.tag == "movable")
            Destroy(gameObject);
        // if (true)
        // {
        //     // Get the collision point
        //     Vector3 hitPosition = Vector3.zero;

        //     foreach (ContactPoint2D hit in collision.contacts)
        //     {
        //         hitPosition = hit.point;
        //         break;
        //     }

        //     // Convert the collision point to a cell position
        //     Vector3Int cellPosition = tilemap.WorldToCell(hitPosition);

        //     // Remove the tile at the cell position
        //     if (tilemap.HasTile(cellPosition))
        //     {
        //         tilemap.SetTile(cellPosition, null); // Remove the tile

        //     }
        // }
    }
}

