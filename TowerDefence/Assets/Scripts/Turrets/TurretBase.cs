using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TurretBase : MonoBehaviour
{
    //Private Variables\\
       
    private Collider[] targetColliders;
    private Transform target;
    private GameObject rangeObject;
    private bool placed = false;
    private Node placementNode;

    public tData turretData = new tData();
    public float updateTargetSeconds = 0.2f;

    //Public Variables\\

    public bool trageting = true;
    public GameObject rotPoint;
    public GameObject hitEffectPrefab;
    public Transform[] firePoint = new Transform[2];

    private void Start ()
    {
        StartCoroutine(UpdateTargets());
	}

    private IEnumerator UpdateTargets()
    {
        while (trageting)
        {
            if (target != null)
            {
                yield return new WaitForSeconds(updateTargetSeconds);
            }

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
                yield return new WaitForSeconds(updateTargetSeconds);
            }

            yield return new WaitForSeconds(updateTargetSeconds);
        }
    }

    private void Update ()
    {
        if (!GameManager.gameManager.GetPause() && placed)
        {
            ShowRange(References.Refs.buildManager.GetBuildingMode());

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
                Fire();

                turretData.fireRateCoolDown = 1f / turretData.tFireRate;
            }

            turretData.fireRateCoolDown -= Time.deltaTime;
        }       
	}

    protected virtual void Fire()
    {
        Ray ray = new Ray();
        ray.origin = transform.position;
        ray.direction = target.position - transform.position;
        RaycastHit hit = new RaycastHit();

        if(Physics.Raycast(ray, out hit, (float)turretData.tRange, turretData.layerMask))
        {          
            EnemyBase Enemy = hit.transform.gameObject.GetComponent(typeof(EnemyBase)) as EnemyBase;

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
                turretData.tDamage += turretData.tDamagePreUpgrade;
                turretData.costToUpgradeDamage += turretData.costPerUpgrade;
                if (turretData.tDamage > turretData.tMaxDamage)
                {
                    turretData.tDamage = turretData.tMaxDamage;
                }
                break;
            case TurretUpgradeTypes.FireRate:
                turretData.tFireRate += turretData.tFireRatePreUpgrade;
                turretData.costToUpgradeFirerate += turretData.costPerUpgrade;
                if (turretData.tFireRate > turretData.tMaxFireRate)
                {
                    turretData.tFireRate = turretData.tMaxFireRate;
                }
                break;
            case TurretUpgradeTypes.Range:
                turretData.tRange += turretData.tRangePreUpgrade;
                turretData.costToUpgradeRange += turretData.costPerUpgrade;
                if (turretData.tRange > turretData.tMaxRange)
                {
                    turretData.tRange = turretData.tMaxRange;
                }
                break;
            default:
                break;
        }
    }

    public void SellTurret()
    {
        References.Refs.playerData.currentcurrency += turretData.SellTurretCost;
        References.turretPlaced.Remove(gameObject);
        placementNode.Placeable = true;
        Destroy(gameObject);
    }

    public void SetPlacementNode(Node _node)
    {
        placementNode = _node;
    }

    public Node GetPlacementNode()
    {
        return placementNode;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, turretData.tRange);
    }
}
