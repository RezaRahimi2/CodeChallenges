using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator m_animator;
    [SerializeField] private CapsuleCollider m_capsuleCollider; 
    [SerializeField] private Transform m_characterTransform;
    [SerializeField] private PlayerCubes m_playerCubes;
    [SerializeField] private AudioSource m_audioSource;
    [SerializeField] private Material m_trialMaterial;
    public AudioSource AudioSource => m_audioSource;
    public int PlayerCubeCount => m_playerCubes.Cubes.Count;
    
    public Vector3 FirstCubeSize => m_playerCubes.Cubes[0].Size;
    private Vector3 m_newPos;

    public void Initialize(Color trialColor)
    {
        m_trialMaterial.color = trialColor;
        m_playerCubes.Initialize();
    }
    
    public void SetPosition(Vector3 newPos)
    {
        transform.position =  new Vector3(transform.position.x,transform.position.y,newPos.z);
    }

    public void AddCubeToPlayer(CollectableCube collectableCube)
    {
        m_animator.SetBool("Falling",true);
        m_characterTransform.position = new Vector3(m_characterTransform.position.x, m_characterTransform.position.y + collectableCube.Size.y * 1.2f,
            m_characterTransform.position.z);
        m_playerCubes.AddCube(collectableCube);
        StopAllCoroutines();
        StartCoroutine(FallingFinish());
    }

    public void RemoveCubeFromPlayer(PlayerCube collectableCubeTransform)
    {
        m_playerCubes.RemoveCube(collectableCubeTransform);
    }

    private IEnumerator FallingFinish()
    {
        Ray ray = new Ray(m_capsuleCollider.transform.position, -transform.up);
        yield return new WaitWhile(()=>!Physics.Raycast(ray,2));
        m_animator.SetBool("Falling",false);
    }

    public void GameIsOver(bool playerIsWinner)
    {
        if(playerIsWinner)
            m_animator.SetBool("Win",true);
        else
            m_animator.SetBool("Lose",true);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(m_capsuleCollider.transform.position,-transform.up);
    }

}
