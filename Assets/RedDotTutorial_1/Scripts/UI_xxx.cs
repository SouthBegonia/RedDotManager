using UnityEngine;
using UnityEngine.UI;

namespace RedDotTutorial_1
{
    /// <summary>
    /// 业务UI
    /// </summary>
    public class UI_xxx : MonoBehaviour
    {
        /// <summary>
        /// 邮箱 红点
        /// </summary>
        public RedDotItem MailDot;
        /// <summary>
        /// 邮箱->系统 红点
        /// </summary>
        public RedDotItem MailSystemDot;
        /// <summary>
        /// 邮箱->队伍 红点
        /// </summary>
        public RedDotItem MailTeamDot;

        /*
* 驱动层：注册监听红点、红点触发，并通知表现层
*/

        void Start()
        {
            //注册红点，通常放在 UI.OnInit 或 UI.OnOpen 中
            ManagerComponent.RedDotManager.SetRedDotNodeCallBack(E_RedDotDefine.MailBox, MailCallBack);
            ManagerComponent.RedDotManager.SetRedDotNodeCallBack(E_RedDotDefine.MailBox_System, MailSystemCallBack);
            ManagerComponent.RedDotManager.SetRedDotNodeCallBack(E_RedDotDefine.MailBox_Team, MailTeamCallBack);

            //初始显示红点信息
            ManagerComponent.RedDotManager.Set(E_RedDotDefine.MailBox_System, 3);
            ManagerComponent.RedDotManager.Set(E_RedDotDefine.MailBox_Team, 2);
        }

        private void OnDestroy()
        {
            //注销红点，通常放在 UI.OnClose 中
            ManagerComponent.RedDotManager.SetRedDotNodeCallBack(E_RedDotDefine.MailBox, null);
            ManagerComponent.RedDotManager.SetRedDotNodeCallBack(E_RedDotDefine.MailBox_System, null);
            ManagerComponent.RedDotManager.SetRedDotNodeCallBack(E_RedDotDefine.MailBox_Team, null);
        }

        void MailCallBack(RedDotNode node)
        {
            MailDot.SetDotState(node.rdCount > 0, node.rdCount);
        }

        void MailSystemCallBack(RedDotNode node)
        {
            MailSystemDot.SetDotState(node.rdCount > 0, node.rdCount);
        }

        void MailTeamCallBack(RedDotNode node)
        {
            MailTeamDot.SetDotState(node.rdCount > 0, node.rdCount);
        }


        #region GM按钮的点击事件

        public void OnAddRdSystemBtnClick()
        {
            int count = ManagerComponent.RedDotManager.GetRedDotCount(E_RedDotDefine.MailBox_System);
            ManagerComponent.RedDotManager.Set(E_RedDotDefine.MailBox_System, count + 1);
        }
        public void OnAddRdTeamBtnClick()
        {
            int count = ManagerComponent.RedDotManager.GetRedDotCount(E_RedDotDefine.MailBox_Team);
            ManagerComponent.RedDotManager.Set(E_RedDotDefine.MailBox_Team, count + 1);
        }
        public void OnReduceRdSystemBtnClick()
        {
            int count = ManagerComponent.RedDotManager.GetRedDotCount(E_RedDotDefine.MailBox_System);
            ManagerComponent.RedDotManager.Set(E_RedDotDefine.MailBox_System, count - 1);
        }
        public void OnReduceRdTeamBtnClick()
        {
            int count = ManagerComponent.RedDotManager.GetRedDotCount(E_RedDotDefine.MailBox_Team);
            ManagerComponent.RedDotManager.Set(E_RedDotDefine.MailBox_Team, count - 1);
        }

        #endregion
    }
}