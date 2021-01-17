using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Контроллер объектов типа "Летающая тарелка".
public class SaucerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float shootRate;
    [SerializeField] private int score;
    [SerializeField] private bool aim;
    [SerializeField] private float changeDirAngle;
    [SerializeField] private Vector2 changeDirTimeRange;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform target;
    [SerializeField] private GameObject bullet;
    [SerializeField] private AudioClip shootClip;

    private AudioSource audioSource;
    private Bounds bounds;

    void Start()
    {
        try { if (aim) target = GameObject.FindGameObjectWithTag("Player").transform; }
        catch (System.Exception e) { Debug.LogError(e.Message); }

        GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * speed, ForceMode2D.Impulse);

        audioSource = GetComponent<AudioSource>();
        bounds = GetComponent<Collider2D>().bounds;

        StartCoroutine(ShootCoroutine());
        StartCoroutine(ChangeDirection());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("PlayerBullet"))
        {
            Die();
        }
    }

    void Die()
    {
        Camera.main.GetComponent<GameManager>().AddScore(score);
        GetComponent<Exploding>().Play();
        Destroy(gameObject);
    }

    void Shoot()
    {
        Vector3 shootDir;
        if (target != null)
            shootDir = (target.position - transform.position).normalized;
        else
            shootDir = Random.insideUnitCircle.normalized;

        Vector3 position = transform.position + shootDir * (Mathf.Max(bounds.extents.x, bounds.extents.y) + .05f);
        shootPoint.position = position;
        Quaternion rotation = Quaternion.LookRotation(Vector3.forward, target == null ? shootDir : (target.position - shootPoint.position));

        GameObject newBullet = Instantiate(bullet, shootPoint.position, rotation);
        newBullet.tag = "EnemyBullet";

        audioSource.clip = shootClip;
        audioSource.volume = .8f;
        audioSource.Play();
    }

    IEnumerator ShootCoroutine()
    {
        yield return new WaitForSeconds(1f);
        while (true)
        {
            if (!aim || (target != null && target.GetComponent<PlayerControl>().IsActive())) Shoot();
            yield return new WaitForSeconds(shootRate);
        }
    }

    // Поворачивает вектор движения тарелки на угол changeDirAngle в случайном направлении
    // спустя случайное время из changeDirTimeRange.
    IEnumerator ChangeDirection()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        while (true)
        {
            ushort time = (ushort)Random.Range(changeDirTimeRange.x, changeDirTimeRange.y);
            yield return new WaitForSeconds(time);
            rb.velocity = Quaternion.Euler(0f, 0f, changeDirAngle * (Random.Range(0, 100) % 2 == 0 ? 1 : -1)) * rb.velocity;
        }
    }
}
