using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    public Sprite faceSprite;
    Sprite backSprite;

    public SpriteRenderer myRend;
    public bool mouseOver;

    public Vector3 targetPos;

    public bool inHand, pPlayed, oPlayed;

    void Start()
    {
        myRend = GetComponent<SpriteRenderer>();
        backSprite = myRend.sprite;

        //targetX = (transform.position.x;
       // targetY = transform.position.y;

       mouseOver = false;

        inHand = false;
        pPlayed = false;
        oPlayed = false;
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
