using UnityEngine;

// Конртоллер объектов типа "Пуля".
namespace Controllers
{
    public class BulletControl : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float lifeTime;

        void Start()
        {
            Destroy(gameObject, lifeTime);
            GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * speed, ForceMode2D.Impulse);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(gameObject);
        }

    }
}
