uusing UnityEngine;
using UnityEngine.UI;

public class ScoreDisplayUI : MonoBehaviour
{
    private Text scoreText;
    private Text winnerText;

    void Start()
    {
        Canvas canvas = new GameObject("Score Canvas").AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = canvas.gameObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        canvas.gameObject.AddComponent<GraphicRaycaster>();

        scoreText = CreateText(canvas.transform, "ScoreText", new Vector2(0, 450), 40);
        winnerText = CreateText(canvas.transform, "WinnerText", new Vector2(0, 380), 50);
    }

    void Update()
    {
        if (AirHockeyGameManager.Instance == null) return;

        scoreText.text = "Left Team: " + AirHockeyGameManager.Instance.leftScore +
                         "   |   Right Team: " + AirHockeyGameManager.Instance.rightScore;

        if (AirHockeyGameManager.Instance.leftScore >= 7)
        {
            winnerText.text = "LEFT TEAM WINS!";
        }
        else if (AirHockeyGameManager.Instance.rightScore >= 7)
        {
            winnerText.text = "RIGHT TEAM WINS!";
        }
        else
        {
            winnerText.text = "";
        }
    }

    Text CreateText(Transform parent, string name, Vector2 position, int fontSize)
    {
        GameObject obj = new GameObject(name);
        obj.transform.SetParent(parent);

        Text text = obj.AddComponent<Text>();
        text.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        text.fontSize = fontSize;
        text.alignment = TextAnchor.MiddleCenter;
        text.color = Color.white;

        RectTransform rect = obj.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(900, 100);
        rect.anchoredPosition = position;

        return text;
    }
}