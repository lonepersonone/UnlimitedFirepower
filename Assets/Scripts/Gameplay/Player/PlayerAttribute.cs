using MyGame.Data.SO;
using System.Collections.Generic;

namespace MyGame.Gameplay.Player
{
    /// <summary>
    /// �洢���������������
    /// </summary>
    public class PlayerAttribute
    {
        // Ship����

        private CharacterDataSO FrigateData;
        private CharacterDataSO BattleShipData;
        private CharacterDataSO DestroyerData;
        private CharacterDataSO CruiserData;

        // ���˻�����
        private CharacterDataSO UAVData;

        public CharacterAttribute ShipAttribute;
        public CharacterAttribute UAVAttribute;

        private Queue<CharacterDataSO> shipQueue = new Queue<CharacterDataSO>();

        public PlayerAttribute(ScriptableManager scriptable)
        {
            FrigateData = scriptable.GetCharacterById("Frigate"); 
            BattleShipData = scriptable.GetCharacterById("BattleShip"); 
            DestroyerData = scriptable.GetCharacterById("Destroyer");
            CruiserData = scriptable.GetCharacterById("Cruiser");

            shipQueue.Enqueue(FrigateData);            
            shipQueue.Enqueue(DestroyerData);
            shipQueue.Enqueue(CruiserData);
            shipQueue.Enqueue(BattleShipData);

            UAVData = scriptable.GetCharacterById("UAV");

            ShipAttribute = new CharacterAttribute(shipQueue.Dequeue());
        }

        public bool ChangeShip()
        {
            if(shipQueue.Count > 0)
            {
                ShipAttribute.ChangeCharacterData(shipQueue.Dequeue());
                return true;
            }
            return false;
        }

    }

}


