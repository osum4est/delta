using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldHandler : MonoBehaviour
{
    public float cameraMoveSpeed = .05f;
    public SpawnableObject[] objectsToSpawn;
    public float gridSize = 100f;
	
    public static List<Cell> cells;
   
	
    void Start()
    {
        cells = new List<Cell>();
        CheckForSpawning(true);
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
        Globals.background.transform.position = new Vector3(newPos.x, newPos.y, Globals.background.transform.position.z);
    }

    void CheckForDespawning()
    {
        for (int i = 0; i < cells.Count; i++)
        {
            if (Mathf.Abs(cells[i].Position.x) > 1 || Mathf.Abs(cells[i].Position.y) > 1)
            {
                Debug.Log("removing cell at: " + cells[i].Position.x + ", " + cells[i].Position.y);
                cells[i].Remove();
            }
        }
    }
	
    void CheckForSpawning(bool forceSpawn = false)
    {
        Vector2 newPlayerCell = new Vector2(Mathf.Round(Globals.player.transform.position.x / gridSize), Mathf.Round(Globals.player.transform.position.y / gridSize));
		
        if (Vector2.zero != newPlayerCell || forceSpawn)
        {
            Debug.Log("You moved to cell: " + newPlayerCell.x + ", " + newPlayerCell.y);
            //playerCell = newPlayerCell;

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

        Debug.Log("resetting world by: " + translateX + ", " + translateY);

        Vector3 translation = new Vector2(translateX, translateY);

		foreach (Cell cell in cells)
        {
            //Debug.Log("Translating cell " + cell.Position.x + ", " + cell.Position.y);
            cell.Position += playerCell * -1;
           // Debug.Log("Translated cell to " + cell.Position.x + ", " + cell.Position.y);
			foreach(GameObject obj in cell.objs)
            {
                Debug.Log("Translating object " + obj.transform.position.x + ", " + obj.transform.position.y);
                obj.transform.position += translation;
                Debug.Log("Translated object to" + obj.transform.position.x + ", " + obj.transform.position.y);
            }
        }

        Globals.player.transform.position += translation;

        Globals.mainCamera.transform.position += translation;
        Globals.background.transform.position += translation;

        ExtraUtils.MoveParticleSystem(Globals.background, translation);
        ExtraUtils.MoveParticleSystem(Globals.player, translation);
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
            worldPosition = new Vector2(value.x * Globals.worldHandler.gridSize, value.y * Globals.worldHandler.gridSize);
        }
    }
    public Vector2 worldPosition { get; protected set; }
	

    public List<GameObject> objs;
	
    public Cell(Vector2 position)
    {
        this.Position = position;
		
		objs = new List<GameObject>();

        SpawnObjects();
    }
	
    public void SpawnObjects()
    {
        for (int i = 0; i < Globals.worldHandler.objectsToSpawn.Length; i++)
        {
            SpawnableObject obj = Globals.worldHandler.objectsToSpawn[i];

            //GameObject spawnedObj = (GameObject)GameObject.Instantiate(obj.gameObject, worldPosition, Quaternion.identity);
            //objs.Add(spawnedObj);
            //spawnedObj.GetComponent<SpriteRenderer>().color = new Color(Random.value, Random.value, Random.value);

            for (int j = 0; j < obj.spawnAmount; j++)
            {
                if (Random.value < obj.spawnChance)
                {
                    Vector2 objPosition = new Vector2(Random.Range(worldPosition.x - Globals.worldHandler.gridSize / 2, worldPosition.x + Globals.worldHandler.gridSize / 2), 
					                                  Random.Range(worldPosition.y - Globals.worldHandler.gridSize / 2, worldPosition.y + Globals.worldHandler.gridSize / 2));

					GameObject spawnedObj = (GameObject)GameObject.Instantiate(obj.gameObject, objPosition, Quaternion.identity);
                    objs.Add(spawnedObj);
                }
            }
        }
    }

    public void Remove()
    {
        foreach (GameObject obj in objs)
            GameObject.Destroy(obj);
        WorldHandler.cells.Remove(this);
    }


}