using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Card Grid Settings")]
    [SerializeField] private int rows = 2;
    [SerializeField] private int columns = 2;

    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardContainer;

    [SerializeField] private List<Sprite> cardSprites;

    private List<Card> currentPair = new List<Card>();

    public float spacing = 10f;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
       StartCoroutine(SpawnCards());
    }

    private IEnumerator SpawnCards()
    {
        int totalCards = rows * columns;
        int pairCount = totalCards / 2;

        if (pairCount > cardSprites.Count)
        {
            Debug.LogError("Not enough sprites for this grid size");
            yield break;
        }

        List<int> ids = GenerateShuffledIds(pairCount, totalCards);
        RectTransform parentRect = cardContainer as RectTransform;

        GridLayoutGroup grid = cardContainer.GetComponent<GridLayoutGroup>();
        grid.constraintCount = columns;

        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);          
            int id = ids[i];
            cardObj.GetComponent<Card>().InitCard(id, cardSprites[id]);
        }

        yield return null;

        grid.enabled = false;
    }

    private List<int> GenerateShuffledIds(int pairCount, int totalCards)
    {
        List<int> ids = new List<int>(totalCards);
        List<int> available = new List<int>();

        for (int i = 0; i < cardSprites.Count; i++)
            available.Add(i);

        Shuffle(available);

        for (int i = 0; i < pairCount; i++)
        {
            int id = available[i];
            ids.Add(id);
            ids.Add(id);
        }
        Shuffle(ids);

        return ids;
    }

    private void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rand = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[rand];
            list[rand] = temp;
        }
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
