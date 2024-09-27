using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class LevelGenerator : MonoBehaviour
{
    int[,] levelMap =
    {
        {1,2,2,2,2,2,2,2,2,2,2,2,2,7},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,4},
        {2,6,4,0,0,4,5,4,0,0,0,4,5,4},
        {2,5,3,4,4,3,5,3,4,4,4,3,5,3},
        {2,5,5,5,5,5,5,5,5,5,5,5,5,5},
        {2,5,3,4,4,3,5,3,3,5,3,4,4,4},
        {2,5,3,4,4,3,5,4,4,5,3,4,4,3},
        {2,5,5,5,5,5,5,4,4,5,5,5,5,4},
        {1,2,2,2,2,1,5,4,3,4,4,3,0,4},
        {0,0,0,0,0,2,5,4,3,4,4,3,0,3},
        {0,0,0,0,0,2,5,4,4,0,0,0,0,0},
        {0,0,0,0,0,2,5,4,4,0,3,4,4,0},
        {2,2,2,2,2,1,5,3,3,0,4,0,0,0},
        {0,0,0,0,0,0,5,0,0,0,4,0,0,0},
    };
    public Tilemap tilemap;

    public Tile tile0_Empty;
    public Tile tile1_OutsideCorner;
    public Tile tile2_OutsideWall;
    public Tile tile3_InsideCorner;
    public Tile tile4_InsideWall;
    public Tile tile5_StandardPellet;
    public Tile tile6_PowerPellet;
    public Tile tile7_TJunction;

    private Tile[] tiles;
    private Quaternion currentRotation;
    // Start is called before the first frame update
    void Start()
    {
        //tilemap.ClearAllTiles();
        //tiles = new Tile[8] { tile0_Empty, tile1_OutsideCorner, tile2_OutsideWall, tile3_InsideCorner, tile4_InsideWall, tile5_StandardPellet, tile6_PowerPellet, tile7_TJunction };

        //for (int i = 0; i < levelMap.GetLength(0); i++)
        //{
        //    for (int j = 0; j < levelMap.GetLength(1); j++)
        //    {
        //        Vector3Int tilePosition = new Vector3Int(-14 + j, 13 - i, 0);
        //        tilemap.SetTile(tilePosition, tiles[levelMap[i, j]]);

        //        Quaternion currentRotation = getTheRightRirection(i, j);
        //        SetTileRotation(tilePosition, currentRotation);
        //        if (levelMap[i, j] == 7)
        //        {
        //            if (isOut(i - 1, j) && isOut(i, j + 1))
        //            {
        //                //SetTileHorizontalFlip(tilePosition);
        //            }
        //        }
        //    }
        //}

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Helper method to set tile rotation
    private void SetTileRotation(Vector3Int position, Quaternion rotation)
    {
        Matrix4x4 tileMatrix = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);
        tilemap.SetTransformMatrix(position, tileMatrix);
    }

    bool isEmpty (int i, int j)
    {
        try
        {
            return levelMap[i, j] == 0 || levelMap[i, j] == 5 || levelMap[i, j] == 6;
        } catch (IndexOutOfRangeException)
        {
            return false;
        }
        
    }

    bool isWall (int i, int j)
    {
        try
        {
            return levelMap[i, j] == 1 || levelMap[i, j] == 2 || levelMap[i, j] == 3 || levelMap[i, j] == 4;
        }
        catch (IndexOutOfRangeException)
        {
            return false;
        }
    }

    bool isOut (int i, int j)
    {
        try
        {
            int temp = levelMap[i, j];
            return false;
        }
        catch (IndexOutOfRangeException)
        {
            return true;
        }
    }

    Quaternion getTheRightRirection(int i, int j)
    {
        if (levelMap[i, j] == 1)
        {
            if (isWall(i, j-1) && isWall(i-1, j))
            {
                return Quaternion.Euler(0, 0, -90);
            }
            else if (isWall(i-1, j) && isWall(i, j + 1))
            {
                return Quaternion.Euler(0, 0, 180);
            }
            else if (isWall(i, j + 1) && isWall(i + 1, j))
            {
                return Quaternion.Euler(0, 0, 90);
            }
        }
        if (levelMap[i, j] == 2 || levelMap[i, j] == 4)
        {
            if (isEmpty(i-1, j) || isEmpty(i + 1, j))
            {
                return Quaternion.Euler(0, 0, 90);
            }
        }
        if (levelMap[i, j] == 3)
        {
            if (isEmpty(i, j - 1) && isEmpty(i - 1, j))
            {
                return Quaternion.Euler(0, 0, 90);
            }
            else if (isEmpty(i, j - 1) && isEmpty(i + 1, j))
            {
                return Quaternion.Euler(0, 0, 180);
            }
            else if (isEmpty(i, j + 1) && isEmpty(i + 1, j))
            {
                return Quaternion.Euler(0, 0, -90);
            }
        }
        if (levelMap[i, j] == 7)
        {
            if (isOut(i, j - 1) && isOut(i + 1, j))
            {
                return Quaternion.Euler(0, 0, 90);
            }
        }
        return Quaternion.Euler(0, 0, 0);
    }

    public void SetTileHorizontalFlip(Vector3Int position)
    {
        Matrix4x4 flipMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1, 1, 1));

        tilemap.SetTransformMatrix(position, flipMatrix);
    }
}
