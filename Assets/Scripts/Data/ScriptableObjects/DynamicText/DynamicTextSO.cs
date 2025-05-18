using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data.SO
{
    [CreateAssetMenu(fileName = "NewDynamicText", menuName = "UI/DynamicTextController")]
    public class DynamicTextSO : ScriptableObject, IGameData
    {
        public string ID;
        public GameObject DynamicTextPrefab;
        public DynamicTextData DynamicTextData;

        string IGameData.ID => ID;
    }

}

