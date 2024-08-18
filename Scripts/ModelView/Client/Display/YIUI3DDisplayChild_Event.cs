using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    /// <summary>
    /// 3DDisplay的扩展组件
    /// 文档: https://lib9kmxvq7k.feishu.cn/wiki/FhGGwVZSyiCqHCkTVQYcKHQCnKf
    /// </summary>
    public partial class YIUI3DDisplayChild
    {
        //可拖拽目标
        public GameObject m_DragTarge;

        public bool CanDrag
        {
            get => UI3DDisplay.m_CanDrag;
            set => UI3DDisplay.m_CanDrag = value;
        }

        public Vector2           m_OnClickDownPos = Vector2.zero;
        public RaycastHit        m_ClickRaycastHit;
        public RaycastHit        m_DragRaycastHit;
        public EntityRef<Entity> m_OnClickedEntity; //被点击的触发实体

        public Entity OnClickedEntity => m_OnClickedEntity;

        //被点击的触发实体的调用名称
        //参数1 被点击的对象 参数2 他的最终父级是谁(显示对象)
        private string m_OnClickInvokeName;

        public string OnClickInvokeName
        {
            get => m_OnClickInvokeName;
            set
            {
                m_OnClickInvokeName = value;
                if (UI3DDisplay)
                {
                    UI3DDisplay.m_OnClickInvokeName = value;
                }
            }
        }
    }
}
