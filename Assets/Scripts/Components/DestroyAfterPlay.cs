using System.Collections;
using UnityEngine;

// Уничтожение системы частиц после окончания проигрывания.
namespace Components
{
    public class DestroyAfterPlay : MonoBehaviour
    {
        private ParticleSystem particle;

        void Start()
        {
            particle = GetComponent<ParticleSystem>();
            StartCoroutine(Particle());
        }

        IEnumerator Particle()
        {
            particle.Play();
            yield return new WaitWhile(() => particle.isPlaying);
            Destroy(gameObject);
        }
    }
}
