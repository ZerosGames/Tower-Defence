using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TurretController : MonoBehaviour
{
    //Private Variables\\
       
    [SerializeField]
    private Collider[] targetColliders;

    [SerializeField]
    private Transform target;

    [SerializeField]
    private GameObject rangeObject;

    [SerializeField]
    private tData turretData = new tData();

    [SerializeField]
    private bool placed = false;

    //Public Variables\\

    public GameObject rotPoint;
    public GameObject hitEffectPrefab;
    public Transform[] firePoint = new Transform[2];
    public enum TurretUpgradeTypes
    {
        Damage,
        FireRate,
        Range
    }

    private void Start ()
    {
        InvokeRepeating("UpdateTargets", 0f, 0.5f);
	}

    private void UpdateTargets()
    {
        if (target == null)
        {
            targetColliders = Physics.OverlapSphere(transform.position, turretData.tRange, turretData.layerMask);

            float shortestDistance = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (Collider _hit in targetColliders)
            {
                GameObject Temp = _hit.gameObject;

                float distanceToEnemy = Vector3.Distance(transform.position, Temp.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    shortestDistance = distanceToEnemy;
                    nearestEnemy = Temp;
                }

            }

            if (nearestEnemy != null && shortestDistance <= turretData.tRange)
            {
                target = nearestEnemy.transform;
                return;
            }

            target = null;
        }
    }

    private void Update ()
    {
        if (!GameManager.gameManager.GetPause() && placed)
        {
            ShowRange(BuildManager.BuildManagerInstance.GetBuildingMode());

            if (target != null && Vector3.Distance(transform.position, target.position) > turretData.tRange)
            {
                target = null;
            }

            if (target == null)
                return;

            Vector3 dir = target.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 Rotation = Quaternion.Lerp(rotPoint.transform.rotation, lookRotation, Time.deltaTime * turretData.tTurnSpeed).eulerAngles;
            rotPoint.transform.rotation = Quaternion.Euler(0, Rotation.y, 0);

            if (turretData.fireRateCoolDown <= 0)
            {
                fireAtTarget();

                turretData.fireRateCoolDown = 1f / turretData.tFireRate;
            }

            turretData.fireRateCoolDown -= Time.deltaTime;
        }       
	}

    private void fireAtTarget()
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = target.position - transform.position;
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(ray, out hit, (float)turretData.tRange, turretData.layerMask))
        {          
            EnemyController Enemy = hit.transform.gameObject.GetComponent<EnemyController>() as EnemyController;

            if (Enemy == null)
                return;

            Enemy.TakeDamage(turretData.tDamage, turretData.tPenatration);
        }
    }

    public void setRange(float _range)
    {
        turretData.tRange = _range;
    }

    public void SetPlaced(bool _placed)
    {
        placed = _placed;
    }

    public void ShowRange(bool _show)
    {
        //rangeObject.SetActive(_show);
    }
    
    public tData GetTurretData()
    {
        return turretData;
    }

    public void UpgradeTurret(TurretUpgradeTypes _TypeOfUpgrade)
    {
        switch (_TypeOfUpgrade)
        {   
            case TurretUpgradeTypes.Damage:
                turretData.tDamage += 10;
                if (turretData.tDamage > turretData.tMaxDamage)
                {
                    turretData.tDamage = turretData.tMaxDamage;
                }
                break;
            case TurretUpgradeTypes.FireRate:
                turretData.tFireRate += 5;
                if (turretData.tFireRate > turretData.tMaxFireRate)
                {
                    turretData.tFireRate = turretData.tMaxFireRate;
                }
                break;
            case TurretUpgradeTypes.Range:
                turretData.tRange += 1;
                if (turretData.tRange > turretData.tMaxRange)
                {
                    turretData.tRange = turretData.tMaxRange;
                }
                break;
            default:
                break;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, turretData.tRange);
    }
}
