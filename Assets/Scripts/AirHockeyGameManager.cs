using UnityEngine;
using TMPro;

public class AirHockeyGameManager : MonoBehaviour
{
    public static AirHockeyGameManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;

    public GameObject puck;

    private int leftScore = 0;
    private int rightScore = 0;
    public int winningScore = 7;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;
        UpdateScoreText();

        if (winText != null)
            winText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ResetMatch();
        }
    }

    public void RegisterPuck(GameObject newPuck)
    {
        puck = newPuck;
    }

    public void ScoreLeft()
    {
        leftScore++;
        UpdateScoreText();
        CheckWinner();
    }

    public void ScoreRight()
    {
        rightScore++;
        UpdateScoreText();
        CheckWinner();
    }

    void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = leftScore + " - " + rightScore;
    }

    void CheckWinner()
    {
        if (winText == null) return;

        if (leftScore >= winningScore)
        {
            winText.text = "Left Player Wins!";
        }
        else if (rightScore >= winningScore)
        {
            winText.text = "Right Player Wins!";
        }
    }

    void ResetMatch()
    {
        Time.timeScale = 1f;

        leftScore = 0;
        rightScore = 0;
        UpdateScoreText();

        if (winText != null)
            winText.text = "";

        if (puck != null)
        {
            puck.transform.position = new Vector3(0f, 0.35f, 0f);

            Rigidbody rb = puck.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
                rb.AddForce(new Vector3(5f, 0f, 2f), ForceMode.Impulse);
            }
        }
    }
}