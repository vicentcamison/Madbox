using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    //enemy spawner not only controls the behaviour ob the obstacle, but it also acts as a
    // checkpoint for the player: if the player dies during the obstacle, itr will be reverted back here

    private int enemyType = 0; //type of enemy behaviour
    public GameObject enemy0; //obstacle prefab

    private int checkpointNumber = 0; //index of checkpint along the level

    //VARAIBLES ASSOCIATED TO ALL THE DIFFERENT BEHAVIOURS
    private Vector3 enemySpawnStartPos; //starting position of the obstacle, if needed
    private float enemySpawnDelayTime; //delay between each obstacle spawned, if needed
    private float enemyLifeTime; //total lifetime of each obstacle, if needed
    private Vector3 enemySpeed; //speed of enemy, if needed

    private float enemyType2Radius = 4f; //radius of circle associated with enemyType = 2
    private float enemyType2Phase = 6.5f; //time (seconds) it takes to complete a full circle trajectory
    private float enemyType2PhaseNow = 0f; //position of the phase that we now have

    private float enemySpawnDelayNow;

    //new camera positions when reaching the obstacle
    private Vector3 playerCameraPosition;
    private Quaternion playerCameraRotation; 


    private GameObject m, n, o; //variables needed in case enemyType = 2;
    
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (enemySpawnDelayNow > 0)
        {
            enemySpawnDelayNow -= Time.deltaTime;
            if (enemySpawnDelayNow <= 0)
            {
                EnemySpawn();
                enemySpawnDelayNow = enemySpawnDelayTime;
            }
        }

        //management of position of rotating obstacles
        if (enemyType == 2)
        {
            ObstacleRotationManagement();
            enemyType2PhaseNow += Time.deltaTime;
        }
    }

    //Initiates the behaviour of the enemy spawner
    public void Initiate()
    {
        switch (enemyType)
        {
            //spawns enemies from left to right (along x axis)
            case 0:
                enemySpawnStartPos = transform.position + new Vector3(-5f, 0f, 2.5f);
                enemySpawnDelayTime = 3f;
                enemyLifeTime = 10f;
                enemySpeed = new Vector3(1.75f, 0f, 0f);
                enemySpawnDelayNow = enemySpawnDelayTime;
                EnemySpawn();

                //set camera position
                playerCameraPosition = new Vector3(0f, 8.1f, -3.7f);
                playerCameraRotation = Quaternion.Euler(new Vector3(52.68f, 0f, 0f));
                break;

            //spawns enemies from down to up (along y axis)
            case 1:
                enemySpawnStartPos = transform.position + new Vector3(0f, -5f, 2.5f);
                enemySpawnDelayTime = 3f;
                enemyLifeTime = 10f;
                enemySpeed = new Vector3(0f, 1.75f, 0f);
                enemySpawnDelayNow = enemySpawnDelayTime;
                EnemySpawn();

                //set camera position
                playerCameraPosition = new Vector3(0f, 3f, -6.8f);
                playerCameraRotation = Quaternion.Euler(new Vector3(12.63f, 0f, 0f));
                    
                break;

            //spawns 3 enemies that circle around
            case 2:
                enemySpawnStartPos = transform.position + new Vector3(-0.75f, 0f, 8f);
                enemyLifeTime = -1f; //infinite live time
                enemySpawnDelayTime = -1f; //no need to spawn more obstacles than the initial batch
                EnemySpawn();

                //set camera position
                playerCameraPosition = new Vector3(0f, 8.1f, -3.7f);
                playerCameraRotation = Quaternion.Euler(new Vector3(52.68f, 0f, 0f));
                break;
        }
    }

    //spawns an enemy according to the type and specs defined beforehand
    private void EnemySpawn()
    {
        GameObject g;

        switch (enemyType)
        {
            case 0:
                g = Instantiate(enemy0, enemySpawnStartPos, Quaternion.identity);
                g.GetComponent<Enemy0Controller>().lifeTime = enemyLifeTime;
                g.GetComponent<Enemy0Controller>().speed = enemySpeed;
                break;

            //if we spawn obstacles from below, we need a different obstacle for each spaceship
            case 1:
                g = Instantiate(enemy0, enemySpawnStartPos, Quaternion.identity);
                g.GetComponent<Enemy0Controller>().lifeTime = enemyLifeTime;
                g.GetComponent<Enemy0Controller>().speed = enemySpeed;

                g = Instantiate(enemy0, enemySpawnStartPos + new Vector3(-1.5f, 0.5f, 0f), Quaternion.identity);
                g.GetComponent<Enemy0Controller>().lifeTime = enemyLifeTime;
                g.GetComponent<Enemy0Controller>().speed = enemySpeed;

                g = Instantiate(enemy0, enemySpawnStartPos + new Vector3(-3f, 1f, 0f), Quaternion.identity);
                g.GetComponent<Enemy0Controller>().lifeTime = enemyLifeTime;
                g.GetComponent<Enemy0Controller>().speed = enemySpeed;

                g = Instantiate(enemy0, enemySpawnStartPos + new Vector3(1.5f, -0.5f, 0f), Quaternion.identity);
                g.GetComponent<Enemy0Controller>().lifeTime = enemyLifeTime;
                g.GetComponent<Enemy0Controller>().speed = enemySpeed;
                break;

            //spawn 3 obstacles that perform a circular trajectory
            case 2:
                m = Instantiate(enemy0, enemySpawnStartPos, Quaternion.identity);
                m.GetComponent<Enemy0Controller>().lifeTime = enemyLifeTime;

                n = Instantiate(enemy0, enemySpawnStartPos, Quaternion.identity);
                n.GetComponent<Enemy0Controller>().lifeTime = enemyLifeTime;

                o = Instantiate(enemy0, enemySpawnStartPos, Quaternion.identity);
                o.GetComponent<Enemy0Controller>().lifeTime = enemyLifeTime;
                break;

        }

    }

    //management of obstacle position in enemyType = 2;
    private void ObstacleRotationManagement()
    {
        m.transform.position = enemySpawnStartPos + new Vector3(enemyType2Radius * Mathf.Cos(enemyType2PhaseNow / enemyType2Phase * 2f * Mathf.PI), 0f, enemyType2Radius * Mathf.Sin(enemyType2PhaseNow / enemyType2Phase * 2f * Mathf.PI));
        n.transform.position = enemySpawnStartPos + new Vector3(enemyType2Radius * Mathf.Cos(enemyType2PhaseNow / enemyType2Phase * 2f * Mathf.PI + 2f / 3f * Mathf.PI), 0f, enemyType2Radius * Mathf.Sin(enemyType2PhaseNow / enemyType2Phase * 2f * Mathf.PI + 2f / 3f * Mathf.PI));
        o.transform.position = enemySpawnStartPos + new Vector3(enemyType2Radius * Mathf.Cos(enemyType2PhaseNow / enemyType2Phase * 2f * Mathf.PI + 4f / 3f * Mathf.PI), 0f, enemyType2Radius * Mathf.Sin(enemyType2PhaseNow / enemyType2Phase * 2f * Mathf.PI + 4f / 3f * Mathf.PI));
    }

    //GETTERS AND SETTERS

    //set spawner index along the level
    public void SetSpawnerIndex(int index)
    {
        checkpointNumber = index;
    }

    public int GetSpawnerIndex()
    {
        return checkpointNumber;
    }

    //set spawner behaviour
    public void SetSpawnerType(int type)
    {
        enemyType = type;
    }

    //get new camera position
    public Vector3 GetCameraPos()
    {
        return playerCameraPosition;
    }

    //get new camera rotation
    public Quaternion GetCameraRotation()
    {
        return playerCameraRotation;
    }
}
