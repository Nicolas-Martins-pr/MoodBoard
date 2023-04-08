using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UpgradeType{
    VitesseUp,
    SlowBlackCombat,
    ExtraShot
}

public class Upgrade : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private UpgradeType _upgrade;

    public void SetUpgrateType(UpgradeType upgradeType)
    {
        _upgrade = upgradeType; 
    }

    public UpgradeType GetUpgradeType()
    {
        return _upgrade;
    }
}
