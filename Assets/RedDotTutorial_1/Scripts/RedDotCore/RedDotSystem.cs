using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedDotTutorial_1
{
    /// <summary>
    /// 红点路径定义
    /// </summary>
    public static class E_RedDotDefine
    {
        /// <summary>
        /// 红点树的根节点
        /// </summary>
        public const string rdRoot = "Root";


        // ---------- 业务红点 ----------

        public const string MailBox = "Root/Mail";
        public const string MailBox_System = "Root/Mail/System";
        public const string MailBox_Team = "Root/Mail/Team";
    }

    /// <summary>
    /// 红点系统
    /// </summary>
    public class RedDotSystem
    {
        public RedDotSystem()
        {
            InitRedDotTreeNode();
            Debug.Log("--------------- 初始化 RedDotSystem 完毕 ---------------");
        }

        /// <summary>
        /// 红点数变化通知委托
        /// </summary>
        /// <param name="node"></param>
        public delegate void OnRdCountChange(RedDotNode node);

        /// <summary>
        /// 红点树的的 Root节点
        /// </summary>
        private RedDotNode mRootNode;

        /// <summary>
        /// 红点路径的表（每次 E_RedDotDefine 添加完后此处也必须添加）
        /// </summary>
        private static List<string> lstRedDotTreeList = new List<string>
        {
            E_RedDotDefine.rdRoot,

            E_RedDotDefine.MailBox,
            E_RedDotDefine.MailBox_System,
            E_RedDotDefine.MailBox_Team,
        };


        #region 内部接口

        /// <summary>
        /// 初始化红点树
        /// </summary>
        private void InitRedDotTreeNode()
        {
            /*
            * 结构层：根据红点是否显示或显示数，自定义红点的表现方式
            */

            mRootNode = new RedDotNode {rdName = E_RedDotDefine.rdRoot};

            foreach (string path in lstRedDotTreeList)
            {
                string[] treeNodeAy = path.Split('/');
                int nodeCount = treeNodeAy.Length;
                RedDotNode curNode = mRootNode;

                if (treeNodeAy[0] != mRootNode.rdName)
                {
                    Debug.LogError("根节点必须为Root，检查 " + treeNodeAy[0]);
                    continue;
                }

                if (nodeCount > 1)
                {
                    for (int i = 1; i < nodeCount; i++)
                    {
                        if (!curNode.rdChildrenDic.ContainsKey(treeNodeAy[i]))
                        {
                            curNode.rdChildrenDic.Add(treeNodeAy[i], new RedDotNode());
                        }

                        curNode.rdChildrenDic[treeNodeAy[i]].rdName = treeNodeAy[i];
                        curNode.rdChildrenDic[treeNodeAy[i]].parent = curNode;

                        curNode = curNode.rdChildrenDic[treeNodeAy[i]];
                    }
                }
            }
        }

        #endregion

        #region 外部接口

        /// <summary>
        /// 设置红点数变化的回调
        /// </summary>
        /// <param name="strNode">红点路径，必须是 RedDotDefine </param>
        /// <param name="callBack">回调函数</param>
        public void SetRedDotNodeCallBack(string strNode, OnRdCountChange callBack)
        {
            var nodeList = strNode.Split('/');

            if (nodeList.Length == 1)
            {
                if (nodeList[0] != E_RedDotDefine.rdRoot)
                {
                    Debug.LogError("Get Wrong Root Node! current is " + nodeList[0]);
                    return;
                }
            }

            var node = mRootNode;
            for (int i = 1; i < nodeList.Length; i++)
            {
                if (!node.rdChildrenDic.ContainsKey(nodeList[i]))
                {
                    Debug.LogError("Does Not Contain child Node: " + nodeList[i]);
                    return;
                }

                node = node.rdChildrenDic[nodeList[i]];

                if (i == nodeList.Length - 1)
                {
                    node.countChangeFunc = callBack;
                    return;
                }
            }
        }

        /// <summary>
        /// 设置红点参数
        /// </summary>
        /// <param name="nodePath">红点路径，必须走 RedDotDefine </param>
        /// <param name="rdCount">红点计数</param>
        public void Set(string nodePath, int rdCount = 1)
        {
            string[] nodeList = nodePath.Split('/');

            if (nodeList.Length == 1)
            {
                if (nodeList[0] != E_RedDotDefine.rdRoot)
                {
                    Debug.Log("Get Wrong RootNod！ current is " + nodeList[0]);
                    return;
                }
            }

            //遍历子红点
            RedDotNode node = mRootNode;
            for (int i = 1; i < nodeList.Length; i++)
            {
                //父红点的 子红点字典表 内，必须包含
                if (node.rdChildrenDic.ContainsKey(nodeList[i]))
                {
                    node = node.rdChildrenDic[nodeList[i]];

                    //设置叶子红点的红点数
                    if (i == nodeList.Length - 1)
                    {
                        node.SetRedDotCount(Math.Max(0, rdCount));
                    }
                }
                else
                {
                    Debug.LogError($"{node.rdName}的子红点字典内无 Key={nodeList[i]}, 检查 RedDotSystem.InitRedDotTreeNode()");
                    return;
                }
            }
        }

        /// <summary>
        /// 获取红点的计数
        /// </summary>
        /// <param name="nodePath"></param>
        /// <returns></returns>
        public int GetRedDotCount(string nodePath)
        {
            string[] nodeList = nodePath.Split('/');

            int count = 0;
            if (nodeList.Length >= 1)
            {
                //遍历子红点
                RedDotNode node = mRootNode;
                for (int i = 1; i < nodeList.Length; i++)
                {
                    //父红点的 子红点字典表 内，必须包含
                    if (node.rdChildrenDic.ContainsKey(nodeList[i]))
                    {
                        node = node.rdChildrenDic[nodeList[i]];

                        if (i == nodeList.Length - 1)
                        {
                            count = node.rdCount;
                            break;
                        }
                    }
                }
            }

            return count;
        }

        #endregion
    }
}