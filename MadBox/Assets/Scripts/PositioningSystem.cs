using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositioningSystem : MonoBehaviour
{
    //system in charge to calculate players' rank in race (1st, 2nd... etc)

    public GameObject[] AI = new GameObject[3]; //opponents
    private bool raceFinished = false; //has the race already finished? If so, positions stop being tracked

    private int playerRank; //player rank in the race

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!raceFinished)
        {
            playerRank = PlayerRankCalculate();
            raceFinished = CheckRaceFinished();
        }

    }

    //returns the rank of the player in the race
    public string PlayerStringPosGet()
    {
        switch (playerRank)
        {
            case 1:
                return "1st";

            case 2:
                return "2nd";

            case 3:
                return "3rd";

            default:
                return playerRank + "th";
        }
    }

    //checks if player has surpassed finish line
    private bool CheckRaceFinished()
    {
        if (transform.position.z >= GameController.levelLength)
            return true;
        else
            return false;
    }

    //calculates player rank
    public int PlayerRankCalculate()
    {
        int rank = 1;
        for (int i = 0; i < AI.Length; i++)
        {
            if (AI[i].transform.position.z > transform.position.z)
                rank++;
        }
        return rank;
    }
}
