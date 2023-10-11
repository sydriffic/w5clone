using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    public Sprite faceSprite;
    public Sprite backSprite;

    public SpriteRenderer myRend;
    public bool mouseOver, mouseClicked;

    public Vector3 targetPos;

    public bool inHand, pPlayed, oPlayed;
    AudioSource audiosrc;

    void Start()
    {
        myRend = GetComponent<SpriteRenderer>();
        backSprite = myRend.sprite;

        //targetX = (transform.position.x;
       // targetY = transform.position.y;

       mouseOver = false;
       mouseClicked = false;

        inHand = false;
        pPlayed = false;
        oPlayed = false;

        audiosrc = GetComponent<AudioSource>();
    }

    void Update()
    {
        
        Vector3 newPos = transform.position;
        newPos = Vector3.Lerp(transform.position, targetPos, 0.2f);
        transform.position = newPos;

    }

    void OnMouseOver()
    {
        if(CardGameManager.state == CardGameManager.GameState.PCHOOSE)
        {
           
            if(inHand)
            {
                mouseOver = true;
                Debug.Log(this.faceSprite.name + " lerping Up");
                targetPos = new Vector3((transform.position.x), 
                                    -2.8f,
                                    transform.position.z);

                if (Input.GetMouseButtonDown(0))
                {
                    targetPos = new Vector3(-0.3f, 
                                        -1f,
                                        transform.position.z);
                    pPlayed = true;
                    audiosrc.Play();
                    CardGameManager.state = CardGameManager.GameState.EVAL;
                    Debug.Log(CardGameManager.state);  
                }
            }

            
        }
    }

    void OnMouseExit()
    {
        if(CardGameManager.state == CardGameManager.GameState.PCHOOSE)
        {
           
            if(inHand)
            {
                mouseOver = false;
                targetPos = new Vector3((transform.position.x), 
                                    -3f,
                                    transform.position.z);
              
            }
        }
    }

}
