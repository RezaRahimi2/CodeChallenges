using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{
    [SerializeField] private Rigidbody m_rigidBody;
    [SerializeField] private Animator m_animator;
    [SerializeField] private CapsuleCollider m_capsuleCollider;
    //used when the player hit to jump platform and goes up
    [SerializeField] private bool m_isJump;
    //used for show landing point of the player
    [SerializeField] private TubeRenderer m_lineRenderer;
    //stored the layer of the jump platform
    [SerializeField] private LayerMask m_platformLayerMask;
    //used when the player wants to hit the final platform 
    [SerializeField] private bool m_fallInFinalPlatform;
    //Material of Tube renderer
    [SerializeField] private Material m_rayHitMaterial;
    //used for active and deactive the line detector of the player 
    [SerializeField] private Transform m_hitPointer;
    //store the trails renderer
    [SerializeField] private TrailRenderer m_leftHandTrailRenderer;
    [SerializeField] private TrailRenderer m_rightHandTrailRenderer;
    //used for detecting player do acrobat movement
    private bool m_onAcrobatMovement;
    private Tween delayTween;

    // set the initialize value of player
    public void Initialize(Vector3 playerStartPosition,Vector3 playerStartRotation,LayerMask platformLayerMask)
    {
        m_animator.SetBool($"AcrobatMovement1",false); 
        m_animator.SetBool($"AcrobatMovement2",false);
        m_animator.SetBool($"AcrobatMovement3",false); 
        m_animator.SetBool("Falling",false);
        m_animator.SetBool("Victory",false);
        m_hitPointer.gameObject.SetActive(true);
        m_lineRenderer.enabled = true;
        transform.localPosition = playerStartPosition;
        transform.localEulerAngles = playerStartRotation;
        m_platformLayerMask = platformLayerMask;
        m_fallInFinalPlatform = false;
        m_rigidBody.useGravity = true;
    }
    
    private void Update()
    {
        // create a ray for detecting platform and change the color of the line renderer depend of that
        Ray ray = new Ray( new Vector3(transform.position.x,transform.position.y,transform.position.z), -transform.up);
        Debug.DrawRay(ray.origin,ray.direction * 100,new Color(1,0,0,1));
        m_lineRenderer.SetPosition(0,new Vector3(ray.origin.x,ray.origin.y + (m_capsuleCollider.height /2.5f ),ray.origin.z));

        RaycastHit raycastHit;

        if (Physics.Raycast(ray, out raycastHit, 100,m_platformLayerMask))
        {
            m_rayHitMaterial.color = new Color(0,.3f,1,.66f);
            m_lineRenderer.SetPosition(1,raycastHit.point);
            
            m_hitPointer.position = new Vector3(raycastHit.point.x,raycastHit.point.y + .3f,raycastHit.point.z) ;
            m_hitPointer.gameObject.SetActive(true);
            // show trails when player want to hit the final platform
            if (!m_isJump && raycastHit.transform.CompareTag("FinalPlayform"))
            {
                m_fallInFinalPlatform = true;
                m_animator.SetBool("Falling",false);
                m_leftHandTrailRenderer.enabled = true;
                m_rightHandTrailRenderer.enabled = true;
            }
        }
        else
        {
            m_rayHitMaterial.color = new Color(1,0,0,.66f);
            m_lineRenderer.SetPosition(1,new Vector3(ray.origin.x,ray.origin.y - 100,ray.origin.z) );
            m_hitPointer.gameObject.SetActive(false);
        }
        
        //if player goes up show acrobat movement randomly
        if (m_rigidBody.velocity.y > 1 && !m_onAcrobatMovement)
        {
            m_onAcrobatMovement = true;
            //We can use Random value for choosing acrobat movement randomly
            int rndValue = Random.Range(1,4);
            // set delay time to 1.5f if movement is flip back animation and set it to .5f if movement is flip forward aniamtion
            float delay = rndValue == 1?1f:.65f;
            //increase the delay time if acrobat movement is combination of flip back and forward
            if (rndValue == 3)
            {
                delay += 1;
            }
            delayTween.Kill();
            m_animator.SetBool($"AcrobatMovement{rndValue}",true); 
            //after delay time set false the animator parameter
            delayTween = DOVirtual.DelayedCall(delay, () =>
            {
                m_animator.SetBool($"AcrobatMovement{rndValue}",false);
            });
        }
        
        // if player move down show falling animation
        if (m_rigidBody.velocity.y < -1 && m_isJump && !m_fallInFinalPlatform)
        {
            m_isJump = false;
            m_animator.SetBool($"AcrobatMovement1",false);
            m_animator.SetBool($"AcrobatMovement2",false);
            m_animator.SetBool($"AcrobatMovement3",false);
            m_animator.SetBool("Falling",true);
        }
        //disable the trails after certain down speed
        else if(m_rigidBody.velocity.y < -8)
        {
            m_leftHandTrailRenderer.enabled = false;
            m_rightHandTrailRenderer.enabled = false;
            m_leftHandTrailRenderer.time = 0;
            m_rightHandTrailRenderer.time = 0;
        }
    }

    // add up force and show trails when character hit to jump platform
    public void AddForce(float force)
    {
        m_leftHandTrailRenderer.enabled = true;
        m_rightHandTrailRenderer.enabled = true;
        m_leftHandTrailRenderer.time = 2;
        m_rightHandTrailRenderer.time = 2;
        
        m_isJump = true;
        m_onAcrobatMovement = false; 
        m_rigidBody.AddForce(Vector3.up * force, ForceMode.Impulse);
        m_animator.SetBool($"AcrobatMovement1",false);
        m_animator.SetBool($"AcrobatMovement2",false);
        m_animator.SetBool($"AcrobatMovement3",false);
        m_animator.SetBool("Falling",false);
    }

    //deactive hit pointer and line renderer and rigidbody and show victory animation when player landing on final platform
    public void FinishLevel()
    {
        m_hitPointer.gameObject.SetActive(false);
        m_lineRenderer.enabled = false;
        m_rigidBody.useGravity = false;
        m_animator.SetBool("Victory",true);
    }
}
