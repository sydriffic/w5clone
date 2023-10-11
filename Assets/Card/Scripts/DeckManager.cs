using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class DeckManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public Sprite[] cardFaces;
    public int deckCount;
    public static List<GameObject> deck = new List<GameObject>();
    public static List<GameObject> discard = new List<GameObject>();

    void Start()
    {
        deckCount = 24;
        Application.targetFrameRate = 60;
        for(int i = 0; i < deckCount; i++)
        {
            GameObject newCard = Instantiate(cardPrefab, gameObject.transform);
            Card newCardScript = newCard.GetComponent<Card>();
            newCardScript.faceSprite = cardFaces[i % 3];
            deck.Add(newCard);
            newCardScript.targetPos = new Vector3(transform.position.x,
                                                transform.position.y - (0.05f*i) - 0.5f,
                                                i);
        }

        //shuffling the deck
        for (int i = 0; i < deck.Count; i++)
        {
            GameObject temp = deck[i];
            int randomIndex = Random.Range(i, deck.Count);
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }

        //putting things back where they belong
        for(int i = 0; i < deck.Count; i ++)
        {
            GameObject newCard = deck[i];
            Card newCardScript = newCard.GetComponent<Card>();
            newCardScript.targetPos = new Vector3(transform.position.x,
                                                transform.position.y - (0.05f*i) - 0.5f,
                                                i);
        }
    }
}
