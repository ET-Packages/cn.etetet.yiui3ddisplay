using System;
using System.Collections.Generic;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 3DDisplay的扩展组件
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/FhGGwVZSyiCqHCkTVQYcKHQCnKf
    /// </summary>
    public static partial class YIUI3DDisplayChildSystem
    {
        //清除显示的对象
        public static void ClearShow(this YIUI3DDisplayChild self)
        {
            if (self.UI3DDisplay == null) return;
            if (self.m_ShowCameraCtrl != null)
            {
                self.m_ShowCameraCtrl.ShowObject = null;
            }

            self.DisableMeshRectShadow();
            self.RecycleLastShow(self.UI3DDisplay.m_ShowObject);
            self.UI3DDisplay.m_ShowObject = null;
        }

        //重置旋转
        public static void ResetRotation(this YIUI3DDisplayChild self)
        {
            if (self.UI3DDisplay == null) return;
            self.m_DragRotation = 0.0f;
            if (self.UI3DDisplay.m_ShowObject == null) return;

            var showTsf      = self.UI3DDisplay.m_ShowObject.transform;
            var showRotation = Quaternion.Euler(self.UI3DDisplay.m_ShowRotation);
            var showUp       = showRotation * Vector3.up;
            showRotation     *= Quaternion.AngleAxis(self.m_DragRotation, showUp);
            showTsf.rotation =  showRotation;
        }

        //设置旋转
        public static void SetRotation(this YIUI3DDisplayChild self, Vector3 rotation)
        {
            if (self.UI3DDisplay == null) return;
            self.UI3DDisplay.m_ShowRotation = rotation;
            if (self.UI3DDisplay.m_ShowObject == null) return;

            var showTsf      = self.UI3DDisplay.m_ShowObject.transform;
            var showRotation = Quaternion.Euler(self.UI3DDisplay.m_ShowRotation);
            var showUp       = showRotation * Vector3.up;
            showRotation     *= Quaternion.AngleAxis(self.m_DragRotation, showUp);
            showTsf.rotation =  showRotation;
        }

        //设置位置偏移
        public static void SetOffset(this YIUI3DDisplayChild self, Vector3 offset)
        {
            if (self.UI3DDisplay == null) return;
            self.UI3DDisplay.m_ShowOffset = offset;
            if (self.UI3DDisplay.m_ShowObject == null) return;

            var showTsf = self.UI3DDisplay.m_ShowObject.transform;
            showTsf.localPosition = self.m_ModelGlobalOffset + self.UI3DDisplay.m_ShowOffset;
            self.m_ShowPosition   = showTsf.localPosition;
        }

        //设置大小
        public static void SetScale(this YIUI3DDisplayChild self, Vector3 scale)
        {
            if (self.UI3DDisplay == null) return;
            self.UI3DDisplay.m_ShowScale = scale;
            if (self.UI3DDisplay.m_ShowObject == null) return;

            var showTsf = self.UI3DDisplay.m_ShowObject.transform;
            showTsf.localScale = self.UI3DDisplay.m_ShowScale;
        }

        //改变 当前的分辨率 一般情况下 都不会改变
        public static void ChangeResolution(this YIUI3DDisplayChild self, Vector2 newResolution)
        {
            if (self.UI3DDisplay == null) return;
            if (!(Math.Abs(newResolution.x - self.UI3DDisplay.m_ResolutionX) > 0.01f) && !(Math.Abs(newResolution.y - self.UI3DDisplay.m_ResolutionY) > 0.01f)) return;

            self.UI3DDisplay.m_ResolutionX = (int)Math.Round(newResolution.x);
            self.UI3DDisplay.m_ResolutionY = (int)Math.Round(newResolution.y);
            self.SetTemporaryRenderTexture();
        }
    }
}
