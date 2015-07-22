using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public class WorldHandler : MonoBehaviour
{
    public float cameraMoveSpeed = .05f;
    public SpawnableObject[] objectsToSpawn;
    public float gridSize = 100f;
	
    public static List<Cell> cells;
    public static List<GameObject> objs;
   
	
    void Start()
    {
        cells = new List<Cell>();
        objs = new List<GameObject>();
        CheckForSpawning(true);
    }
	
    void Update()
    {
        CheckForSpawning();
        CheckForDespawning();
    }
	
    void LateUpdate()
    {
        Vector3 player = Globals.i.player.transform.localPosition;
        Vector3 newPos = Vector3.MoveTowards(Globals.i.mainCamera.transform.position, player, cameraMoveSpeed * Vector3.Distance(Globals.i.mainCamera.transform.position, player));
        Globals.i.background.transform.position = new Vector3(newPos.x, newPos.y, Globals.i.background.transform.position.z);
    }

    void CheckForDespawning()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (Mathf.Abs(cells[i].Position.x) > 1 || Mathf.Abs(cells[i].Position.y) > 1)
            {
                //UnityEngine.Debug.Log("removing cell at: " + cells[i].Position.x + ", " + cells[i].Position.y);
                cells[i].Remove();
            }
        }
    }
	
    void CheckForSpawning(bool forceSpawn = false)
    {
        Vector2 newPlayerCell = new Vector2(Mathf.Round(Globals.i.player.transform.position.x / gridSize), Mathf.Round(Globals.i.player.transform.position.y / gridSize));
		
        if (Vector2.zero != newPlayerCell || forceSpawn)
        {

			/* player changed cells */
			ResetWorld(newPlayerCell);

            for (int y = 0; y < 3; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    if (x * 3 + y != 4)
                    {
                        bool createNew = true;
                        Vector2 cellPosition = new Vector2((y - 1), (x - 1));
                        foreach (Cell cell in cells)
                        {
                            if (cell.Position == cellPosition)
                            {
                                createNew = false;
                                //.Log("Not adding cell");
                                break;
                            }
                        }

                        if (createNew)
                        {
                            //Debug.Log("Adding cell at: " + cellPosition.x + ", " + cellPosition.y);
                            cells.Add(new Cell(cellPosition));
                        }
                    }
                }
            }
        }
    }

	void ResetWorld(Vector2 playerCell)
	{

        Stopwatch watch = Stopwatch.StartNew();

		float translateX = 0;
		float translateY = 0;

		switch((int)playerCell.x)
		{
		case -1:
            translateX = gridSize;
            break;
        case 1:
            translateX = -gridSize;
            break;
		}
        switch((int)playerCell.y)
        {
        case -1: 
            translateY = gridSize;
            break;
        case 1: 
            translateY = -gridSize;
            break;
        }

        //UnityEngine.Debug.Log("resetting world by: " + translateX + ", " + translateY);

        Vector3 translation = new Vector2(translateX, translateY);

        Globals.i.player.transform.position += translation;
        
        Globals.i.mainCamera.transform.position += translation;
        Globals.i.background.transform.position += translation;

        ExtraUtils.MoveParticleSystem(Globals.i.background, translation);
        ExtraUtils.MoveParticleSystem(Globals.i.player, translation);

        for (int i = 0; i < cells.Count; i++)
        {
            cells[i].Position += playerCell * -1;
        }
        for (int i = 0; i < objs.Count; i++)
        {
            //Debug.Log("Translating object " + objs[i].transform.position.x + ", " + objs[i].transform.position.y);
            objs[i].transform.position += translation;
            //Debug.Log("Translated object to" + objs[i].transform.position.x + ", " + objs[i].transform.position.y);
        }

        watch.Stop();
        UnityEngine.Debug.Log("Took " + watch.ElapsedMilliseconds + " ms to reset world.");
	}
}

[System.Serializable]
public class SpawnableObject
{
    public GameObject gameObject;
    public float spawnChance;
    public int spawnAmount;
}

public class Cell
{

    private Vector2 _position;
    public Vector2 Position {
        get {
            return _position;
        }
        set { 
            _position = value; 
            worldPosition = new Vector2(value.x * Globals.i.worldHandler.gridSize, value.y * Globals.i.worldHandler.gridSize);
        }
    }
    public Vector2 worldPosition { get; protected set; }
	
    public Cell(Vector2 position)
    {
        this.Position = position;

        SpawnObjects();
    }
	
    public void SpawnObjects()
    {
        for (int i = 0; i < Globals.i.worldHandler.objectsToSpawn.Length; i++)
        {
            SpawnableObject obj = Globals.i.worldHandler.objectsToSpawn[i];
            for (int j = 0; j < obj.spawnAmount; j++)
            {
                if (Random.value < obj.spawnChance)
                {
                    Vector2 objPosition = new Vector2(Random.Range(worldPosition.x - Globals.i.worldHandler.gridSize / 2, worldPosition.x + Globals.i.worldHandler.gridSize / 2), 
                                                      Random.Range(worldPosition.y - Globals.i.worldHandler.gridSize / 2, worldPosition.y + Globals.i.worldHandler.gridSize / 2));

					ExtraUtils.SpawnGameObject(obj.gameObject, objPosition);
                }
            }
        }
    }

    public void Remove()
    {
        WorldHandler.cells.Remove(this);
    }


}