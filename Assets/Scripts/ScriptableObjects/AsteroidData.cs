using Controllers;
using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class AsteroidData : EnemyData
    {
        public AsteroidController child; // Объект, создаваемый при разрушении
        public int childCount;
        public Sprite[] sprites;
        public bool isBig;
    }
}