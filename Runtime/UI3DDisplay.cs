﻿//------------------------------------------------------------
// Author: 亦亦
// Mail: 379338943@qq.com
// Data: 2023年2月12日
//------------------------------------------------------------

using System;
using ET;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace YIUIFramework
{
    /// <summary>
    /// 这个类用于在UI中显示3D对象。
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/FhGGwVZSyiCqHCkTVQYcKHQCnKf
    /// </summary>
    public partial class UI3DDisplay : SerializedMonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("[动态] 展示的对象")]
        public GameObject m_ShowObject;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("[动态] 观察的摄像机")]
        public Camera m_LookCamera;

        [Required]
        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("面板")]
        public RawImage m_ShowImage;

        [Required]
        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("摄像机")]
        public Camera m_ShowCamera;

        [Required]
        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("摄像机控制器")]
        public UI3DDisplayCamera m_ShowCameraCtrl;

        [Required]
        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("灯光")]
        public Light m_ShowLight;

        [Required]
        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("这个变换将自动适应比例")]
        public Transform m_FitScaleRoot;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("自动设置图像大小")]
        public bool m_AutoChangeSize = true;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("图像宽")]
        [MinValue(0)]
        public int m_ResolutionX = 512;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("图像高")]
        [MinValue(0)]
        public int m_ResolutionY = 512;

        [NonSerialized]
        [ShowInInspector]
        [OdinSerialize]
        [LabelText("深度值")] //默认16 不懂的最好不要改
        public int m_RenderTextureDepthBuffer = 16;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("允许拖拽")]
        public bool m_CanDrag = true;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("拖拽速度")]
        [ShowIf("m_CanDrag")]
        public float m_DragSpeed = 10.0f;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("显示对象的位置偏移值")]
        public Vector3 m_ShowOffset = Vector3.zero;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("显示对象的旋转偏移值")]
        public Vector3 m_ShowRotation = Vector3.zero;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("显示对象的比例")]
        public Vector3 m_ShowScale = Vector3.one;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("镜面反射面")]
        public Transform m_ReflectionPlane;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("阴影面")]
        public Transform m_ShadowPlane;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("使用观察摄像机的颜色")]
        public bool m_UseLookCameraColor;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("自动同步")]
        public bool m_AutoSync;

        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        [LabelText("多目标模式")]
        public bool m_MultipleTargetMode;

        [MinValue(0)]
        [LabelText("点击时允许偏移值")]
        [NonSerialized]
        [OdinSerialize]
        [ShowInInspector]
        public Vector2 m_OnClickOffset = new Vector2(50, 50);

        [NonSerialized]
        [HideInInspector]
        public EntityRef<Entity> m_YIUI3DDisplayChildRef;

        public Entity YIUI3DDisplayChild => m_YIUI3DDisplayChildRef;

        [NonSerialized]
        [HideInInspector]
        public string m_OnClickInvokeName;

        public void OnDrag(PointerEventData eventData)
        {
            if (!m_CanDrag) return;
            YIUIInvokeSystem.Instance?.Invoke(YIUI3DDisplayChild, "OnDrag", eventData);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(m_OnClickInvokeName)) return;
            YIUIInvokeSystem.Instance?.Invoke(YIUI3DDisplayChild, "OnPointerDown", eventData);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (string.IsNullOrEmpty(m_OnClickInvokeName)) return;
            YIUIInvokeSystem.Instance?.Invoke(YIUI3DDisplayChild, "OnPointerUp", eventData);
        }

        #if UNITY_EDITOR
        private void OnValidate()
        {
            if (m_AutoChangeSize)
            {
                var rect = transform.GetComponent<RectTransform>();
                if (Math.Abs(rect.sizeDelta.x - m_ResolutionX) > 0.01f || Math.Abs(rect.sizeDelta.y - m_ResolutionY) > 0.01f)
                    rect.sizeDelta = new Vector2(m_ResolutionX, m_ResolutionY);
            }
        }
        #endif
    }
}