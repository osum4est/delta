using UnityEngine;
using System.Collections;

public class ExtraUtils  {

    public static void MoveParticleSystem(GameObject go, Vector3 translation)
    {
        ParticleSystem ps = go.GetComponent<ParticleSystem>();
        if (ps == null)
            ps = go.GetComponentInChildren<ParticleSystem>();

        ParticleSystem.Particle[] parts = new ParticleSystem.Particle[ps.maxParticles];
        int iParts = ps.GetParticles(parts);
        for (int i = 0; i < iParts; i++)
            parts[i].position += translation;
        ps.SetParticles(parts, iParts);
    }

    static GameObject AddToWorld(GameObject go)
    {
        go.transform.SetParent(Globals.i.world.transform);
        return go;
    }

    public static GameObject SpawnGameObject(GameObject go)
    {
        GameObject spawnedObj = (GameObject)GameObject.Instantiate(go, Vector3.zero, Quaternion.identity);
        WorldHandler.objs.Add(spawnedObj);

        return AddToWorld(spawnedObj);
    }
    public static GameObject SpawnGameObject(GameObject go, Vector3 position)
    {
        GameObject spawnedObj = (GameObject)GameObject.Instantiate(go, position, Quaternion.identity);
        WorldHandler.objs.Add(spawnedObj);
        
        return AddToWorld(spawnedObj);
    }
    public static GameObject SpawnGameObject(GameObject go, Vector3 position, Quaternion rotation)
    {
        GameObject spawnedObj = (GameObject)GameObject.Instantiate(go, position, rotation);
        WorldHandler.objs.Add(spawnedObj);
        
        return AddToWorld(spawnedObj);
    }
    public static GameObject SpawnGameObject(GameObject go, Vector3 position, Quaternion rotation, Color color)
    {
        GameObject spawnedObj = (GameObject)GameObject.Instantiate(go, position, rotation);
        WorldHandler.objs.Add(spawnedObj);

        spawnedObj.GetComponent<SpriteRenderer>().color = color;
        return AddToWorld(spawnedObj);
    }

    public static void RemoveGameObject(GameObject go)
    {
        WorldHandler.objs.Remove(go);
        GameObject.Destroy(go);
    }
}
