using UnityEngine;
using UnityEngine.UIElements;

public class AirHockeyUIController : MonoBehaviour
{
    private UIDocument uiDocument;

    private VisualElement mainMenu;
    private VisualElement hud;

    private Label scoreLabel;
    private Label roleLabel;
    private Label winnerLabel;

    public static string selectedRole = "Striker";

    void Awake()
    {
        uiDocument = GetComponent<UIDocument>();

        VisualElement root = uiDocument.rootVisualElement;

        mainMenu = root.Q<VisualElement>("MainMenu");
        hud = root.Q<VisualElement>("HUD");

        scoreLabel = root.Q<Label>("ScoreLabel");
        roleLabel = root.Q<Label>("RoleLabel");
        winnerLabel = root.Q<Label>("WinnerLabel");

        Button strikerButton = root.Q<Button>("PlayStrikerButton");
        Button defenderButton = root.Q<Button>("PlayDefenderButton");
        Button verticalButton = root.Q<Button>("VerticalButton");
        Button horizontalButton = root.Q<Button>("HorizontalButton");
        Button restartButton = root.Q<Button>("RestartButton");

        strikerButton.clicked += () =>
        {
            selectedRole = "Striker";
            StartGame();
        };

        defenderButton.clicked += () =>
        {
            selectedRole = "Defender";
            StartGame();
        };

        verticalButton.clicked += () =>
        {
            Screen.orientation = ScreenOrientation.Portrait;
        };

        horizontalButton.clicked += () =>
        {
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        };

        restartButton.clicked += () =>
        {
            if (AirHockeyGameManager.Instance != null)
            {
                AirHockeyGameManager.Instance.ResetPuck(Random.value > 0.5f ? 1 : -1);
            }
        };

        hud.style.display = DisplayStyle.None;
    }

    void Update()
    {
        if (AirHockeyGameManager.Instance == null) return;

        scoreLabel.text = "Left Team: " + AirHockeyGameManager.Instance.leftScore +
                          " | Right Team: " + AirHockeyGameManager.Instance.rightScore;

        roleLabel.text = "Current Role: " + selectedRole;

        if (AirHockeyGameManager.Instance.leftScore >= 7)
        {
            winnerLabel.text = "LEFT TEAM WINS!";
        }
        else if (AirHockeyGameManager.Instance.rightScore >= 7)
        {
            winnerLabel.text = "RIGHT TEAM WINS!";
        }
        else
        {
            winnerLabel.text = "";
        }
    }

    void StartGame()
    {
        mainMenu.style.display = DisplayStyle.None;
        hud.style.display = DisplayStyle.Flex;
    }
}