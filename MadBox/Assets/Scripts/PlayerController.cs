using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //this code is used for both player and AI. to differentiate both cases, objects have different tags "Player" and "AI"

    private float playerAccel = 12f; //acceleration of the player
    private float playerDeccel = 12f; //decceleration of the player
    private float playerSpeedMax = 6f; //max speed of the player
    private float playerSpeedMin = 0f; //min speed of the player
    private float AISpeedMax = 4f;

    private float playerSpeed = 0f; //speed of the player

    private int mode = 0; //behaviour mode of player

    private float deadRespawnTimeTotal = 1f; //time (in seconds) it takes to be respawned
    private float deadRespawnTime = 1f;

    private float lastCheckpointPositionZ = 0f; //z position of the last checkpoint that the player has intercated with
    private int lastCheckpointIndex = 0; //index of the last checkpoint the player has interacted with

    public GameObject camera;
    public GameObject UI;

    // Start is called before the first frame update
    void Start()
    {
        mode = 0;
        //mode = 0: before the race
        //mode = 1: gameplay mode
        //mode = 2: dead, waiting to respawn
        //mode = 3: finished
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            //gameplay mode
            case 1:
                //Player input
                if (tag == "Player")
                    GetInput();
                else
                    PlayerAccel();

                //Player movement
                transform.position += new Vector3(0f, 0f, playerSpeed * Time.deltaTime);
                break;

            //dead: waiting to respawn
            case 2:
                deadRespawnTime -= Time.deltaTime;
                if (deadRespawnTime <= 0)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y, lastCheckpointPositionZ);
                    deadRespawnTime = deadRespawnTimeTotal;
                    ActivateMeshRenderers(true);
                    mode = 1;

                }
                break;

            //finished
            case 3:
                PlayerAccelMax();

                transform.position += new Vector3(0f, 0f, playerSpeed * Time.deltaTime);
                break;
        }
        
    }

    //ACCELERATION AND DECCELERATION

    //accelerates the player
    private void PlayerAccel()
    {
        playerSpeed += playerAccel * Time.deltaTime;
        if (tag == "Player")
            playerSpeed = Mathf.Min(playerSpeed, playerSpeedMax);
        else
            playerSpeed = Mathf.Min(playerSpeed, AISpeedMax);
    }

    //deccelerates the player
    private void PlayerDeccel()
    {
        playerSpeed -= playerDeccel * Time.deltaTime;
        playerSpeed = Mathf.Max(playerSpeed, playerSpeedMin);
    }

    //accelerates the player after finish line
    private void PlayerAccelMax()
    {
        playerSpeed += playerAccel * 3f * Time.deltaTime;
    }

    //Deals with everything related to player input
    private void GetInput()
    {
        if ((Input.touchCount > 0) || (Input.GetMouseButton(0)))
            PlayerAccel();
        else
            PlayerDeccel();

    }

    //collision with stuff
    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            //collision with the beginning of an obstacle
            case "Spawner":
                //we add the new checkpoint position
                int newCheckpoint = other.gameObject.GetComponent<EnemySpawnerController>().GetSpawnerIndex();
                if (newCheckpoint > lastCheckpointIndex)
                {
                    lastCheckpointIndex = newCheckpoint;
                    lastCheckpointPositionZ = other.gameObject.transform.position.z;
                }

                //change camera position to better suit the situation
                if (tag == "Player")
                    NewCameraPosition(other.gameObject, camera);
                break;

            //collision with an obstacle
            case "Enemy":
                if (mode == 1)
                {
                    mode = 2;
                    ActivateMeshRenderers(false);
                }
                break;

            //collision with finish line
            case "Finish":
                if (mode == 1)
                {
                    mode = 3;
                    //display "finish!" text
                    UI.GetComponent<GameplayUIController>().Finished();
                }
                break;
                
        }
    }

    //start the spaceships
    public void ActivateSpaceship()
    {
        if (mode == 0)
            mode = 1;
    }

    //deactivate all mesh renderers
    private void ActivateMeshRenderers(bool activate)
    {
        foreach (Transform child in transform.Find("AtomRocket"))
            child.gameObject.GetComponent<MeshRenderer>().enabled = activate;
    }

    //set new camera position
    private void NewCameraPosition(GameObject spawner, GameObject camera)
    {
        camera.GetComponent<CameraController>().ChangeCameraRelativePosition(spawner.GetComponent<EnemySpawnerController>().GetCameraPos(), spawner.GetComponent<EnemySpawnerController>().GetCameraRotation());
    }
}
