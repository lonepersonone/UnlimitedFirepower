using MyGame.Gameplay.Upgrade;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Scene.Main
{
    public class LogicInitializer
    {
        public static void InitialLogic(MainDataManager data)
        {
            UpgradeController.Instance.Initalize(data);
        }

    }
}


