using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

//each interactive object have an action and fire when hitted to that object
public class InteractiveObject : Object
{
    public static bool AddForced;
    public static bool PassCoolDown;

    public InteractiveObjectType Type;

    public Renderer Renderer;
    public Collider Collider;
    public Action<Object> OnHitAction;
    public Action<Object> OnPassAction;
    public Action<Object> OnFoulAction;
    public Action<Object> OnGoalAction;


    public InteractiveObject(InteractiveObjectType type)
    {
        Type = type;
    }

    public void Initialize(InteractiveObjectType type, Color color = new Color(), Action<Object> onHitAction = null,
        Action<Object> onPassAction = null, Action<Object> onGoalAction = null, Action<Object> onFoulAction = null)
    {
        Type = type;
        Color = color;
        OnHitAction = onHitAction;
        OnPassAction = onPassAction;
        OnGoalAction = onGoalAction;
        OnFoulAction = onFoulAction;

        if (type != InteractiveObjectType.Gap)
        {
            Renderer.enabled = true;
            Collider.isTrigger = false;
        }
        else
        {
            Color = Color.white;
            Renderer.enabled = false;
            Collider.isTrigger = true;
        }
    }

    public void AddListenerToOnHit(Action<Object> hitAction)
    {
        OnHitAction += hitAction;
    }

    public void AddListenerToOnPass(Action<Object> passAction)
    {
        OnPassAction += passAction;
    }

    public void AddListenerToOnFoul(Action<Object> foulAction)
    {
        OnFoulAction += foulAction;
    }

    public void AddListenerToOnGoal(Action<Object> goalAction)
    {
        OnGoalAction += goalAction;
    }

    public void OnCollisionEnter(Collision other)
    {
        if (Type == InteractiveObjectType.Neutral)
        {
            if (!AddForced)
            {
                AddForced = true;
                OnHitAction?.Invoke(other.gameObject.GetComponent<Object>());
            }

            DOVirtual.DelayedCall(.5f, () => { AddForced = false; });
        }
        else if (Type == InteractiveObjectType.Goal)
        {
            OnGoalAction?.Invoke(other.gameObject.GetComponent<Object>());
        }
        else if (Type == InteractiveObjectType.Foul)
        {
            OnFoulAction?.Invoke(other.gameObject.GetComponent<Object>());
        }
    }


    public void OnTriggerExit(Collider other)
    {
        if (!PassCoolDown)
        {
            PassCoolDown = true;
            OnPassAction?.Invoke(this);
        }

        DOVirtual.DelayedCall(.1f, () => { PassCoolDown = false; });
    }
}