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

        private static Camera GetCamera(this YIUI3DDisplayChild self, GameObject obj, string cameraName)
        {
            if (!self.m_CameraPool.ContainsKey(obj))
            {
                self.m_CameraPool.Add(obj, new Dictionary<string, Camera>());
            }

            var objDic = self.m_CameraPool[obj];

            if (!objDic.ContainsKey(cameraName))
            {
                var camera = self.GetCameraByName(obj, cameraName);
                if (camera == null) return null;
                objDic.Add(cameraName, camera);
            }

            return objDic[cameraName];
        }

        private static Camera GetCameraByName(this YIUI3DDisplayChild self, GameObject obj, string cameraName)
        {
            var cameraTsf = obj.transform.FindChildByName(cameraName);
            if (cameraTsf == null)
            {
                Debug.LogError($"{obj.name} 没有找到目标摄像机 {cameraName} 请检查 将使用默认摄像机");
                return self.UI3DDisplay.m_ShowCamera;
            }

            var camera = cameraTsf.GetComponent<Camera>();
            if (camera == null)
            {
                Debug.LogError($"{obj.name} 没有找到目标摄像机组件 {cameraName} 请检查 将使用默认摄像机");
                return self.UI3DDisplay.m_ShowCamera;
            }

            return camera;
        }
    }
}