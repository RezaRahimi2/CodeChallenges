using UnityEngine;

public class ReduceSize : MonoBehaviour
{
    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Scraper"))
            transform.parent.localScale -= Vector3.forward * .01f;
    }
}
