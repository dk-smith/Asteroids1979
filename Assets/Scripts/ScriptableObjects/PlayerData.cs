using System;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class PlayerData : ScriptableObject
    {
        public float aSpeed;
        public float rSpeed;
        public float hyperDelay;
        public Exploding exploding;
        public AudioClip[] clips;
        public GameObject bullet;
        
        [NonSerialized] private int lives;
        [NonSerialized] private int score;
        
        public int Lives
        {
            get => lives;
            set
            {
                if (value != lives)
                {
                    if (value < lives)
                        OnHit?.Invoke();
                    
                    lives = value;
                    OnChange?.Invoke();
                }
            }
        }

        public int Score
        {
            get => score;
            set
            {
                if (value != score)
                {
                    score = value;
                    OnChange?.Invoke();
                }
            }
        }

        public event Action OnChange;
        public event Action OnHit;
    }
}