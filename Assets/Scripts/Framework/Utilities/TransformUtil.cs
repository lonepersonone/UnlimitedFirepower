using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MyGame.Framework.Utilities
{
    /// <summary>
    /// ����Transform�������
    /// </summary>
    public static class TransformUtil
    {

        /// <summary>
        /// ��ȡ�����뾶�µ�Բ��λ��
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

            // �Ƕȴ� 0 �� 360 �ȱ��������� angleStep��������㷽���������Ч�㣩
            for (float angle = 0; angle < 360f; angle += angleStep)
            {
                float radian = angle * Mathf.Deg2Rad; // �Ƕ�ת��Ϊ����

                // �ڵ�ǰ�Ƕȷ����ϣ��� R2 �� R1 ������
                for (float r = R2; r <= R1; r += gridSize)
                {
                    float offsetX = r * Mathf.Cos(radian); // X ƫ��
                    float offsetY = r * Mathf.Sin(radian); // Y ƫ��

                    Vector3 gridPos = playerPos + new Vector3(offsetX, offsetY, 0);

                    // ȷ�������������ģ���ѡ��
                    gridPos.x = Mathf.Round(gridPos.x / gridSize) * gridSize;
                    gridPos.y = Mathf.Round(gridPos.y / gridSize) * gridSize;

                    // �����ظ������ͬ�������
                    if (!ringGrids.Contains(gridPos))
                    {
                        ringGrids.Add(gridPos);
                    }
                }
            }
            return ringGrids;
        }

        /// <summary>
        /// �Ӻ�ѡ���в��Ҿ���Ŀ��λ������ĵ�
        /// </summary>
        public static Vector3 FindClosestPoint(Vector3 targetPosition, List<Vector3> candidatePoints)
        {
            if (candidatePoints.Count == 0) return targetPosition;

            Vector3 closestPoint = Vector3.zero;
            float closestDistanceSqr = float.MaxValue;

            foreach (Vector3 point in candidatePoints)
            {
                float distanceSqr = (point - targetPosition).sqrMagnitude; // ʹ��ƽ�����룬���ܸ���
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



