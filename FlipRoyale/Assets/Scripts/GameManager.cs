using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private const string MENU_SCENE = "Menu";

    [Header("Card Grid Settings")]
    [SerializeField] private int rows = 2;
    [SerializeField] private int columns = 2;

    [Header("UI")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text comboText;
    [SerializeField] private Text turnsLeftText;

    [Space]
    [SerializeField] private GameObject cardPrefab;
    [SerializeField] private Transform cardContainer;

    [SerializeField] private List<Sprite> cardSprites;

    private int turns = 0;
    
    private List<Card> currentPair = new List<Card>();
    private List<Card> cardsList = new List<Card>();

    private bool lastCardMatched = false;
    private int score = 0;
    private int comboScoreInc = 0;

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        rows = GameSettings.Instance.GetRows();
        columns = GameSettings.Instance.GetColumns();
        turns = GameSettings.Instance.GetTurns();

        UpdateUI();
        StartCoroutine(SpawnCards());
    }

    private IEnumerator SpawnCards()
    {
        int totalCards = rows * columns;
        int pairCount = totalCards / 2;

        List<int> ids = GenerateShuffledIds(pairCount, totalCards);

        GridLayoutGroup grid = cardContainer.GetComponent<GridLayoutGroup>();
        RectTransform parentRect = cardContainer as RectTransform;

        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columns;

        float containerWidth = parentRect.rect.width;
        float containerHeight = parentRect.rect.height;

        float totalSpacingX = grid.spacing.x * (columns - 1);
        float totalSpacingY = grid.spacing.y * (rows - 1);

        float totalPaddingX = grid.padding.left + grid.padding.right;
        float totalPaddingY = grid.padding.top + grid.padding.bottom;

        float availableWidth = containerWidth - totalSpacingX - totalPaddingX;
        float availableHeight = containerHeight - totalSpacingY - totalPaddingY;

        float maxCellWidth = availableWidth / columns;
        float maxCellHeight = availableHeight / rows;

        float aspectRatio = cardSprites[0].rect.width / cardSprites[0].rect.height; // general card ratio

        float cellWidth = maxCellWidth;
        float cellHeight = cellWidth / aspectRatio;

        if (cellHeight > maxCellHeight)
        {
            cellHeight = maxCellHeight;
            cellWidth = cellHeight * aspectRatio;
        }

        grid.cellSize = new Vector2(cellWidth, cellHeight);

        for (int i = 0; i < totalCards; i++)
        {
            GameObject cardObj = Instantiate(cardPrefab, cardContainer);
            int id = ids[i];
            Card card = cardObj.GetComponent<Card>();
            card.InitCard(id, cardSprites[id]);
            cardsList.Add(card);
        }

        yield return null;

        grid.enabled = false;
    }

    private List<int> GenerateShuffledIds(int pairCount, int totalCards)
    {
        List<int> ids = new List<int>(totalCards);

        int spriteCount = cardSprites.Count;

        for (int i = 0; i < pairCount; i++)
        {
            int spriteIndex = Random.Range(0, spriteCount);

            // Add a matching pair
            ids.Add(spriteIndex);
            ids.Add(spriteIndex);
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

    public void ResetCards()
    {
        //Resetting values
        comboScoreInc = 0;
        lastCardMatched = false;
        score = 0;

        UpdateUI();

        foreach (Card c in cardsList)
            c.ResetCard();
    }

    public void OnCardSelected(Card clickedCard)
    {
        if (turns < 1)
        {
            //TODO: Show Game over panel
            return;
        }

        turns--;

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
            //Increase score
            score++;
            if (lastCardMatched)
            {
                comboScoreInc++;
                score += comboScoreInc;
            }

            UpdateUI();

            first.SetIsMatched(true);
            second.SetIsMatched(true);

            lastCardMatched = true;

            yield return new WaitForSeconds(0.3f);
            first.DisableCard();
            second.DisableCard();

            //TODO:Show Game Win panel
        }
        else
        {
            //TODO: mismatch SFX

            lastCardMatched = false;
            yield return new WaitForSeconds(1f); //Adding a little delay to show the mismatch
            first.CloseCard();
            second.CloseCard();
        }

        yield return null;
    }

    private void UpdateUI()
    {
        scoreText.text = $"Score : {score}";
        comboText.text = $"Combo : +{comboScoreInc}";
        turnsLeftText.text = $"Turns Left : {turns}";
    }

    public void OnMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(MENU_SCENE);
    }

}
