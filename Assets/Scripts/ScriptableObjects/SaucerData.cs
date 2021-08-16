using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class SaucerData : EnemyData
    {
        public float shootRate;
        public float changeDirAngle;
        public Vector2 changeDirTimeRange;
        public GameObject bullet;
        public AudioClip shootClip;
    }
}