using System;
using UnityEngine;
using YIUIFramework;
using System.Collections.Generic;

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
            await self.Display.ShowAsync("DisplayDemoModel","CustomCamera");
        }

        #region YIUIEvent开始

        [YIUIInvoke]
        private static async ETTask OnEventCloseInvoke(this ModelDisplayDemoViewComponent self)
        {
            await self.UIView.CloseAsync();
        }

        #endregion YIUIEvent结束
    }
}