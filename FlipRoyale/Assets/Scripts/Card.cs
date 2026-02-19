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
    private RectTransform rectTransform;

    // Start is called before the first frame update
    void Start()
    {
        cardImage = gameObject.GetComponent<Image>();
        tween = TweenManager.Instance;
        rectTransform = gameObject.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartCoroutine(ShowCardAnim());
    }

    private IEnumerator ShowCardAnim()
    {
        tween.Scale(rectTransform, new Vector3(0,1,1), 0.25f);
        yield return new WaitForSeconds(0.25f);
        ShowCardFront();
        tween.Scale(rectTransform, Vector3.one,0.25f);

        yield return null;
    }


    public void ShowCardFront() => cardImage.sprite = cardSprite;
    public void ShowCardBack() => cardImage.sprite = cardBackSprite;

}
