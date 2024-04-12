using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

public class InputController : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        bool mouseDown = false;
        bool startScrabing = false;
        
        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButton(0)).Subscribe(unit =>
            {
                mouseDown = true;
            });

        this.UpdateAsObservable()
            .Where(_ => Input.GetMouseButtonUp(0))
            .Subscribe((unit) =>
            {
                mouseDown = false;
            });
        
        this.UpdateAsObservable()
            .SampleFrame(4)
            .Subscribe(x =>
                {
                    if (mouseDown)
                    {
                        if(!startScrabing)
                            GameManager.Instance.StartScraping();
                        
                        startScrabing = true;
                        GameManager.Instance.GenerateSpiral();
                    }
                    else if(startScrabing)
                    {
                        GameManager.Instance.StopScraping();
                        startScrabing = false;
                    }
                },
                () => Debug.Log("destroy"));
    }
    
}