using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.Framework.Utilities
{
    /// <summary>
    /// 计算Transform相关属性
    /// </summary>
    public static class TransformUtil
    {

        /// <summary>
        /// 获取给定半径下的圆环位置
        /// </summary>
        /// <param name="playerPos"></param>
        /// <param name="R1"></param>
        /// <param name="R2"></param>
        /// <param name="gridSize"></param>
        /// <param name="angleStep"></param>
        /// <returns></returns>
        public static List<Vector3> GetRingGridPositions(Vector3 playerPos, float R1, float R2, float gridSize, float angleStep = 2f)
        {
            List<Vector3> ringGrids = new List<Vector3>();

            // 角度从 0 到 360 度遍历，步进 angleStep（避免计算方形区域的无效点）
            for (float angle = 0; angle < 360f; angle += angleStep)
            {
                float radian = angle * Mathf.Deg2Rad; // 角度转换为弧度

                // 在当前角度方向上，从 R2 到 R1 逐步增加
                for (float r = R2; r <= R1; r += gridSize)
                {
                    float offsetX = r * Mathf.Cos(radian); // X 偏移
                    float offsetY = r * Mathf.Sin(radian); // Y 偏移

                    Vector3 gridPos = playerPos + new Vector3(offsetX, offsetY, 0);

                    // 确保点是网格对齐的（可选）
                    gridPos.x = Mathf.Round(gridPos.x / gridSize) * gridSize;
                    gridPos.y = Mathf.Round(gridPos.y / gridSize) * gridSize;

                    // 避免重复添加相同的网格点
                    if (!ringGrids.Contains(gridPos))
                    {
                        ringGrids.Add(gridPos);
                    }
                }
            }
            return ringGrids;
        }

        /// <summary>
        /// 从候选点中查找距离目标位置最近的点
        /// </summary>
        public static Vector3 FindClosestPoint(Vector3 targetPosition, List<Vector3> candidatePoints)
        {
            if (candidatePoints.Count == 0) return targetPosition;

            Vector3 closestPoint = Vector3.zero;
            float closestDistanceSqr = float.MaxValue;

            foreach (Vector3 point in candidatePoints)
            {
                float distanceSqr = (point - targetPosition).sqrMagnitude; // 使用平方距离，性能更好
                if (distanceSqr < closestDistanceSqr)
                {
                    closestDistanceSqr = distanceSqr;
                    closestPoint = point;
                }
            }

            return closestPoint;
        }

        public static Vector3 GetRandomPosition(Vector3 origin, float range)
        {
            float x = Random.Range(-range, range);
            float y = Random.Range(-range, range);

            return new Vector3(origin.x + x, origin.y + y, origin.z);
        }

        public static Quaternion GetLookRotation(Transform origin, Vector3 target)
        {
            Vector3 direction = target - origin.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            Quaternion rotation = Quaternion.AngleAxis(angle, origin.forward);
            return rotation;
        }

    }
}



