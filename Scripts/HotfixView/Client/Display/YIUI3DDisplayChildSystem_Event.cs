using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 3DDisplay的扩展组件
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/FhGGwVZSyiCqHCkTVQYcKHQCnKf
    /// </summary>
    public static partial class YIUI3DDisplayChildSystem
    {
        //添加点击回调
        public static void SetClickEvent(this YIUI3DDisplayChild self, Entity owner, Action<GameObject, GameObject> action)
        {
            self.m_OnClickedEntity               = owner;
            self.m_OnClickInvokeName             = action.GetType().Name;
            self.UI3DDisplay.m_OnClickInvokeName = self.m_OnClickInvokeName;
        }

        //移除点击回调
        public static void RemoveClickEvent(this YIUI3DDisplayChild self)
        {
            self.m_OnClickedEntity               = null;
            self.m_OnClickInvokeName             = null;
            self.UI3DDisplay.m_OnClickInvokeName = null;
        }

        //从屏幕坐标发送射线检测
        public static bool Raycast(this YIUI3DDisplayChild self, Vector2 screenPoint, out RaycastHit hitInfo)
        {
            var rect = self.UI3DDisplay.transform as RectTransform;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, screenPoint, YIUIMgrComponent.Inst.UICamera, out var localScreenPoint) == false)
            {
                hitInfo = default;
                return false;
            }

            localScreenPoint -= rect.rect.min;
            var ray = self.UI3DDisplay.m_ShowCamera.ScreenPointToRay(localScreenPoint);
            return Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, 1 << self.m_ShowLayer);
        }

        //拖拽
        [YIUIInvoke]
        public static void OnDrag(this YIUI3DDisplayChild self, PointerEventData eventData)
        {
            if (!self.UI3DDisplay.m_CanDrag) return;

            if (!self.m_DragTarge || !(self.UI3DDisplay.m_DragSpeed > 0.0f)) return;

            var delta    = eventData.delta.x;
            var deltaRot = -self.UI3DDisplay.m_DragSpeed * delta * Time.deltaTime;
            var dragTsf  = self.m_DragTarge.transform;

            if (self.UI3DDisplay.m_MultipleTargetMode)
            {
                dragTsf.Rotate(Vector3.up * deltaRot, Space.World);
            }
            else
            {
                self.m_DragRotation += deltaRot;
                var showRotation = Quaternion.Euler(self.UI3DDisplay.m_ShowRotation);
                var showUp       = showRotation * Vector3.up;
                showRotation     *= Quaternion.AngleAxis(self.m_DragRotation, showUp);
                dragTsf.rotation =  showRotation;
            }
        }

        //按下
        [YIUIInvoke]
        public static void OnPointerDown(this YIUI3DDisplayChild self, PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(self.m_OnClickInvokeName))
                return;

            if (self.UI3DDisplay.m_MultipleTargetMode)
            {
                if (!self.Raycast(eventData.position, out self.m_DragRaycastHit))
                    return;

                self.m_DragTarge = self.GetMultipleTargetByClick(self.m_DragRaycastHit.collider.gameObject);
            }

            self.m_OnClickDownPos = eventData.position;
        }

        //抬起
        [YIUIInvoke]
        public static void OnPointerUp(this YIUI3DDisplayChild self, PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(self.m_OnClickInvokeName))
                return;

            if (!self.ClickSucceed(eventData.position))
                return;

            if (!self.Raycast(eventData.position, out self.m_ClickRaycastHit))
                return;

            var clickObj       = self.m_ClickRaycastHit.collider.gameObject;
            var clickObjParent = self.UI3DDisplay.m_MultipleTargetMode ? self.GetMultipleTargetByClick(clickObj) : self.UI3DDisplay.m_ShowObject;

            try
            {
                //参数1 被点击的对象 参数2 他的最终父级是谁(显示对象)
                YIUIInvokeSystem.Instance?.Invoke(self.OnClickedEntity, self.m_OnClickInvokeName, clickObj, clickObjParent);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        //范围检测 因为手机上会有偏移
        private static bool ClickSucceed(this YIUI3DDisplayChild self, Vector2 upPos)
        {
            var offset = upPos - self.m_OnClickDownPos;
            return Math.Abs(offset.x) <= self.UI3DDisplay.m_OnClickOffset.x && Math.Abs(offset.y) <= self.UI3DDisplay.m_OnClickOffset.y;
        }
    }
}