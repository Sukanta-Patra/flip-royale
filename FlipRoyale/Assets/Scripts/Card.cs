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

    // Start is called before the first frame update
    void Start()
    {
        cardImage = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        cardImage.sprite = cardSprite;
    }

}
