using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class AllTilesBlinker : MonoBehaviour
{
    public Tilemap tilemap;             
    public TileBase targetTile;         
    public float blinkInterval = 0.5f;  

    private float timer = 0f;
    private bool tilesVisible = true;   
    private List<Vector3Int> targetTilePositions = new List<Vector3Int>();
    void Start()
    {
        foreach (Vector3Int pos in tilemap.cellBounds.allPositionsWithin)
        {
            if (tilemap.GetTile(pos) == targetTile)
            {
                targetTilePositions.Add(pos);
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= blinkInterval)
        {
            tilesVisible = !tilesVisible;

            foreach (Vector3Int pos in targetTilePositions)
            {
                tilemap.SetTile(pos, tilesVisible ? targetTile : null);
            }

            timer = 0f;
        }
    }
}
