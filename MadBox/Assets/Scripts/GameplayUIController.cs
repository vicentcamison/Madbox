using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameplayUIController : MonoBehaviour
{

    public Slider raceLength;
    public TextMeshProUGUI playerPos;
    public GameObject player;
    public TextMeshProUGUI countdown;

    //variables related to race start countdown
    private float countdownTimer = -1f;
    private float countdownSizeMin = 40f;
    private float countdownSizeMax = 80f;

    private float finishMessageTime = 3f; //time "Finish" message stays on screen

    private bool raceStart = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCountdown();
    }

    // Update is called once per frame
    void Update()
    {
        //RACE LENGTH BAR
        raceLength.value = Mathf.Clamp(player.transform.position.z / GameController.levelLength, 0f, 1f);

        //PLAYER POSITION IN RACE
        playerPos.text = player.GetComponent<PositioningSystem>().PlayerStringPosGet();

        //code realted to start race countdown
        if ((countdownTimer > 0f) && (countdownTimer < 10f))
            CountdownUpdate();
        //we use this variable also to display "Finish!", we force countdownTimer 11f; for that situation
        else if (countdownTimer >= 10)
            FinishMessage();

        //TOUCH CONTROLS: lets you restart scene after finishing
        if (((Input.touchCount > 0) || (Input.GetMouseButton(0))) && (countdownTimer == 10f))
        {
            SceneManager.LoadScene("MainScene");
        }
    }

    //Start race countdown
    public void StartCountdown()
    {
        //countdownTimer starts at 4 because it represents all 4 possible states: 3, 2, 1, Go!
        countdownTimer = 4f;
    }

    //Race countdown update event
    private void CountdownUpdate()
    {
        countdownTimer -= Time.deltaTime;
        if (countdownTimer >= 1)
            countdown.text = Mathf.CeilToInt(countdownTimer - 1f).ToString();
        else
            countdown.text = "Go!";
        countdown.fontSize = countdownSizeMin + (countdownSizeMax - countdownSizeMin) * (countdownTimer % 1);

        if (countdownTimer < 0)
            countdownTimer = 0;

        //vanishing effect in "Go!"
        if (countdownTimer < 1)
            countdown.color = new Color(1f, 1f, 1f, countdownTimer);

        //start all player and AI movement when reaching "Go!"
        if ((countdownTimer <= 1) && (raceStart == false))
        {
            raceStart = true;
            RaceStart();
        }

    }

    //Finish message
    private void FinishMessage()
    {
        if (countdownTimer > 10f)
        {
            countdown.text = "Finish!";
            countdownTimer -= Time.deltaTime / finishMessageTime;
            if (countdownTimer < 10f)
                countdownTimer = 10f;
        }
        else
        {
            countdown.fontSize = 40f;
            countdown.text = "Click to restart";
        }

        countdown.color = new Color(1f, 1f, 1f, 1f);
    }

    //activate finish! procedure
    public void Finished()
    {
        countdownTimer = 11f;
    }

    //activates all spaceships
    private void RaceStart()
    {
        GameObject[] players;

        //activate AI
        players = GameObject.FindGameObjectsWithTag("AI");
        foreach (GameObject player in players)
        {
            player.GetComponent<PlayerController>().ActivateSpaceship();
        }

        //activate player
        players[0] = GameObject.FindGameObjectWithTag("Player");
        players[0].GetComponent<PlayerController>().ActivateSpaceship();
    }

}
