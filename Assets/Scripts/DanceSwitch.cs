using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DanceSwitch : MonoBehaviour
{
    Animator anim;

    //array to store whether or not our character has used each dance mnove
    bool[] movesBusted;
    //bool[] movesBusted = new bool[5];

    //number of moves available
    public int numOfMoves;


    // Start is called before the first frame update
    void Start()
    {
        //setting the animator to the Animator component
        anim = GetComponent<Animator>();
        movesBusted = new bool[numOfMoves];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            BustRandomMove();
        }

    }

    void BustRandomMove()
    {
        //first make sure there is at least 1 unused move to select from
        CheckMoves(movesBusted);

        //pick a random number from 0-the number of moves available
        int moveIndex = Random.Range(0, numOfMoves);

        //if the move has already been used, pick another random move
        if(movesBusted[moveIndex] == true)
        {
            BustRandomMove();
        }

        //otherwise bust this move and add it to the array of previously used moves
        else 
        {
            movesBusted[moveIndex] = true;
            anim.SetInteger("MoveIndex", moveIndex);
            anim.SetTrigger("BustMove");
        }
    }

    void ResetMoves(bool[] moves, bool isBusted)
    {
        //loop through array, setting them all to the request value
        for(int i = 0; i < moves.Length; i++)
        {
            moves[i] = isBusted;
        }
    }

    // <summary>
    // loops through an array of booleans checking their values
    // if they are all true, reset all to false
    // </summary>
    // <param name="moves"></param>
    void CheckMoves(bool[] moves)
    {
        int numMovesBusted = 0;

        foreach(bool move in moves)
        {
            if(move == true)
            {
                numMovesBusted += 1;
            }
        }

        if(numMovesBusted >= moves.Length)
        {
            ResetMoves(moves, false);
        }
    }
}
