﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

public class TerrainManager : MonoBehaviour
{
    #region Variables
    [SerializeField] private bool debug = true;

    [Header("Tiles Config")]
    [Tooltip("The Tilemap to draw onto")]
	public Tilemap tilemap;
	[Tooltip("The Tile to draw when buying")]
	public TileBase tilePavement;
    [Tooltip("The Tile to draw when selling")]
	public TileBase tileDirt;

    [Header("Map Properties")]
    [SerializeField] private int mapWidth = 80;
    [SerializeField] private int mapHeight = 80;
    [SerializeField, Min(1)] private int cellSize = 10;

    private List<TerrainCell> cells = new List<TerrainCell>(); 
    private int numberOfCellsX;
    private int numberOfCellsY;
    #endregion

    // Start is called before the first frame update
    void Awake()
    {
        BuildTerrainBase();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    #region Methods
    void BuildTerrainBase()
    {
        numberOfCellsX = Mathf.FloorToInt(mapWidth/cellSize);
        numberOfCellsY = Mathf.FloorToInt(mapHeight/cellSize);

        for(int i = 0; i < numberOfCellsX; i++)
        {
            for(int j = 0; j < numberOfCellsY; j++)
            {
                Vector2 cellPosition = new Vector2(transform.position.x - (mapWidth/2) + (cellSize/2) + (cellSize*i), 
                    transform.position.y - (mapHeight/2) + (cellSize/2) + (cellSize*j));

                TerrainCell newCell = new TerrainCell(cells.Count, cellPosition, cellSize, tilemap, tilePavement, tileDirt);
                cells.Add(newCell);
            }
        }
    }

    public Vector3 RandomCellPosition(int newOwnerId)
    {
        int randomCell = 0;

        //TO DO: Change while for a list of available cells
        do 
        {
            randomCell = Random.Range(0, cells.Count);
        } while(cells[randomCell].Available == false);

        TerrainCell cell = cells[randomCell];
        cell.OwnerId = newOwnerId;
        cell.RenderMap(tilemap, tilePavement);
        return cell.Center;
    }    

    public void BuyTerrainByPosition(int newOwnerId, Vector3 relativePosition)
    {
        bool success = cells[0].BuyTerrain(0);

        #if UNITY_EDITOR
            if(!success) Debug.LogWarning("Terrain not available");
        #endif
    }

    public void BuyTerrainByID(int cellID, int newOwnerId)
    {
        bool success = cells[cellID].BuyTerrain(newOwnerId);

        #if UNITY_EDITOR
            if(!success) Debug.LogWarning("Terrain not available");
        #endif
    }
    #endregion

    #region Unity Editor
    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if(!debug) return;
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position, new Vector3(mapWidth, mapHeight));

        int _numberOfCellsX = Mathf.FloorToInt(mapWidth/cellSize);
        int _numberOfCellsY = Mathf.FloorToInt(mapHeight/cellSize);
        for(int i = 0; i < _numberOfCellsX; i++)
        {
            for(int j = 0; j < _numberOfCellsY; j++)
            {
                Vector2 cellPosition = new Vector2(transform.position.x - (mapWidth/2) + (cellSize/2) + (cellSize*i), 
                    transform.position.y - (mapHeight/2) + (cellSize/2) + (cellSize*j));

                Gizmos.DrawWireCube(cellPosition, new Vector3(cellSize, cellSize));
            }
        }
    }
    #endif
    #endregion
}
