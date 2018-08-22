﻿using UnityEngine;

namespace FSpace
{
    /// <summary>
    /// 绘制一条从笔尖发出的射线
    /// </summary>
    internal class PenRay : MonoBehaviour
    {
        /// <summary>
        /// 笔的射线的LineRenderer
        /// </summary>
        private LineRenderer _lineRenderer;

        /// <summary>
        /// 射线的最长长度
        /// </summary>
        private float MAX_RAY_LENGTH = 1.0f;

        private void Awake()
        {
            _lineRenderer = gameObject.AddComponent<LineRenderer>();
            _lineRenderer.material = new Material(Shader.Find("Unlit/Texture"));
        }

        private void Update()
        {
            float rayLength = MAX_RAY_LENGTH * FCore.ViewerScale;//射线初始设置长度1m长

            _lineRenderer.SetPosition(0, FCore.penPosition);
            if (FCore.isDraging)//如果当前正在拖拽状态
            {
                //使用记录的拖拽距离来设置射线长
                _lineRenderer.SetPosition(1, FCore.penPosition + FCore.lastDragDistance * FCore.penDirection.normalized);
            }
            else//如果不在拖拽状态
            {
                RaycastHit raycastHit;
                int layer = LayerMask.NameToLayer("Default");
                if (Physics.Raycast(FCore.penRay, out raycastHit, rayLength, 1 << layer))//使用设定的射线长度来做射线检测
                {
                    _lineRenderer.SetPosition(1, raycastHit.point);
                }
                else
                {
                    _lineRenderer.SetPosition(1, FCore.penPosition + (rayLength * FCore.penDirection.normalized));
                }
            }

#if UNITY_5_2 || UNITY_5_3
            _lineRenderer.SetWidth(0.001f * FCore.ViewerScale, 0.001f * FCore.ViewerScale);
#else
             _lineRenderer.startWidth = 0.001f * FCore.ViewerScale;
            _lineRenderer.endWidth = 0.001f * FCore.ViewerScale;
#endif
        }

        private void OnDestroy()
        {
            Destroy(_lineRenderer.material);
        }
    }
}