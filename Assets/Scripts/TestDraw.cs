using UnityEngine;

public class TestDraw : MonoBehaviour
{
    public Deck deck;
    public Hand hand;

    private void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            CardData card = deck.Draw();
            if (card != null)
            {
                hand.AddCard(card);
            }
        }
    }
}