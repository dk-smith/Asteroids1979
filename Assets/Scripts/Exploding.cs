using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Создание и проигрывание эффектов уничтожения (системы частиц и звука).
public class Exploding : MonoBehaviour
{
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private AudioClip clip;

    public void Play()
    {
        Instantiate(particle, transform.position, Quaternion.identity);
        GameObject explosion = new GameObject("Explosion", typeof(AudioSource));
        AudioSource audio = explosion.GetComponent<AudioSource>();
        audio.clip = clip;
        Destroy(explosion, audio.clip.length);
        audio.Play();
    }

}
