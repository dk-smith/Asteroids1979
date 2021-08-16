using System;
using Controllers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class GameSettings : ScriptableObject
    {
        public int startLives;
        public int[] levels; // Массив, содержащий количество больших астероидов, создаваемых в начале уровня
        public AsteroidController asteroid;
        public SaucerController[] saucers;
        public Vector2 saucerRespTimeRange;
        [SerializeField][Range(0, 1)] public float saucerRespCondition;

        [NonSerialized] public bool WasInit = false;
    }
}