using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject enemySpawner; //prefab of enemy spawner
    public GameObject finishLine; //prefab of the finish line
    public static float levelLength; //length of the level

    // Start is called before the first frame update
    void Start()
    {
        //creating all the elements that conform the level (obstacles and finish line)
        InstantiateEnemySpawner(Vector3.forward * 10f, 1, 0);
        InstantiateEnemySpawner(Vector3.forward * 20f, 2, 0);
        InstantiateEnemySpawner(Vector3.forward * 30f, 3, 0);
        InstantiateEnemySpawner(Vector3.forward * 40f, 4, 1);
        InstantiateEnemySpawner(Vector3.forward * 60f, 5, 2);
        InstantiateEnemySpawner(Vector3.forward * 80f, 6, 1);
        InstantiateEnemySpawner(Vector3.forward * 100f, 7, 2);

        levelLength = 120f; //total length of this level
        //instantiate finish line
        Instantiate(finishLine, new Vector3(0f, 0f, levelLength), Quaternion.identity);
    }


    void InstantiateEnemySpawner(Vector3 position, int index, int type)
    {
        GameObject g = Instantiate(enemySpawner, position, Quaternion.identity);
        g.GetComponent<EnemySpawnerController>().SetSpawnerIndex(index);
        g.GetComponent<EnemySpawnerController>().SetSpawnerType(type);

        g.GetComponent<EnemySpawnerController>().Initiate();       
    }
}
