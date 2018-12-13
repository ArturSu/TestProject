using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using Random = UnityEngine.Random;

public class RollerAgent : Agent
{
    Rigidbody rBody;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        StartCoroutine(DecisionsCoroutine());
    }

    private IEnumerator DecisionsCoroutine()
    {
        while (true)
        {
            RequestDecision();
            yield return new WaitForSeconds(1f);
        }
    }

    public Transform Target;

    public override void AgentReset()
    {
        if (Mathf.Abs(transform.localPosition.x) > 5 || Mathf.Abs(transform.localPosition.z) > 5)
        {
            // The Agent fell
            this.transform.localPosition = Vector3.zero;
//            this.rBody.angularVelocity = Vector3.zero;
//            this.rBody.velocity = Vector3.zero;
        }
        else
        {
            // Move the target to a new spot
            Target.localPosition = new Vector3(Random.value * 8 - 4,
                0.5f,
                Random.value * 8 - 4);
        }
    }

    public override void CollectObservations()
    {
        // Calculate relative position
        Vector3 relativePosition = Target.position - this.transform.position;

        // Relative position
        AddVectorObs(relativePosition.x / 5);
        AddVectorObs(relativePosition.z / 5);

        // Distance to edges of platform
        AddVectorObs((this.transform.localPosition.x + 5) / 5);
        AddVectorObs((this.transform.localPosition.x - 5) / 5);
        AddVectorObs((this.transform.localPosition.z + 5) / 5);
        AddVectorObs((this.transform.localPosition.z - 5) / 5);

        // Agent velocity
        AddVectorObs(rBody.velocity.x / 5);
        AddVectorObs(rBody.velocity.z / 5);
    }

    public float speed = 10;
    private float previousDistance = float.MaxValue;

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        int action = Mathf.FloorToInt(vectorAction[0]);

        Vector3 dirToGo = Vector3.zero;

        // Goalies and Strikers have slightly different action spaces.
        switch (action)
        {
            case 1:
                dirToGo = Vector3.forward;
                break;
            case 2:
                dirToGo = Vector3.forward * -1f;
                break;
            case 3:
                dirToGo = Vector3.left;
                break;
            case 4:
                dirToGo = Vector3.left * -1f;
                break;
        }

//        // Actions, size = 2
//        Vector3 controlSignal = Vector3.zero;
//        controlSignal.x = vectorAction[0];
//        controlSignal.z = vectorAction[1];
//        rBody.AddForce(controlSignal * speed);


        transform.localPosition += dirToGo;// * 0.1f;

        // Rewards
        float distanceToTarget = Vector3.Distance(this.transform.position,
            Target.position);

        // Reached target
        if (distanceToTarget < 1.42f)
        {
            AddReward(1.0f);
            Done();
        }

        if (distanceToTarget < previousDistance)
        {
            AddReward(0.1f);
        }
        else
        {
            AddReward(-0.1f);
        }

        previousDistance = distanceToTarget;

        // Time penalty
        AddReward(-0.05f);

        // Fell off platform
        if (Mathf.Abs(transform.localPosition.x) > 5 || Mathf.Abs(transform.localPosition.z) > 5)
        {
            AddReward(-1f);
            Done();
        }
    }
}