using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MyGame.Framework.Utilities
{
    public static class EventSystemUtil
    {
        public static GameObject GetMosueOverUI(GameObject canvas)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);

            if (results.Count != 0)
            {
                return results[0].gameObject;
            }

            return null;
        }

        public static GameObject[] GetMouseOverUIs(GameObject canvas)
        {
            PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            pointerEventData.position = Input.mousePosition;
            GraphicRaycaster gr = canvas.GetComponent<GraphicRaycaster>();
            List<RaycastResult> results = new List<RaycastResult>();
            gr.Raycast(pointerEventData, results);
            GameObject[] result = new GameObject[results.Count];

            if (results.Count != 0)
            {
                for (int i = 0; i < results.Count; i++)
                {
                    result[i] = results[i].gameObject;
                }
                return result;
            }
            return null;
        }

    }
}


