using TMPro;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AirHockeyAgent : Agent
{
    private Vector3 startPosition;
    public Transform puck;
    public Rigidbody puckRb;

    public float moveSpeed = 10f;

    public float minX = 3f;
    public float maxX = 6f;
    public float minZ = -2.7f;
    public float maxZ = 2.7f;

    private Rigidbody rb;

    public override void Initialize()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        RequestDecision();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(puck.localPosition);
        sensor.AddObservation(puckRb.velocity);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();

        float moveX = actions.ContinuousActions[0];
        float moveZ = actions.ContinuousActions[1];

        Vector3 newPos = rb.position + new Vector3(moveX, 0f, moveZ) * moveSpeed * Time.fixedDeltaTime;

        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);
        newPos.z = Mathf.Clamp(newPos.z, minZ, maxZ);

        rb.MovePosition(newPos);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<float> actions = actionsOut.ContinuousActions;

        actions[0] = 0f;
        actions[1] = 0f;

        if (Input.GetKey(KeyCode.LeftArrow)) actions[0] = -1f;
        if (Input.GetKey(KeyCode.RightArrow)) actions[0] = 1f;
        if (Input.GetKey(KeyCode.UpArrow)) actions[1] = 1f;
        if (Input.GetKey(KeyCode.DownArrow)) actions[1] = -1f;
    }
    void LateUpdate()
    {
        Vector3 pos = transform.position;

        pos.y = startPosition.y;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minZ, maxZ);

        transform.position = pos;
    }
}