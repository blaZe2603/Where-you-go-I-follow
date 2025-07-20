using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridManager : MonoBehaviour
{
    [SerializeField] int width, length;
    [SerializeField] private tile tileprefab;

    private void Start()
    {
        makegrid();
    }
    void makegrid()
    {
        for (int i = 0; i < width; i++) {
            for (int j = 0; j < length; j++)
            {
                var spawnedTile =  Instantiate(tileprefab, new Vector3(i - width/2, j - length/2), Quaternion.identity);
                spawnedTile.name = $"Tile {i} {j} ";

                var isoffset = (i % 2 == 0 && j % 2 != 0) || (j % 2 == 0 && i % 2 != 0);
            }
       
        }

    }
}
