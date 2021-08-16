using System.Collections;
using ScriptableObjects;
using UnityEngine;

// Контроллер объектов типа "Летающая тарелка".
namespace Controllers
{
    public class SaucerController : MonoBehaviour
    {
        [SerializeField] private SaucerData saucerData;
        [SerializeField] private Transform shootPoint;

        private AudioSource audioSource;
        private Bounds bounds;
        private Rigidbody2D rb;
    
        protected virtual Vector3 ShootDir => Random.insideUnitCircle.normalized;
        protected virtual bool CanShoot => true;

        protected virtual void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            audioSource = GetComponent<AudioSource>();
            bounds = GetComponent<Collider2D>().bounds;
        
            rb.AddForce(Random.insideUnitCircle.normalized * saucerData.speed, ForceMode2D.Impulse);

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
            saucerData.OnDie();
            saucerData.exploding.Play(gameObject);
            Destroy(gameObject);
        }

        void Shoot()
        {
            var shootDir = ShootDir; 

            Vector3 position = transform.position + shootDir * (Mathf.Max(bounds.extents.x, bounds.extents.y) + .05f);
            shootPoint.position = position;
            Quaternion rotation = Quaternion.LookRotation(Vector3.forward, shootDir);

            Instantiate(saucerData.bullet, shootPoint.position, rotation).tag = "EnemyBullet";

            audioSource.clip = saucerData.shootClip;
            audioSource.volume = .8f;
            audioSource.Play();
        }

    
        IEnumerator ShootCoroutine()
        {
            yield return new WaitForSeconds(1f);
            while (true)
            {
                if (CanShoot) Shoot();
                yield return new WaitForSeconds(saucerData.shootRate);
            }
        }

        // Поворачивает вектор движения тарелки на угол changeDirAngle в случайном направлении
        // спустя случайное время из changeDirTimeRange.
        IEnumerator ChangeDirection()
        {
            while (true)
            {
                ushort time = (ushort)Random.Range(saucerData.changeDirTimeRange.x, saucerData.changeDirTimeRange.y);
                yield return new WaitForSeconds(time);
                rb.velocity = Quaternion.Euler(0f, 0f, saucerData.changeDirAngle * (Random.Range(0, 100) % 2 == 0 ? 1 : -1)) * rb.velocity;
            }
        }
    }
}
