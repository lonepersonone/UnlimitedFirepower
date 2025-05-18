using UnityEngine;


namespace MyGame.Gameplay.Upgrade
{
    [System.Serializable]
    public class UpgradeData
    {
        public float[] Values; //ֵ
        public int Cost; //����

        public UpgradeData Clone()
        {
            return new UpgradeData
            {
                Values = this.Values,
                Cost = this.Cost
            };
        }
    }

}


