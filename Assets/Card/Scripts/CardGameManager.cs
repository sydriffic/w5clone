using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

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
            case GameState.OCHOOSE:
                    OpponentChoice();
                break;
            case GameState.PCHOOSE:
                    PlayerChoice();
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
                                                    playerPos.transform.position.y + 5.5f,
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
            cardScript.inHand = true;
            //play audio here
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
        timer --;
        if(timer <= -35)
        {
            if(opponentHand.Count == playerHandCount)
            {
                GameObject randCard = opponentHand[Random.Range(0, opponentHand.Count)];
                Card cardScript = randCard.GetComponent<Card>();
                Vector3 newPos = playerPos.transform.position;
                //i should prob set variables for where to go. womp womp
                cardScript.targetPos = new Vector3((newPos.x + (2)), 
                                                    playerPos.transform.position.y + 3.5f,
                                                    playerPos.transform.position.z);
                //play audio here
                cardScript.oPlayed = true;
                state = GameState.PCHOOSE;
            }

            timer = maxTimer;
        }
    }

    void PlayerChoice()
    {
        //
        
        for(int i = 0; i < playerHand.Count; i ++)
        {
            
            GameObject nextCard = playerHand[i];
            Card cardScript = nextCard.GetComponent<Card>();
            Vector3 newPos = playerPos.transform.position;
            cardScript.targetPos = new Vector3((newPos.x + (2*i)), 
                                    playerPos.transform.position.y,
                                    playerPos.transform.position.z);
          
            Debug.Log(i + " " + cardScript.mouseOver);
            //Debug.Log(cardScript.faceSprite.name + " lerping Down");
        }



        
    }

    
}
