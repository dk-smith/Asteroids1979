using System;
using ScriptableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

// Контроллер объектов типа "Астероид".
namespace Controllers
{
    public class AsteroidController : MonoBehaviour
    {
        public static event Action<AsteroidController> OnAsteroidDestroy;
    
        [SerializeField] private AsteroidData asteroidData;

        public bool IsBig => asteroidData.isBig;
        public AsteroidData Data => asteroidData;
    
        void Awake()
        {
            GetComponent<SpriteRenderer>().sprite = asteroidData.sprites[Random.Range(0, asteroidData.sprites.Length)];

            transform.Rotate(transform.forward, Random.Range(0f, 360f));
            GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * asteroidData.speed, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag.Contains("Bullet"))
            {
                if (collision.tag.Equals("PlayerBullet")) asteroidData.OnDie();
                DestroySelf();
            }
        }

        void DestroySelf()
        {
            asteroidData.exploding.Play(gameObject);
            OnAsteroidDestroy?.Invoke(this);
        
            Destroy(gameObject);
        }

    }
}
