using UnityEngine;
using System.Collections;

public class PivotRotation : MonoBehaviour 
{
	public Vector3 rotationAxis = Vector3.up;
	public float rotationSpeed = 1f;
	

	[ContextMenu("Move")]
	public void SplineMove()
	{
	}

	[ContextMenu("MoveSC")]
	public void SplineMoveSC()
	{
		
	}
	
	[ContextMenu("MoveCP")]
	public void SplineMoveCP()
	{
		float delay = 0;
	}
	
	void Update () 
	{
		//transform.Rotate(rotationAxis * rotationSpeed * Time.deltaTime);
	}
}
