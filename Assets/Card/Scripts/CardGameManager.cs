using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using TMPro;

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
    
    public Transform playerPos;

    public Transform discardPos, deckPos;

    //timer
    float maxTimer, timer, revealTimer;

    public AudioSource audiosrc;
    public AudioClip place, win, lose, shuffle;
    
    public TMP_Text pScoreText, oScoreText;
    int pScore, oScore;

    void Start()
    {
        state = GameState.ODEAL;

        //timer
        maxTimer = 15f;
        timer = maxTimer;
        revealTimer = 0;

        audiosrc = GetComponent<AudioSource>();
    }

    void Update()
    {

        pScoreText.text = "" + pScore;
        oScoreText.text = "" + oScore;
        
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
                    //i did this all in Card
                    //im sorry coders everywhere
                    //this feels blasphemous
                break;
            case GameState.EVAL:
                    Eval();
                break;
            case GameState.DISCARD:
                    Discard();
                break;
            case GameState.RESHUFFLE:
                    Reshuffle();
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
                                                    playerPos.transform.position.y + 5.8f,
                                                    playerPos.transform.position.z);
                audiosrc.clip = place;
                audiosrc.Play();
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
            audiosrc.clip = place;
            audiosrc.Play();
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
            audiosrc.clip = place;
            audiosrc.Play();
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
                                                    playerPos.transform.position.y + 3.8f,
                                                    playerPos.transform.position.z);
                //play audio here
                audiosrc.clip = place;
                audiosrc.Play();
                cardScript.oPlayed = true;
                state = GameState.PCHOOSE;
            }

            timer = maxTimer;
        }
    }

    void Eval()
    {
        timer --;
        Debug.Log(timer);
        if(timer <= -100)
        {
            for(int i = 0; i < opponentHand.Count; i ++)
            {
                Card cardScript = opponentHand[i].GetComponent<Card>();
                if(cardScript.oPlayed)
                {
                    cardScript.myRend.sprite = cardScript.faceSprite;
                    //WHO WINS
                    //opponent plays rock
                    if(cardScript.myRend.sprite.name == "rock")
                    {
                        for(int p = 0; p < playerHand.Count; p ++)
                        {
                            Card pScript = playerHand[p].GetComponent<Card>();
                            //player plays rock (tie)
                            if(pScript.pPlayed && pScript.myRend.sprite.name == "rock")
                            {
                                //no audio, no point change, just move to next state
                                state = GameState.DISCARD;
                            }
                            //player plays paper (win)
                            if(pScript.pPlayed && pScript.myRend.sprite.name == "paper")
                            {
                                PlayerWin();
                            }
                            //player plays scissors (lose)
                            if(pScript.pPlayed && pScript.myRend.sprite.name == "scissors")
                            {
                                PlayerLoss();
                            }
                        }

                        state = GameState.DISCARD;
                    }
                    
                    //opponent plays paper
                    if(cardScript.myRend.sprite.name == "paper")
                    {
                        for(int p = 0; p < playerHand.Count; p ++)
                        {
                            Card pScript = playerHand[p].GetComponent<Card>();
                            //player plays rock (lose)
                            if(pScript.pPlayed && pScript.myRend.sprite.name == "rock")
                            {
                                PlayerLoss();
                            }
                            //player plays paper (tie)
                            if(pScript.pPlayed && pScript.myRend.sprite.name == "paper")
                            {
                                //no audio, no point change, just move to next state
                                state = GameState.DISCARD;
                            }
                            //player plays scissors (win)
                            if(pScript.pPlayed && pScript.myRend.sprite.name == "scissors")
                            { 
                                PlayerWin();    
                            }
                        }

                        state = GameState.DISCARD;
                    }

                    if(cardScript.myRend.sprite.name == "scissors")
                    {
                        for(int p = 0; p < playerHand.Count; p ++)
                        {
                            Card pScript = playerHand[p].GetComponent<Card>();
                            //player plays rock (win)
                            if(pScript.pPlayed && pScript.myRend.sprite.name == "rock")
                            {
                                PlayerWin();
                            }
                            //player plays paper (loss)
                            if(pScript.pPlayed && pScript.myRend.sprite.name == "paper")
                            {
                                PlayerLoss();
                               
                            }
                            //player plays scissors (tie)
                            if(pScript.pPlayed && pScript.myRend.sprite.name == "scissors")
                            { 
                                 //no audio, no point change, just move to next state
                                state = GameState.DISCARD;    
                            }
                        }

                        state = GameState.DISCARD;
                    }






                }
            }
        timer = maxTimer;
        }

        
    }

    void Discard()
    {
       timer --;
       if(timer <= -85)
       {
            for(int i = 0; i < opponentHand.Count; i ++)
            {
                Card cardScript = opponentHand[i].GetComponent<Card>();
                if(cardScript.oPlayed)
                {
                    DeckManager.discard.Add(opponentHand[i]);
                    opponentHand.Remove(opponentHand[i]);
                    //play audio
                    audiosrc.clip = place;
                    audiosrc.Play();
                }
            }
       } 

       if(timer <= -100)
       {
            for(int i = 0; i < playerHand.Count; i ++)
            {
                Card cardScript = playerHand[i].GetComponent<Card>();
                if(cardScript.pPlayed)
                {
                    DeckManager.discard.Add(playerHand[i]);
                    playerHand.Remove(playerHand[i]);
                    //play audio
                    audiosrc.clip = place;
                    audiosrc.Play();
                }
            }
       }

        //moving one card at a time for some reason..?
       if(timer <= -115 && opponentHand.Count == 2)
       {
            for(int i = 0; i < opponentHand.Count; i ++)
            {
                Card cardScript = opponentHand[i].GetComponent<Card>();
                cardScript.myRend.sprite = cardScript.faceSprite;
                DeckManager.discard.Add(opponentHand[i]);
                opponentHand.Remove(opponentHand[i]);
                //play audio
                audiosrc.clip = place;
                audiosrc.Play();
            }
       }

       if(timer <= -130 && opponentHand.Count == 1)
       {
        //is this for loop necessary for one card. syd.
        //what were you cooking 
            for(int i = 0; i < opponentHand.Count; i ++)
            {
               Card cardScript = opponentHand[i].GetComponent<Card>();
                cardScript.myRend.sprite = cardScript.faceSprite;
                DeckManager.discard.Add(opponentHand[i]);
                opponentHand.Remove(opponentHand[i]);
                //play audio 
                audiosrc.clip = place;
                audiosrc.Play();
            }
       }

       //i dont get why i coded this this way. i do not care
       if(timer <= -145 && playerHand.Count == 2)
       {
            for(int i = 0; i < playerHand.Count; i++)
            {
                Card cardScript = playerHand[i].GetComponent<Card>();
                cardScript.myRend.sprite = cardScript.faceSprite;
                DeckManager.discard.Add(playerHand[i]);
                playerHand.Remove(playerHand[i]);
                //play audio 
                audiosrc.clip = place;
                audiosrc.Play();
            }
       }

       if(timer <= -160 && playerHand.Count == 1)
       {
            for(int i = 0; i < playerHand.Count; i++)
            {
                Card cardScript = playerHand[i].GetComponent<Card>();
                cardScript.myRend.sprite = cardScript.faceSprite;
                DeckManager.discard.Add(playerHand[i]);
                playerHand.Remove(playerHand[i]);
                //play audio
                audiosrc.clip = place;
                audiosrc.Play();
            }
       }

        
       for(int i = 0; i < DeckManager.discard.Count; i ++)
       {
        int depth = i;
        Card cardScript = DeckManager.discard[i].GetComponent<Card>();
        cardScript.targetPos = new Vector3(discardPos.transform.position.x,
                                            discardPos.transform.position.y - (0.05f*i),
                                            -i);
        }
        
       if(timer <= -175)
       {
            if(DeckManager.deck.Count > 0)
            {
                state = GameState.ODEAL;
            }
            else
            {
                state = GameState.RESHUFFLE;
            }

            timer = maxTimer;
       }

    }
   
   void Reshuffle()
   {
        timer --;
        if(timer == 5)
        {
            audiosrc.clip = shuffle;
            audiosrc.Play();
        }
        for(int i = 0; i < DeckManager.discard.Count; i++)
        {
            Card cardScript = DeckManager.discard[i].GetComponent<Card>();
            cardScript.oPlayed = false;
            cardScript.pPlayed = false;
            cardScript.inHand = false;
            if(timer < 0 - i*2)
            {
                cardScript.myRend.sprite = cardScript.backSprite;
                cardScript.targetPos = new Vector3(deckPos.transform.position.x,
                                                deckPos.transform.position.y - (0.05f*i),
                                                deckPos.transform.position.z);
            }
            if(timer < -90 - i)
            {
                DeckManager.discard.Reverse();
                DeckManager.deck.Add(DeckManager.discard[i]);
                DeckManager.discard.Remove(DeckManager.discard[i]);
            }
        }
                                    //why wont it let me use deckCount
        if(DeckManager.deck.Count == 24)
        {
            for(int i = 0; i < DeckManager.deck.Count; i ++)
            {
                if(timer < -120 - i*2)
                {
                    Card cardScript = DeckManager.deck[i].GetComponent<Card>();
                    cardScript.targetPos = new Vector3(deckPos.transform.position.x,
                                                deckPos.transform.position.y - (0.05f*i),
                                                i);
                }
            }

            if(timer < -240)
            {
                state = GameState.ODEAL;
            }
        }
   }
    void PlayerWin()
    {
        //play audio win
        audiosrc.clip = win;
        audiosrc.Play();
        //give player point 
        pScore ++;

        Debug.Log("player won btw");
    }
    void PlayerLoss()
    {
        //play audio lose
        audiosrc.clip = lose;
        audiosrc.Play();
        //give opponent point
        oScore ++;
        Debug.Log("player u SUK!!!!");
                               
    }
}
