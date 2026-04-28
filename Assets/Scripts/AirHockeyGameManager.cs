using UnityEngine;

public class AirHockeyGameManager : MonoBehaviour
{
    public static AirHockeyGameManager Instance;

    public int leftScore = 0;
    public int rightScore = 0;

    public float puckMinSpeed = 8f;
    public float puckMaxSpeed = 20f;

    private GameObject puck;
    private Rigidbody puckRb;

    void Awake()
    {
        Instance = this;
    }

    public void RegisterPuck(GameObject puckObject)
    {
        puck = puckObject;
        puckRb = puck.GetComponent<Rigidbody>();
    }

    public void ScoreGoal(bool leftGoal)
    {
        if (leftGoal)
        {
            rightScore++;
            Debug.Log("Right Team Scores! Score: Left " + leftScore + " - Right " + rightScore);
            ResetPuck(-1);
        }
        else
        {
            leftScore++;
            Debug.Log("Left Team Scores! Score: Left " + leftScore + " - Right " + rightScore);
            ResetPuck(1);
        }

        if (leftScore >= 7 || rightScore >= 7)
        {
            Debug.Log("Match finished. Resetting score.");
            leftScore = 0;
            rightScore = 0;
        }
    }

    public void ResetPuck(int serveDirection)
    {
        if (puck == null || puckRb == null)
        {
            return;
        }

        puck.transform.position = new Vector3(0f, 0.35f, 0f);
        puckRb.linearVelocity = Vector3.zero;
        puckRb.angularVelocity = Vector3.zero;

        ServePuck(serveDirection);
    }

    public void ServePuck(int direction)
    {
        if (puckRb == null)
        {
            return;
        }

        float zDirection = Random.Range(-0.45f, 0.45f);
        Vector3 forceDirection = new Vector3(direction, 0f, zDirection).normalized;

        puckRb.AddForce(forceDirection * 15f, ForceMode.Impulse);
    }

    void FixedUpdate()
    {
        if (puckRb == null)
        {
            return;
        }

        Vector3 velocity = puckRb.linearVelocity;
        velocity.y = 0f;

        if (velocity.magnitude < 0.1f)
        {
            return;
        }

        if (velocity.magnitude < puckMinSpeed)
        {
            puckRb.linearVelocity = velocity.normalized * puckMinSpeed;
        }
        else if (velocity.magnitude > puckMaxSpeed)
        {
            puckRb.linearVelocity = velocity.normalized * puckMaxSpeed;
        }
    }
}
