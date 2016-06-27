using UnityEngine;
using System.Collections;

public class AITemplate : MonoBehaviour {

    public enum AIState {
        Patrol, Alert, Search
    }

    #region Inspector Variables
    public Transform[] patrol;
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
    float lineOfSight;
    Vector3 targetLastKnown;
    Vector3 randomLocation;
    bool searchExpire;
    float currentR;
    float targetR;
    AITemplate instance;
    int lineOfSightTime;
    #endregion

    void Start() {
        currentPatrolInt = 0;
        delayOver = true;

        currentState = AIState.Patrol;
    }

    void Update() {
        switch (currentState) {
            case AIState.Patrol:
                if (patrol.Length > 0) {
                    if (transform.position == patrol[currentPatrolInt].position) {
                        if (accending) {
                            if (currentPatrolInt < patrol.Length - 1)
                                currentPatrolInt++;
                            else
                                accending = false;
                        } else {
                            if (currentPatrolInt > 0)
                                currentPatrolInt--;
                            else
                                accending = true;
                        }
                        StartCoroutine(Delay(delayTime));
                    } else {
                        if (delayOver) {
                            transform.position = Vector3.MoveTowards(transform.position, patrol[currentPatrolInt].position, speed);
                            transform.LookAt(patrol[currentPatrolInt]);
                        } else {
                            LookAround();
                        }
                    }
                } else {
                    LookAround();
                }

                if (target != null) {
                    currentState = AIState.Alert;
                }
                break;

            case AIState.Alert:
                if (target != null) {
                    if ((target.position - transform.position).magnitude < range) {
                        Shooting(1);
                    } else {
                        transform.position = Vector3.MoveTowards(transform.position, target.position, speed);
                    }
                    head.transform.LookAt(target);
                } else {
                    StartCoroutine(Delay(5));
                    randomLocation = transform.position;
                    currentState = AIState.Search;
                }
                break;

            case AIState.Search:
                if (!delayOver) {
                    RadiusPatrol(targetLastKnown, 10);
                    LookAround();
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
            if (hit.transform == player) {
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
        if (transform.position == randomLocation)
            randomLocation = new Vector3(radiusCentre.x + Random.Range(-radius, radius), radiusCentre.y, radiusCentre.z + Random.Range(-radius, radius));
        else
            transform.position = Vector3.MoveTowards(transform.position, randomLocation, speed);
    }

    public void Dead() {
        Destroy(this, 1);
    }
}
