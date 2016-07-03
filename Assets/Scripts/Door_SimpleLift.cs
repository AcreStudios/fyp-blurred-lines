using UnityEngine;
using System.Collections;

public class Door_SimpleLift : MonoBehaviour
{
	public Transform openedPos1;
	public Transform closedPos1;
	public Transform openedPos2;
	public Transform closedPos2;

	[Header("Sliding Door")]
	public GameObject doorModel1;
	public GameObject doorModel2;
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
			if(doorModel1.transform.position.x != openedPos1.position.x) // Closed
			{
				StartCoroutine(MoveDoor(doorModel1, openedPos1));
				StartCoroutine(MoveDoor(doorModel2, openedPos2));
			}
				
			else
			{
				StartCoroutine(MoveDoor(doorModel1, closedPos1));
				StartCoroutine(MoveDoor(doorModel2, closedPos2));
			}
		}
		else // Idk why but this is switched???
		{
			StartCoroutine(MoveDoor(doorModel1, closedPos1));
			StartCoroutine(MoveDoor(doorModel2, closedPos2));

			yield return new WaitForSeconds(doorCloseDelay);

			StartCoroutine(MoveDoor(doorModel1, openedPos1));
			StartCoroutine(MoveDoor(doorModel2, openedPos2));
		}	
	}

	IEnumerator MoveDoor(GameObject doorModel, Transform moveToPos)
	{
		while(doorModel.transform.position.x != moveToPos.position.x)
		{
			doorModel.transform.position = Vector3.MoveTowards(doorModel.transform.position, moveToPos.position, slideSpeed * Time.smoothDeltaTime);
			yield return null;
		}
	}
}
