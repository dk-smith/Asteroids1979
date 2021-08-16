using TMPro;
using UnityEngine;

// Графический интерфейс меню.
namespace UI
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI stateText;
        [SerializeField] private TextMeshProUGUI promptText;
        [SerializeField] private TextMeshProUGUI addText;
        [SerializeField] private TextMeshProUGUI highScoreText;

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
            stateText.SetText(state);
            promptText.SetText(prompt);
            addText.SetText(add);
        }
    
        public void UpdateHighScoreText(int highScore)
        {
            highScoreText.GetComponent<TextMeshProUGUI>().SetText("High Score: " + highScore.ToString());
        }
    }
}
