using System.Collections;
using ScriptableObjects;
using TMPro;
using UnityEngine;

namespace UI
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI livesText;
        [SerializeField] private TextMeshProUGUI newHighScoreText;

        [SerializeField] private PlayerData playerData;

        private void Awake()
        {
            if (playerData)
            {
                playerData.OnChange += OnPlayerDataChange;
                OnPlayerDataChange();
            }
        }

        private void OnPlayerDataChange()
        {
            scoreText.text = playerData.Score.ToString();
            livesText.text = new string('A', playerData.Lives);
        }

        public void OnNewHighScore() => StartCoroutine(NewHighScore());

        IEnumerator NewHighScore()
        {
            newHighScoreText.gameObject.SetActive(true);
            yield return new WaitForSeconds(3f);
            newHighScoreText.gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            if (playerData)
            {
                playerData.OnChange -= OnPlayerDataChange;
            }
        }
    }
}
