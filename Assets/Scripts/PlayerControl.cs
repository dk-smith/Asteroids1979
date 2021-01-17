using System.Collections;
using UnityEngine;
using TMPro;

// Контроллер объекта игрока.
public class PlayerControl : MonoBehaviour
{

    [SerializeField] private float aSpeed;
    [SerializeField] private float rSpeed;
    [SerializeField] private int startLives;
    [SerializeField] private float hyperDelay;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject livesText;
    [SerializeField] private AudioClip[] clips;

    private AudioSource audioSource;
    private Rigidbody2D rb;
    private bool isActive;
    private int lives;
    private float vertInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        Reset();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        vertInput = Mathf.Clamp(Input.GetAxis("Vertical"), 0, 1);

        transform.Rotate(Vector3.forward, -1 * (h * rSpeed) * Time.deltaTime, Space.Self);

        if (Input.GetKeyDown(KeyCode.Space)) Shoot();
        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) StartCoroutine(Hyperspace());
    }

    void FixedUpdate()
    {
        rb.AddForce(transform.up * vertInput * aSpeed);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("EnemyBullet") || collision.tag.Equals("Asteroid") || collision.tag.Equals("Saucer"))
        {
            StartCoroutine(GetHit());
        }
    }

    void Shoot()
    {
        PlayClip(clips[0], .6f);
        GameObject newBullet = Instantiate(bullet, shootPoint.position, transform.rotation);
        newBullet.tag = "PlayerBullet";
    }

    IEnumerator Hyperspace()
    {
        Vector2 newPos = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
        newPos = Camera.main.ScreenToWorldPoint(newPos);
        SetActive(false);
        PlayClip(clips[1], .6f);
        yield return new WaitForSeconds(hyperDelay);
        SetActive(true);
        transform.position = newPos;
    }

    void PlayClip(AudioClip clip, float volume)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();
    }

    IEnumerator GetHit()
    {
        lives--;
        UpdateLivesText();

        GetComponent<Exploding>().Play();

        SetActive(false);
        yield return new WaitForSeconds(1f);
        if (lives == 0)
        {
            Camera.main.GetComponent<GameManager>().GameOver();
        } else
        {
            ResetPosition();
            SetActive(true);
        }
    }

    void SetActive(bool active)
    {
        GetComponentInChildren<Renderer>().enabled = active;
        GetComponent<Collider2D>().enabled = active;
        enabled = active;
        isActive = active;
    }

    public bool IsActive() { return isActive; }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        rb.velocity = Vector3.zero;
    }

    public void Reset()
    {
        SetActive(true);
        ResetPosition();
        lives = startLives;
        UpdateLivesText();
    }

    public int GetLives() { return lives; }

    void UpdateLivesText()
    {
        livesText.GetComponent<TextMeshProUGUI>().SetText(new string('A', lives));
    }
}
