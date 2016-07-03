using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]

public class AITemplate : MonoBehaviour {

    public enum AIState {
        Patrol, Alert, Search
    }

    #region Inspector Variables
    public Transform[] patrol;
    public GameObject vizor;
    public GameObject UICanvas;
    public float delayTime;
    public float speed;
    public float range;
    public Transform head;
    #endregion

    #region Script Variables
    public AIState currentState;
    int currentPatrolInt;
    bool delayOver;
    bool accending;

    Transform target;
    RaycastHit hit;
    Vector3 targetLastKnown;
    Vector3 randomLocation;
    bool searchExpire;
    float currentR;
    float targetR;
    AITemplate instance;
    int lineOfSightTime;
    int changeValue;
    NavMeshAgent agent;
    #endregion

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        currentPatrolInt = 0;
        delayOver = true;
        currentState = AIState.Patrol;
    }

    void Update() {
        switch (currentState) {
            case AIState.Patrol:
                if (patrol.Length > 0) {
                    if ((patrol[currentPatrolInt].position - transform.position).magnitude < 0.5f) {

                        if (currentPatrolInt == patrol.Length - 1)
                            changeValue = -1;

                        if (currentPatrolInt == 0)
                            changeValue = 1;

                        currentPatrolInt += changeValue;
                        StartCoroutine(Delay(delayTime));

                    } else {
                        if (delayOver) {

                            agent.destination = patrol[currentPatrolInt].position;
                            transform.LookAt(patrol[currentPatrolInt]);
                            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                            head.transform.eulerAngles = transform.eulerAngles;
                        } else {
                            LookAround();
                        }
                    }
                }

                if (target) {
                    currentState = AIState.Alert;
                }
                break;

            case AIState.Alert:
                if (target) {
                    if ((target.position - transform.position).magnitude < range) {
                        agent.destination = transform.position;
                        Shooting(1);
                    } else {
                        agent.destination = target.position;
                    }
                    head.transform.LookAt(target);
                } else {
                    StartCoroutine(Delay(5));
                    randomLocation = targetLastKnown;
                    currentState = AIState.Search;
                }
                break;

            case AIState.Search:
                if (!delayOver) {
                    RadiusPatrol(targetLastKnown, 10);
                    LookAround();
                    if (target)
                        currentState = AIState.Alert;
                } else {
                    currentState = AIState.Patrol;
                }
                break;
        }

        if (lineOfSightTime > 0) {
            lineOfSightTime--;

            if (lineOfSightTime == 0)
                target = null;
        }
    }

    IEnumerator Delay(float time) {
        delayOver = false;
        yield return new WaitForSeconds(time);
        delayOver = true;
    }

    public void Alert(Transform player) {
        if (Physics.Linecast(head.transform.position, player.position, out hit)) {
            if (hit.transform.root == player) {
                target = player;
                targetLastKnown = target.position;
                lineOfSightTime = 2;
            }
        }
    }

    public void Shooting(float randomValue) {
        Vector3 storage = new Vector3(Random.Range(-randomValue, randomValue), Random.Range(-randomValue, randomValue), Random.Range(-randomValue, randomValue));
        Debug.DrawRay(transform.position + new Vector3(0, transform.localScale.y / 4, 0), head.transform.TransformDirection(0, 0, range) + storage, Color.red, 2f);
    }

    public void LookAround() {
        if (currentR == targetR) {
            targetR = Random.Range(-60, 60);
        } else {
            if (currentR > targetR) {
                currentR--;
            }
            if (currentR < targetR) {
                currentR++;
            }
        }
        head.transform.localEulerAngles = new Vector3(0, 0 + currentR, 0);
    }

    public void RadiusPatrol(Vector3 radiusCentre, float radius) {
        if ((transform.position - randomLocation).magnitude < 0.1)
            randomLocation = new Vector3(radiusCentre.x + Random.Range(-radius, radius), radiusCentre.y, radiusCentre.z + Random.Range(-radius, radius));
        agent.SetDestination(randomLocation);
    }

    public void Death()
	{
        Destroy(vizor);
        Destroy(UICanvas);

		// Add rigidbody to head
		Rigidbody headRb = head.gameObject.AddComponent<Rigidbody>();
		headRb.AddTorque(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));

		// Add rigidbody to body
		Rigidbody bodyRb = gameObject.AddComponent<Rigidbody>();
		bodyRb.AddTorque(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f));


		Destroy(this);       
        Destroy(agent);
    }
}
