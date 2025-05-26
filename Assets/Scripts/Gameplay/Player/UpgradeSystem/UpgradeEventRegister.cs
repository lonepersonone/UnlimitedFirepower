using MyGame.Scene.BattleRoom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Gameplay.Upgrade
{
    public class UpgradeEventRegister : MonoBehaviour
    {
        public static void RegistEvent(List<UpgradeItem> items)
        {
            foreach (UpgradeItem item in items)
            {
                if (item.Id == "Health") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.SetHealthOffset);
                else if (item.Id == "HealthRecover") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.SetHealthRecoverOffset);
                else if (item.Id == "Shield") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.SetShieldOffset);
                else if (item.Id == "ShieldRecover") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.SetShieldRecoverOffset);
                else if (item.Id == "DamageReduction") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.SetDamageReductionOffset);
                else if (item.Id == "MoveSpeed") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.SetMoveSpeedOffset);
                else if (item.Id == "ThrusterDuration") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.SetThrusterDurationOffset);
                else if (item.Id == "ThrusterRatio") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.SetThrusterRateOffset);
                else if (item.Id == "Rebirth") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.SetRebirthRateOffset);


                else if (item.Id == "WeaponAttenuation") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.WeaponData.SetAttenuationOffset);
                else if (item.Id == "WeaponCriticalLuky") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.WeaponData.SetCriticalProbabilityOffset);
                else if (item.Id == "WeaponCriticalRatio") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.WeaponData.SetCriticalRatioOffset);
                else if (item.Id == "WeaponDamage") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.WeaponData.SetDamageOffset);
                else if (item.Id == "WeaponFireRate") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.WeaponData.SetFireRateOffset);
                else if (item.Id == "WeaponRange") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.WeaponData.SetRangeOffset);

                else if (item.Id == "UAVCount") item.RegistEvent(BattleDataManager.Instance.PlayerAttribute.ShipAttribute.AddUAVConut);
            }
        }

        public static void UnRegistEvent(List<UpgradeItem> items)
        {
            foreach(var item in items)
            {
                item.UnRegistEvent();
            }
        }

    }
}


