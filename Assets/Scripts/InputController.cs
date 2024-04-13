using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private bool startDragging;
    private float startMouseXPosition;
    
    [SerializeField] private Player m_player;
    [SerializeField] private BoxCollider m_groundCollider;
    
    public void Initialize(Player player,BoxCollider groundCollider)
    {
        m_player = player;
        m_groundCollider = groundCollider;
        
        bool _mouseDown = false;
        bool startScrabing = false;
        
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButton(0)).Subscribe(unit =>
            {
                _mouseDown = true;
            });

        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonUp(0))
            .Subscribe((unit) =>
            {
                _mouseDown = false;
            });

        this.LateUpdateAsObservable()
            .Select(_ => Input.mousePosition)
            .Subscribe(mousePosition =>
            {
                if (!GameManager.GameStarted && _mouseDown)
                {
                    GameManager.Instance.StartGame();
                }
                
                if (_mouseDown)
                {
                    float x = startMouseXPosition - mousePosition.x;
            
                    if (x != 0)
                    {
                        Vector3 newPos = new Vector3(
                            m_player.transform.position.x,
                            m_player.transform.position.y,
                            Mathf.Clamp( m_player.transform.position.z + x * .0007f,
                                m_groundCollider.bounds.min.z + m_player.FirstCubeSize.x /2 ,
                                m_groundCollider.bounds.max.z - + m_player.FirstCubeSize.x /2));
                
                        GameManager.Instance.GetInput(newPos);
                    }
                }
        
                if (_mouseDown && !startDragging)
                {
                    startDragging = true;
                    startMouseXPosition = mousePosition.x;
                }

                if (!_mouseDown)
                {
                    startDragging = false;
                    startMouseXPosition = mousePosition.x;
                }
            });

    }
}
