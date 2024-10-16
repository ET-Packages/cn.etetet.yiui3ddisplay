using System;
using UnityEngine;
using YIUIFramework;

namespace ET.Client
{
    [FriendOf(typeof(ModelDisplayDemoViewComponent))]
    public static partial class ModelDisplayDemoViewComponentSystem
    {
        [EntitySystem]
        private static void YIUIInitialize(this ModelDisplayDemoViewComponent self)
        {
            self.m_Display = self.AddChild<YIUI3DDisplayChild, UI3DDisplay>(self.u_ComDisplay);
        }

        [EntitySystem]
        private static void Destroy(this ModelDisplayDemoViewComponent self)
        {
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this ModelDisplayDemoViewComponent self)
        {
            await ETTask.CompletedTask;
            return true;
        }

        [EntitySystem]
        private static async ETTask<bool> YIUIOpen(this ModelDisplayDemoViewComponent self, ParamVo vo)
        {
            await self.Show();
            return true;
        }

        private static async ETTask Show(this ModelDisplayDemoViewComponent self)
        {
            await self.Display.ShowAsync("DisplayDemoModel", "CustomCamera");
            self.Display.SetClickEvent(self.OnClickModel);
        }

        [YIUIInvoke]
        private static void OnClickModel(this ModelDisplayDemoViewComponent self, GameObject target, GameObject root)
        {
            Log.Info($"点击模型 目标: {target.name}  根节点:{root.name}");
        }

        #region YIUIEvent开始
        #endregion YIUIEvent结束
    }
}