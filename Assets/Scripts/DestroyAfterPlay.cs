using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Уничтожение системы частиц после окончания проигрывания.
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
