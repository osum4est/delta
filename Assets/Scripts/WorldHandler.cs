using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldHandler : MonoBehaviour
{
    public float cameraMoveSpeed = .05f;
    public SpawableObject[] objectsToSpawn;
    public float gridSize = 100f;
	
    private List<Cell> cells;
    private Vector2 playerCell;
	
    void Start()
    {
        playerCell = Vector2.one;
        cells = new List<Cell>();
    }
	
    void Update()
    {
        CheckForSpawning();
        CheckForDespawning();
    }
	
    void LateUpdate()
    {
        Vector3 player = Globals.player.transform.localPosition;
        Vector3 newPos = Vector3.MoveTowards(Globals.mainCamera.transform.position, player, cameraMoveSpeed * Vector3.Distance(Globals.mainCamera.transform.position, player));
        Globals.mainCamera.transform.position = new Vector3(newPos.x, newPos.y, Globals.mainCamera.transform.position.z);
        Globals.background.transform.position = new Vector3(newPos.x, newPos.y, Globals.background.transform.position.z);
    }

    void CheckForDespawning()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (Vector2.Distance(cells[i].worldPosition, Globals.player.transform.position) > gridSize * 2.5f)
            {
                cells.Remove(cells[i]);
            }
        }
    }
	
    void CheckForSpawning()
    {
        Vector2 newPlayerCell = new Vector2(Mathf.Round(Globals.player.transform.position.x / gridSize), Mathf.Round(Globals.player.transform.position.y / gridSize));
		
        if (playerCell != newPlayerCell)
        {
            playerCell = new Vector2(Mathf.Round(Globals.player.transform.position.x / gridSize), Mathf.Round(Globals.player.transform.position.y / gridSize));

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (x * 3 + y != 4)
                    {
                        bool createNew = true;
                        Vector2 cellPosition = new Vector2((y - 1) + playerCell.x, (-x + 1) + playerCell.y);
                        foreach (Cell cell in cells)
                        {
                            if (cell.position == cellPosition)
                            {
                                createNew = false;
                                break;
                            }
                        }

                        if (createNew)
                        {
                            cells.Add(new Cell(cellPosition));
                        }
                    }
                }
            }
        }
    }
	
    public static GameObject InstantiateAt(GameObject gameObject, Vector2 position)
    {
        gameObject.transform.position = position;
        return Instantiate(gameObject);
    }
}

[System.Serializable]
public class SpawableObject
{
    public GameObject gameObject;
    public float spawnChance;
    public int spawnAmount;
}

class Cell
{
    public Vector2 position;
    public Vector2 worldPosition;
	
    public Cell(Vector2 position)
    {
        this.position = position;
        worldPosition = new Vector2(position.x * Globals.worldHandler.gridSize, position.y * Globals.worldHandler.gridSize);
		
        SpawnObjects();
    }
	
    public void SpawnObjects()
    {
        for (int i = 0; i < Globals.worldHandler.objectsToSpawn.Length; i++)
        {
            SpawableObject obj = Globals.worldHandler.objectsToSpawn[i];
			
            for (int j = 0; j < obj.spawnAmount; j++)
            {
                if (Random.value < obj.spawnChance)
                {
                    Vector2 objPosition = new Vector2(Random.Range(worldPosition.x - Globals.worldHandler.gridSize / 2, worldPosition.x + Globals.worldHandler.gridSize / 2), 
					                                  Random.Range(worldPosition.y - Globals.worldHandler.gridSize / 2, worldPosition.y + Globals.worldHandler.gridSize / 2));
                    GameObject objToSpawn = WorldHandler.InstantiateAt(Globals.worldHandler.objectsToSpawn[i].gameObject, objPosition);

                    objToSpawn.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);
                }
            }
        }
    }
}