using UnityEngine;

// Создание и проигрывание эффектов уничтожения (системы частиц и звука).
namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class Exploding : ScriptableObject
    {
        [SerializeField] private ParticleSystem particle;
        [SerializeField] private AudioClip clip;

        public void Play(GameObject obj)
        {
            var particleObject = Instantiate(particle, obj.transform.position, Quaternion.identity);
            AudioSource audio = particleObject.gameObject.AddComponent<AudioSource>();;
            audio.clip = clip;
            audio.Play();
        }

    }
}
