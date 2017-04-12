using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour {

    public eData enemyData = new eData();

    public Image HealthBar;

    //EnemyMovement
    int nodeIndex = 1;
    List<Node> path;
    Node CurrentNode;

    //EnemyRotation
    [SerializeField]
    Transform meshRotationpoint;

    // Use this for initialization
    void Start ()
    {
        InitData();	
	}

	// Update is called once per frame
	void Update ()
    {
        if (enemyData.health <= 0)
        {
            Destory(true);
        }

        Move();     
    }

    protected virtual void Move()
    {
        if (!GameManager.gameManager.GetPause() && path.Count > 0)
        {
            if (Vector3.Distance(transform.position, CurrentNode.WorldPos) < 0.1f)
            {
                nodeIndex++;
            }

            if (nodeIndex < path.Count)
            {
                CurrentNode = path[nodeIndex];
            }
            else
            {
                Destory(false);
            }

            Vector3 dir =  CurrentNode.WorldPos - transform.position;

            dir.Normalize();

            Quaternion lookRotation = new Quaternion();
            lookRotation.SetLookRotation(dir);

            meshRotationpoint.rotation = lookRotation;

            transform.Translate(dir * enemyData.speed * Time.deltaTime);
        }
    }

    void InitData()
    {
        CurrentNode = path[nodeIndex];
        enemyData.health = enemyData.maxHealth;
    }

    public void Destory(bool _killed)
    {
        if (_killed)
        {
            PlayerData.playerData.currentcurrency += enemyData.worth;
        }
        else
        {
            GameManager.gameManager.DecreaseLives();
        }

        EnemySpawner.enemyAlive--;
        References.enemysAlive.Remove(gameObject);

        Destroy(gameObject);
    }

    public void TakeDamage(int _Damage, int _Penatraition)
    {
        enemyData.health -= (_Damage / enemyData.armourValue) * _Penatraition;
        float percent = enemyData.health / enemyData.maxHealth;
        HealthBar.fillAmount = percent;
    }

    public void SetPath(List<Node> _path)
    {
        path = _path;
    }
}
