using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour {

    public eData enemyData = new eData();

    public Image HealthBar;

	void Start () {

        enemyData.health = enemyData.maxHealth;
    }

	void Update () {
	
        if(enemyData.health <= 0)
        {
            Destory(true);
        }
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
}
