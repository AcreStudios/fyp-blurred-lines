using UnityEngine;
using System.Collections;

public class Door_SimpleSlide : MonoBehaviour
{
	public Transform openedPos;
	public Transform closedPos;

	[Header("Sliding Door")]
	public GameObject doorModel;
	public float slideSpeed = 5f;

	[Header("Automatic Sliding Door?")]
	public bool automaticDoor = false;
	public float doorCloseDelay = 3f;
	
	public void DoorInteract()
	{
		StartCoroutine(DoorInteraction());
	}

	IEnumerator DoorInteraction()
	{
		if(!automaticDoor)
		{
			if(doorModel.transform.position.x != openedPos.position.x) // Closed
				StartCoroutine(MoveDoor(openedPos));
			else
				StartCoroutine(MoveDoor(closedPos));
		}
		else // Idk why but this is switched???
		{
			StartCoroutine(MoveDoor(closedPos));

			yield return new WaitForSeconds(doorCloseDelay);

			StartCoroutine(MoveDoor(openedPos));
		}	
	}

	IEnumerator MoveDoor(Transform moveToPos)
	{
		while(doorModel.transform.position.x != moveToPos.position.x)
		{
			doorModel.transform.position = Vector3.MoveTowards(doorModel.transform.position, moveToPos.position, slideSpeed * Time.deltaTime);
			yield return null;
		}
	}
}
