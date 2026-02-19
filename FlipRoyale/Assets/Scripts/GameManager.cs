using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private List<Card> currentPair = new List<Card>();

    private void Awake()
    {
        Instance = this;
    }


    public void OnCardSelected(Card clickedCard)
    {
        currentPair.Add(clickedCard);

        if (currentPair.Count >= 2)
        {
            // Copy the pair to local variables so further clicks won't interfere
            Card first = currentPair[0];
            Card second = currentPair[1];
            currentPair = new List<Card>();

            StartCoroutine(CheckMatch(first, second));
        }
    }

    private IEnumerator CheckMatch(Card first, Card second)
    {

        yield return new WaitUntil(() => first.GetIsReadyToMatch() && second.GetIsReadyToMatch());

        if (first.GetCardId() == second.GetCardId())
        {
            //TODO: match SFX
            //TODO: Increase score

            first.SetIsMatched(true);
            second.SetIsMatched(true);

            yield return new WaitForSeconds(0.3f);
            first.DisableCard();
            second.DisableCard();
        }
        else
        {
            //TODO: mismatch SFX

            yield return new WaitForSeconds(1f); //Adding a little delay to show the mismatch
            first.CloseCard();
            second.CloseCard();
        }

        yield return null;
    }


}
