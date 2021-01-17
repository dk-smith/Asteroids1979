using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Контроллер объектов типа "Астероид".
public class AsteroidController : MonoBehaviour
{

    [SerializeField] private GameObject child; // Объект, создаваемый при разрушении
    [SerializeField] private int childCount;
    [SerializeField] private float speed;
    [SerializeField] private int score;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private bool isBig;
    private LinkedListNode<GameObject> node = null; // Ссылка на узел связного списка астероидов
    
    void Awake()
    {
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Length)];

        transform.Rotate(transform.forward, Random.Range(0f, 360f));
        GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle.normalized * speed, ForceMode2D.Impulse);

        node = Camera.main.GetComponent<GameManager>().AddAsteroid(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Contains("Bullet"))
        {
            if (collision.tag.Equals("PlayerBullet")) Camera.main.GetComponent<GameManager>().AddScore(score);
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        if (child != null)
        {
            for (int i = 0; i < childCount; i++)
            {
                Instantiate(child, transform.position, Quaternion.identity);
            }
        }

        Camera.main.GetComponent<GameManager>().RemoveAsteroid(node);
        GetComponent<Exploding>().Play();
        Destroy(gameObject);
    }

    public bool IsBig() { return isBig; }


}
