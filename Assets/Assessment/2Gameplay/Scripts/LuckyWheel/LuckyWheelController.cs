using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Server.API;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//The controller class of the Lucky Wheel
public class LuckyWheelController : MonoBehaviour
{
    //For check on reset don't initialized founded components
    public static bool IsInitialized;

    //The rigid body of Wheel
    [SerializeField] private Rigidbody2D mWheelRiRigidbody2D;

    //Obstacles parent to get Obstacle Object
    [SerializeField] private Transform m_obstaclesParent;

    //for store the little wheel colliders 
    [SerializeField] private List<ObstacleObject> m_obstacles;

    //Preset Animation curve for rotating movement of main wheel    
    [SerializeField] private AnimationCurve spinAnimationCurve;

    //for detect wheel spinning or not
    [SerializeField] private bool m_spinning;

    //calculate the angle of each item in lucky wheel
    [SerializeField] private float m_anglePerItem;
    [SerializeField] private int m_itemNumber;

    [SerializeField] private Button m_spinClaimButton;

    [SerializeField] private CanvasGroup m_canvasGroup;

    public Button SpinClaimButton
    {
        get => m_spinClaimButton;
    }

    [SerializeField] private TextMeshProUGUI m_spinClaimTextButton;

    //canvas for put the claimed item to front of other UI (change Z order of UI element) 
    [SerializeField] private Canvas animationCanvas;

    //list of transforms as a movement path for claim movement
    [SerializeField] private List<Transform> moveOutTransforms;

    //store the last item when indicator hit with it 
    [SerializeField] private ObstacleObject lastHittedItem;
    [SerializeField] private ObstacleObject lastHittedItemForPunchAnimation;

    [SerializeField] private float m_dependItemTime;

    //action for using after spin animation finished
    private Action<short> m_afterSpinFinishedCallback = null;

    
    //the initializer of the class, used in reset functionality when re enabling the gameObject of the lucky wheel
    public void Initialize(Action<short> afterSpinFinishedCallBack)
    {
        if (!IsInitialized)
        {
            m_obstacles = m_obstaclesParent.GetComponentsInChildren<ObstacleObject>().ToList();
            m_obstacles.ForEach(x =>
            {
                x.Initialize();
                x.OnCollisionEnterEvent = null;
                x.OnCollisionEnterEvent += UpdateLastHittedItem;
            });
            m_anglePerItem = 360 / m_obstacles.Count;
            IsInitialized = true;
        }

        m_spinning = false;
        m_spinClaimTextButton.text = "Spin";
        m_spinClaimButton.onClick.RemoveAllListeners();
        m_spinClaimButton.onClick.AddListener(Spin);
        m_spinClaimButton.interactable = true;
        m_afterSpinFinishedCallback = afterSpinFinishedCallBack;
    }

    //Show the UI of Lucky Wheel with simple animation using DoTween
    public void Show(Transform endTransform)
    {
        m_canvasGroup.alpha = 0;
        m_canvasGroup.gameObject.SetActive(true);
        m_canvasGroup.DOFade(1, .5f).SetDelay(.5f);
        m_canvasGroup.transform.DOMove(endTransform.position, 1.5f).SetEase(Ease.OutBack);
    }

    public void Hide()
    {
        m_canvasGroup.DOFade(0, .5f);
    }


    public void Spin()
    {
        m_spinClaimButton.interactable = false;

        m_itemNumber = Random.Range(0, m_obstacles.Count);
        
        float maxAngle = 360 * m_dependItemTime + (m_itemNumber * m_anglePerItem);
        m_itemNumber = m_itemNumber == 0 ? 0 : (m_obstacles.Count + 1) - (m_itemNumber);


        StartCoroutine(SpinTheWheel(5f * m_dependItemTime, -maxAngle, spinAnimationCurve));
    }

    public void Reset()
    {
        if (lastHittedItem != null)
        {
            mWheelRiRigidbody2D.transform.eulerAngles = Vector3.zero;
        }

        Initialize( m_afterSpinFinishedCallback);
    }

    public void UpdateLastHittedItem(ObstacleObject obstacleObject)
    {
        lastHittedItem = obstacleObject;
    }

    //a coroutine for rotating the wheel over the time used animation curve
    IEnumerator SpinTheWheel(float time, float maxAngle, AnimationCurve animationCurve)
    {
        m_spinning = true;

        float timer = 0.0f;
        float startAngle = mWheelRiRigidbody2D.transform.eulerAngles.z;
        maxAngle = maxAngle - startAngle;

        while (timer < time)
        {
            //to calculate rotation
            float angle = maxAngle * animationCurve.Evaluate(timer / time);
            mWheelRiRigidbody2D.MoveRotation(angle + startAngle);
            timer += Time.deltaTime;
            yield return 0;
            if (lastHittedItem != null && lastHittedItem != lastHittedItemForPunchAnimation)
            {
                lastHittedItem.ShowSelectionAnimation();
                lastHittedItemForPunchAnimation = lastHittedItem;
            }
        }

        lastHittedItem.ShowSelectionAnimation(true);

        m_spinning = false;

        m_afterSpinFinishedCallback?.Invoke(lastHittedItem.GiftNumber);
    }
    
    private void OnEnable()
    {
        Reset();
    }
}