using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Framework.Manager
{
    public class MouseManager : MonoBehaviour
    {
        public static MouseManager Instance;
        private Texture2D battleCursor;
        private CursorMode cursorMode = CursorMode.Auto;
        private Vector2 hotSpot = Vector2.zero;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            battleCursor = Resources.Load<Texture2D>("Mouse/Crosshair1");
        }

        public IEnumerator EnableBattleCursor()
        {
            Cursor.SetCursor(battleCursor, hotSpot, cursorMode);
            yield return null;
        }

        public IEnumerator DisableBattleCursor()
        {
            Cursor.SetCursor(null, hotSpot, cursorMode);
            yield return null;
        }

    }
}



