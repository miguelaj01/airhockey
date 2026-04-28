using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    public bool isLeftGoal;
    public float restartSpeed = 6f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Puck"))
        {
            AirHockeyAgent[] agents = FindObjectsOfType<AirHockeyAgent>();

            if (isLeftGoal)
            {
                AirHockeyGameManager.Instance.ScoreRight();

                foreach (AirHockeyAgent agent in agents)
                    agent.AddReward(1.0f);
            }
            else
            {
                AirHockeyGameManager.Instance.ScoreLeft();

                foreach (AirHockeyAgent agent in agents)
                    agent.AddReward(-1.0f);
            }

            other.transform.position = new Vector3(0f, 0.35f, 0f);

            Rigidbody rb = other.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                float xDirection = isLeftGoal ? -1f : 1f;
                Vector3 restartDirection = new Vector3(xDirection, 0f, Random.Range(-0.5f, 0.5f)).normalized;

                rb.AddForce(restartDirection * restartSpeed, ForceMode.Impulse);
            }
        }
    }
}