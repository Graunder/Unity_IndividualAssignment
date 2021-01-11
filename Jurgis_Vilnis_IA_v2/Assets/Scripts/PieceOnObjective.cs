using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceOnObjective : MonoBehaviour
{

    public bool objectiveTaken = false;

    private void Update() {
        if (!objectiveTaken) {
            StandingOnObjective();
        }
    }

    public void StandingOnObjective() {
        RaycastHit hit;
        if (Physics.Raycast(this.transform.position, Vector3.up, out hit, 1)) {
            Debug.Log("Piece on top of Objective");
            objectiveTaken = true;
            Score_Counter.score++;
        }
    }

}
