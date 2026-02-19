using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerDownHandler
{

    [SerializeField] private Sprite cardBackSprite;

    [SerializeField] private Sprite cardSprite;

    private Image cardImage;
    private TweenManager tween;
    private GameManager gameManager;
    private RectTransform rectTransform;
    private bool isMatched = false;
    private bool isShowing = false;
    private bool readyToMatch = false;

    [SerializeField] private int cardId;

    // Start is called before the first frame update
    void Start()
    {
        cardImage = gameObject.GetComponent<Image>();
        tween = TweenManager.Instance;
        gameManager = GameManager.Instance;
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isShowing || isMatched) return; //TODO: Add Game Over condition later

        gameManager.OnCardSelected(this);
        StartCoroutine(ShowCardAnim());
    }

    private IEnumerator ShowCardAnim()
    {
        isShowing = true;
        tween.Scale(rectTransform, new Vector3(0,1,1), 0.15f);
        yield return new WaitForSeconds(0.15f);
        ShowCardFront();
        tween.Scale(rectTransform, Vector3.one,0.15f);
        readyToMatch = true;
    }

    public void CloseCard()
    {
        if (!isShowing || isMatched) return; // Prevent closing if the card is not showing
        StartCoroutine(CloseCardCoroutine());
    }
    private IEnumerator CloseCardCoroutine()
    {
        tween.Scale(rectTransform, new Vector3(0,1,1), 0.15f);
        yield return new WaitForSeconds(0.15f);

        // Reset to cardBack image
        ShowCardBack();
        tween.Scale(rectTransform, Vector3.one, 0.15f);

        isShowing = false;
        readyToMatch = false;
    }

    public void DisableCard()
    {
        isMatched = true; //Safe check to ensure the card is not matched again
        tween.SetOnComplete(tween.Scale(rectTransform, Vector3.zero, 0.15f), () => gameObject.SetActive(false));
    }


    public void ShowCardFront() => cardImage.sprite = cardSprite;
    public void ShowCardBack() => cardImage.sprite = cardBackSprite;
    public int GetCardId() => cardId;
    public void SetCardId(int _cardId) => cardId = _cardId;
    public bool GetIsMatched() => isMatched;
    public void SetIsMatched(bool _isMatched) => isMatched = _isMatched;
    public bool GetIsShowing() => isShowing;
    public void SetIsShowing(bool _isShowing) => isShowing = _isShowing;
    public bool GetIsReadyToMatch() => readyToMatch;
    public void SetIsReadyToMatch(bool _readyToMatch) => readyToMatch = _readyToMatch;


}
