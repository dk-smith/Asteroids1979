using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Общий контроль игрового процесса. Применяется к главной камере.
public class GameManager : MonoBehaviour
{
    [SerializeField] int[] levels; // Массив, содержащий количество больших астероидов, создаваемых в начале уровня
    [SerializeField] GameObject asteroid;
    [SerializeField] GameObject[] saucers;
    [SerializeField] Transform player;
    [SerializeField] Vector2 saucerRespTimeRange;
    [SerializeField][Range(0, 1)] float saucerRespCondition;
    [SerializeField] private GameObject scoreText;
    [SerializeField] private GameObject pauseCanvas;

    private LinkedList<GameObject> asteroidsList = new LinkedList<GameObject>();
    private int level = 0;
    private int score = 0;
    private bool gameOver = false;
    private int bigCount = 0;

    private void Awake()
    {
        FillList();
        StartCoroutine(WaitForRestart());
        StartCoroutine(AddSaucerCoroutine());
    }

    void Update()
    {
        if (!gameOver && Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    void Pause()
    {
        Time.timeScale = Time.timeScale == 1 ? 0 : 1;
        pauseCanvas.GetComponent<PauseCanvasController>().PauseState();
        pauseCanvas.SetActive(!pauseCanvas.activeSelf);
        PlayerControl pc = player.GetComponent<PlayerControl>();
        pc.enabled = !pc.enabled;
    }

    void FillList()
    {
        for (int i = 0; i < levels[level]; i++)
            Instantiate(asteroid, GetRandomPosition(), Quaternion.identity);
        bigCount += levels[level];
    }

    // Возвращает случайные коордианты игрового мира на границе экрана.
    Vector2 GetRandomPosition()
    {
        byte[] side = new byte[4];
        side[Random.Range(0, 3)] = 1;
        int x = Mathf.Max(Mathf.Max(0, Screen.width * side[2]), Random.Range(0, Screen.width) * Mathf.Max(side[1], side[3]));//Mathf.Max(0, Screen.width * side[2])
        int y = Mathf.Max(Mathf.Max(0, Screen.height * side[3]), Random.Range(0, Screen.height) * Mathf.Max(side[0], side[2]));//Mathf.Max(0, Screen.width * side[2])
        Vector2 screenPoint = new Vector2(x, y);
        return Camera.main.ScreenToWorldPoint(screenPoint);
    }

    public void RemoveAsteroid(LinkedListNode<GameObject> node)
    {
        if (node.Value.GetComponent<AsteroidController>().IsBig()) bigCount--;
        asteroidsList.Remove(node);
        if (!gameOver)
        {
            CheckForEmpty();
        }
    }

    void CheckForEmpty()
    {
        if (asteroidsList.Count == 0)
        {
            StopCoroutine(AddSaucerCoroutine());
            level = Mathf.Min(levels.Length - 1, level + 1);
            FillList();
        }
    }

    public LinkedListNode<GameObject> AddAsteroid(GameObject obj)
    {
        return asteroidsList.AddLast(obj);
    }

    void AddSaucer()
    {
        int index = Random.Range(0, 100) % 2 == 0 ? 1 : 0;
        Instantiate(saucers[index], GetRandomPosition(), Quaternion.identity);
    }

    IEnumerator AddSaucerCoroutine()
    {
        while (true)
        {
            yield return new WaitUntil(() => bigCount < levels[level] * saucerRespCondition);
            do
            {
                AddSaucer();
                yield return new WaitForSeconds(Random.Range(saucerRespTimeRange.x, saucerRespTimeRange.y));
            } while ((bigCount < levels[level] * saucerRespCondition) && !gameOver);
        }
    }

    public void GameOver()
    {
        gameOver = true;
        pauseCanvas.GetComponent<PauseCanvasController>().GameOverState(score);
        pauseCanvas.SetActive(true);
        StartCoroutine(WaitForRestart());
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.GetComponent<TextMeshProUGUI>().SetText(this.score.ToString());
    }

    IEnumerator WaitForRestart()
    {
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));
        ResetGame();
    }

    void ResetGame()
    {
        var node = asteroidsList.First;
        if (node != null) do
        {
            Destroy(node.Value);
            node = node.Next;
            asteroidsList.RemoveFirst();
        } while (node != null);

        bigCount = 0;

        GameObject[][] objs = new GameObject[][]{
            GameObject.FindGameObjectsWithTag("Saucer"), GameObject.FindGameObjectsWithTag("Bullet"),
            GameObject.FindGameObjectsWithTag("PlayerBullet"), GameObject.FindGameObjectsWithTag("EnemyBullet"),
            GameObject.FindGameObjectsWithTag("Particle")};
        for (int i = 0; i < objs.Length; i++)
            foreach (var obj in objs[i])
            {
                Destroy(obj);
            }

        player.gameObject.SetActive(true);
        player.GetComponent<PlayerControl>().Reset();

        gameOver = false;
        score = 0;
        level = 0;
        pauseCanvas.SetActive(false);
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        FillList();
        AddScore(0);
    }
}
