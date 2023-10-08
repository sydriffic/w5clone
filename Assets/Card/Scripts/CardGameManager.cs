using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardGameManager : MonoBehaviour
{
    public enum GameState
    {
        ODEAL,
        PDEAL, 
        OCHOOSE,
        PCHOOSE,
        EVAL,
        DISCARD,
        RESHUFFLE
    }

    public static GameState state;
    public List<GameObject> playerHand = new List<GameObject>();
    public List<GameObject> opponentHand = new List<GameObject>();
    public int playerHandCount;
    public int oHandCount;
    public Transform playerPos;

    //timer
    float maxTimer, timer, revealTimer;

    void Start()
    {
        state = GameState.ODEAL;

        //timer
        maxTimer = 15f;
        timer = maxTimer;
        revealTimer = 0;
    }

    void Update()
    {
        switch(state)
        {
            case GameState.ODEAL:
                    DealOpponentCard();
                break;
            case GameState.PDEAL:
                    DealPlayerCard();
                break;
            case GameState.PCHOOSE:
                break;
            case GameState.EVAL:
                break;
        }
    }

    void DealOpponentCard()
    {
        timer --;
        if(timer <= 0)
        {
            if(opponentHand.Count < playerHandCount)
            {
                GameObject nextCard = DeckManager.deck[0];
                Card cardScript = nextCard.GetComponent<Card>();
                Vector3 newPos = playerPos.transform.position;
                cardScript.targetPos = new Vector3((newPos.x + (2f * opponentHand.Count)), 
                                                    playerPos.transform.position.y + 3f,
                                                    playerPos.transform.position.z);
                opponentHand.Add(nextCard);
                DeckManager.deck.Remove(nextCard);
            }
            else
            {
                state = GameState.PDEAL;
            }

            //reset timer
            timer = maxTimer;
        }
    }

    void DealPlayerCard()
    {
        timer --;
        revealTimer ++;

        if(timer <= 0)
        {
            if(playerHand.Count < playerHandCount)
            {
            GameObject nextCard = DeckManager.deck[0];
            Card cardScript = nextCard.GetComponent<Card>();
            Vector3 newPos = playerPos.transform.position;
            cardScript.targetPos = new Vector3((newPos.x + (2f * playerHand.Count)), 
                                    playerPos.transform.position.y,
                                    playerPos.transform.position.z);
            playerHand.Add(nextCard);
            DeckManager.deck.Remove(nextCard);
            }

            //reset timer
            timer = maxTimer;
        }

        

        //ok so this code is like. play a sound and flip the cards after they've been dealt
        //but idk if this is going to stay that way
        if(revealTimer >= 70f)
        {
            //play audio
            //reveal cards waBOOM
            for(int i = 0; i < playerHand.Count; i ++)
            {
                Card cardScript = playerHand[i].GetComponent<Card>();
                cardScript.myRend.sprite = cardScript.faceSprite;
                state = GameState.OCHOOSE;
            }

            //reset revealTimer 2
            revealTimer = 0;
        }


        Debug.Log(revealTimer);

        /*GameObject nextCard = DeckManager.deck[DeckManager.deck.Count - 1];
        Vector3 newPos = playerPos.transform.position;
        newPos.x = newPos.x + (2f * playerHand.Count);
        nextCard.transform.position = newPos;
        playerHand.Add(nextCard);
        DeckManager.deck.Remove(nextCard);*/
    }

    void OpponentChoice()
    {

    }

    void PlayerChoice()
    {

    }
}
