using UnityEngine;

namespace Controllers
{
    public class SmallSaucerController : SaucerController
    {
        [SerializeField] private Transform target;
        
        protected override Vector3 ShootDir => (target.position - transform.position).normalized;
        protected override bool CanShoot => target != null && target.GetComponent<PlayerControl>().IsActive();

        protected override void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            base.Start();
        }

    }
}