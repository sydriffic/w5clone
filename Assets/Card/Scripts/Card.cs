using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Sprite faceSprite;
    Sprite backSprite;

    public SpriteRenderer myRend;
    bool mouseOver = false;

    public Vector3 targetPos;

    bool inHand, pPlayed, oPlayed;

    void Start()
    {
        myRend = GetComponent<SpriteRenderer>();
        backSprite = myRend.sprite;

        //targetX = (transform.position.x;
       // targetY = transform.position.y;

        inHand = false;
        pPlayed = false;
        oPlayed = false;
    }

    void Update()
    {
        
        Vector3 newPos = transform.position;
        newPos = Vector3.Lerp(transform.position, targetPos, 0.2f);
        transform.position = newPos;
        
        if(mouseOver)
        {
            myRend.sprite = faceSprite;
        }

    }
    void OnMouseDown()
    {
        mouseOver = true;
    }
}
