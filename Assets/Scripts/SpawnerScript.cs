using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnerScript : MonoBehaviour
{
    private GameObject SpawnObject;
    public GameObject[] SpawnObjects;

    private List<GameObject> spawnedPipes = new List<GameObject>();

    public float timeMin = 1.5f;
    public float timeMax = 2.5f;
    // Use this for initialization
    void Start()
    {
        SpawnObject = SpawnObjects[Random.Range(0, SpawnObjects.Length)];
        Spawn();
    }

    void Spawn()
    {
        if (GameStateManager.GameState == GameState.Playing)
        {
            //random y position
            float y = Random.Range(-0.5f, 1f);

            GameObject go = Instantiate(SpawnObject, this.transform.position + new Vector3(0, y, 0), Quaternion.identity) as GameObject;
            spawnedPipes.Add(go);
        }
        Invoke("Spawn", Random.Range(timeMin, timeMax));
    }
    public GameObject GetClosestPipe(Transform t)
    {


        for (int i = 0; i < spawnedPipes.Count; i++)
        {
            if (spawnedPipes[i].transform.position.x < t.position.x)
            {
                spawnedPipes.RemoveAt(i);
            }
        }
        if (spawnedPipes.Count > 0)
        {

            return spawnedPipes[0];
        }
        return null;
    }

    private void Update()
    {

    }


}
