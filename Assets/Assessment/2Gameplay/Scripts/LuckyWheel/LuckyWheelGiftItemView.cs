using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//the view of gift item 
public class LuckyWheelGiftItemView : MonoBehaviour
{
    [SerializeField] private Transform m_backTransform;
    [SerializeField] private TextMeshProUGUI m_giftNumberText;

    public TextMeshProUGUI GiftNumberText
    {
        get => m_giftNumberText;
    }
    
    public void Initialize(out short giftNumber)
    {
        m_giftNumberText = GetComponentInChildren<TextMeshProUGUI>();
        short.TryParse(m_giftNumberText.text, out giftNumber);
    }

    public void ShowAnimation(bool isLastHit)
    {
        Vector2 scale = new Vector2(.2f, .2f);
        
        if(isLastHit)
            scale =new Vector2(.5f, .5f);
        
        m_backTransform.DOPunchScale(scale, isLastHit?.5f:.25f,isLastHit?8:4);
    }
}