using UnityEngine;
using UnityEngine.UI;

public class UITurretStats : MonoBehaviour {

    private struct TurretStats
    {
        public int Damage;
        public float FireRate;
        public int Penetration;
        public int ShieldPenetration;
        public float Range;
    }

    TurretStats turretStats;

    [SerializeField]
    private Image DamageText;

    [SerializeField]
    private Image FireRateText;

    [SerializeField]
    private Image PenetrationText;

    [SerializeField]
    private Image ShieldPenetrationText;

    [SerializeField]
    private Image RangeText;

    private RectTransform Rtransfrom;

    // Use this for initialization
    void Start ()
    {
       Rtransfrom = GetComponent<RectTransform>();
       turretStats = new TurretStats();
	}

    public void ShowUI(bool _show)
    {
        if (_show)
        {
            Rtransfrom.anchoredPosition = new Vector2(-352.5f, 0);
        }
        else
        {
            Rtransfrom.anchoredPosition = new Vector2(10, 0);
        }

        SetUIValues(_show);
    }

    private void SetUIValues(bool _showen)
    {
        if (_showen)
        {
            DamageText.fillAmount = (float)turretStats.Damage / 100;
            FireRateText.fillAmount = turretStats.FireRate / 20;
            PenetrationText.fillAmount = (float)turretStats.Penetration / 10;
            ShieldPenetrationText.fillAmount = (float)turretStats.ShieldPenetration / 20;
            RangeText.fillAmount = turretStats.Range / 50;
        }
        else
        {
            turretStats = new TurretStats();
        }

    }

    public void Setdamage(int _dmg)
    {
        turretStats.Damage = _dmg;
    }

    public void SetFireRate(float _fireRate)
    {
        turretStats.FireRate = _fireRate;
    }

    public void SetArmourPenetration(int _penetration)
    {
        turretStats.Penetration = _penetration;
    }

    public void SetShieldPenetration(int _shieldPen)
    {
        turretStats.ShieldPenetration = _shieldPen;
    }

    public void SetTurretRange(float _range)
    {
        turretStats.Range = _range;
    }
}
