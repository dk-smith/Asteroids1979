using UnityEngine;

namespace ScriptableObjects
{
    public abstract class EnemyData : ScriptableObject
    {
        public float speed;
        public int score;
        public Exploding exploding;
        public PlayerData playerData;

        public void OnDie()
        {
            playerData.Score += score;
        }
    }
}