using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerTrigger : MonoBehaviour {

    Collider[] triggered;
    List<AITemplate> instance;

    int prevCount;

    void Update() {
        triggered = Physics.OverlapBox(transform.position, new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, 2);
        if (triggered.Length > 0) {
            
            if (triggered.Length == prevCount) {
                foreach (AITemplate ai in instance) {
                    ai.Alert(transform);
                }
            } else {
                instance = new List<AITemplate>();
                foreach (Collider ai in triggered) {
                    instance.Add(ai.transform.root.GetComponent<AITemplate>());
                }
            }
        }
        prevCount = triggered.Length;
    }
}
