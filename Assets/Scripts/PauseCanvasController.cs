using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Графический интерфейс меню.
public class PauseCanvasController : MonoBehaviour
{

    [SerializeField] private GameObject stateText;
    [SerializeField] private GameObject promptText;
    [SerializeField] private GameObject addText;

    private void Awake()
    {
        StartState();
    }

    public void StartState()
    {
        ChangeState("Asteroids 1979", "Press ENTER to start", "");
    }

    public void PauseState()
    {
        ChangeState("Pause", "Press ESC to continue", "");
    }

    public void GameOverState(int score)
    {
        ChangeState("Game Over", "Press ENTER to restart", "Score: "+score.ToString());
    }

    void ChangeState(string state, string prompt, string add)
    {
        stateText.GetComponent<TextMeshProUGUI>().SetText(state);
        promptText.GetComponent<TextMeshProUGUI>().SetText(prompt);
        addText.GetComponent<TextMeshProUGUI>().SetText(add);
    }
}
