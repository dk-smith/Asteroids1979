using System.Collections;
using ScriptableObjects;
using UnityEngine;

// Контроллер объекта игрока.
namespace Controllers
{
    public class PlayerControl : MonoBehaviour
    {
        [SerializeField] private Transform shootPoint;
        [SerializeField] private PlayerData playerData;

        private AudioSource audioSource;
        private Rigidbody2D rb;
        private bool isActive;
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

            transform.Rotate(Vector3.forward, -1 * (h * playerData.rSpeed) * Time.deltaTime, Space.Self);

            if (Input.GetKeyDown(KeyCode.Space)) Shoot();
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift)) StartCoroutine(Hyperspace());
        }

        void FixedUpdate()
        {
            rb.AddForce(transform.up * (vertInput * playerData.aSpeed));
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.tag.Equals("PlayerBullet"))
            {
                GetHit();
            }
        }

        void Shoot()
        {
            PlayClip(playerData.clips[0], .6f);
            Instantiate(playerData.bullet, shootPoint.position, transform.rotation).tag = "PlayerBullet";
        }

        IEnumerator Hyperspace()
        {
            Vector2 newPos = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
            newPos = Camera.main.ScreenToWorldPoint(newPos);
            SetActive(false);
            PlayClip(playerData.clips[1], .6f);
            yield return new WaitForSeconds(playerData.hyperDelay);
            SetActive(true);
            transform.position = newPos;
        }

        void PlayClip(AudioClip clip, float volume)
        {
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
        }

        void GetHit()
        {
            playerData.Lives--;
            playerData.exploding.Play(gameObject);
            SetActive(false);
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
        }
    }
}
