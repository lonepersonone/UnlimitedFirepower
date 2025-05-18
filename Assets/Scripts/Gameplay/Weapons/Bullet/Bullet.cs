using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace MyGame.Gameplay.Weapon
{
    public enum BulletType
    {
        Player,
        Enemy
    }

    public class Bullet : MonoBehaviour
    {
        protected WeaponAttribute weaponAttribute;

        protected BulletType type;

        public GameObject HitVFXPrefab;
        public GameObject MuzzleVFXPrefab;
        public List<GameObject> trails;

        protected GameObject hitVFX;
        protected GameObject muzzleVFX;

        protected Vector3 originPos;
        protected RaycastHit2D[] hits = new RaycastHit2D[10]; //射线检测

        protected float airPlaneSpeed;

        private LayerMask layerMask;

        public virtual void Initialize(WeaponAttribute weaponAttribute, float airplaneSpeed, BulletType type)
        {
            this.type = type;
            if (type == BulletType.Player) layerMask = LayerMask.GetMask("Enemy");
            else if (type == BulletType.Enemy) layerMask = LayerMask.GetMask("Player", "UAV");

            this.weaponAttribute = weaponAttribute;
            this.airPlaneSpeed = airplaneSpeed;
            InitialMuzzleVFX();
        }

        private void Start()
        {
            originPos = transform.localPosition;
        }

        void Update()
        {
            if (weaponAttribute.Speed > 0)
            {
                transform.Translate(transform.forward * (weaponAttribute.Speed + airPlaneSpeed) * Time.deltaTime, Space.World);
                if (Vector3.Distance(transform.localPosition, originPos) >= weaponAttribute.Range) Destroy();
            }

            Raycast();
        }

        public virtual void OnHitTarget(Collider2D collider)
        {
            IBulletDamageable[] bulletDamageables = collider.GetComponents<IBulletDamageable>();
            foreach (var bulletDamageable in bulletDamageables)
            {
                if (Random.Range(0f, 1f) <= weaponAttribute.CriticalProbability)
                {
                    bulletDamageable.TakeDamage(DamageType.Critical, weaponAttribute.Damage * weaponAttribute.CriticalRatio);
                }
                    
                else
                {
                    bulletDamageable.TakeDamage(DamageType.Basics, weaponAttribute.Damage);
                }                   
            }

            Destroy();
        }

        public virtual void Destroy()
        {
            Destroy(this.gameObject);
        }

        protected void Raycast()
        {
            int count = RaycastNonAlloc();
            for (int i = 0; i < count; i++)
            {
                RaycastHit2D hit = hits[i];
                if (hit.collider != null)
                {               
                    OnHitTarget(hit.collider);
                    InitialHitVFX();
                }
            }
        }

        protected int RaycastNonAlloc()
        {
            return Physics2D.RaycastNonAlloc(transform.position, transform.forward, hits, 0.5f, layerMask);
        }

        /// <summary>
        /// 开火特效
        /// </summary>
        protected void InitialMuzzleVFX()
        {
            muzzleVFX = Object.Instantiate(MuzzleVFXPrefab);
            muzzleVFX.transform.localPosition = transform.localPosition;
            var ps = muzzleVFX.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(ps, ps.main.duration);
            }
            else
            {
                var psChild = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(muzzleVFX, psChild.main.duration);
            }
        }

        /// <summary>
        /// 命中特效
        /// </summary>
        /// <param name="position"></param>
        /// <param name="rotation"></param>
        protected void InitialHitVFX()
        {
            hitVFX = Instantiate(HitVFXPrefab);
            hitVFX.transform.localPosition = transform.localPosition;
            var ps = hitVFX.GetComponent<ParticleSystem>();
            if (ps != null)
            {
                Destroy(hitVFX, ps.main.duration);
            }
            else
            {
                var psChild = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitVFX, psChild.main.duration);
            }
        }

        /// <summary>
        /// 删除粒子特效
        /// </summary>
        /// <param name="waitTime"></param>
        /// <returns></returns>
        protected IEnumerator DestroyParticle(float waitTime)
        {

            if (transform.childCount > 0 && waitTime != 0)
            {
                List<Transform> tList = new List<Transform>();

                foreach (Transform t in transform.GetChild(0).transform)
                {
                    tList.Add(t);
                }

                while (transform.GetChild(0).localScale.x > 0)
                {
                    yield return new WaitForSeconds(0.01f);
                    transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                    for (int i = 0; i < tList.Count; i++)
                    {
                        tList[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                    }
                }
            }
            yield return new WaitForSeconds(waitTime);
            Destroy(gameObject);
        }

        protected void DestoryTrails()
        {
            if (trails.Count > 0)
            {
                for (int i = 0; i < trails.Count; i++)
                {
                    trails[i].transform.parent = null;
                    var ps = trails[i].GetComponent<ParticleSystem>();
                    if (ps != null)
                    {
                        ps.Stop();
                        Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
                    }
                }
            }
        }

    }
}


