using System;
using System.Collections.Generic;
using UnityEngine;

namespace RedDotTutorial_1
{
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
        /// 红点变化通知委托
        /// </summary>
        /// <param name="node"></param>
        public delegate void OnRdCountChange(RedDotNode node);

        /// <summary>
        /// 红点树的的 Root节点（即Main节点）
        /// </summary>
        private RedDotNode mRootNode;

        /// <summary>
        /// 初始化红点树
        /// </summary>
        private static List<string> lstRedDotTreeList = new List<string>
        {
            RedDotDefine.rdRoot,

            RedDotDefine.MailBox,
            RedDotDefine.MailBox_System,
            RedDotDefine.MailBox_Team,
        };


        #region 内部接口

        /// <summary>
        /// 初始化红点树
        /// </summary>
        private void InitRedDotTreeNode()
        {
            mRootNode = new RedDotNode {rdName = RedDotDefine.rdRoot};

            foreach (string path in lstRedDotTreeList)
            {
                string[] treeNodeAy = path.Split('/');
                int nodeCount = treeNodeAy.Length;
                RedDotNode curNode = mRootNode;

                if (treeNodeAy[0] != mRootNode.rdName)
                {
                    Debug.LogError("根节点必须为Main，检查 " + treeNodeAy[0]);
                    continue;
                }

                if (nodeCount > 1)
                {
                    for (int i = 1; i < nodeCount; i++)
                    {
                        if (!curNode.RdChildrenDic.ContainsKey(treeNodeAy[i]))
                        {
                            curNode.RdChildrenDic.Add(treeNodeAy[i], new RedDotNode());
                        }

                        curNode.RdChildrenDic[treeNodeAy[i]].rdName = treeNodeAy[i];
                        curNode.RdChildrenDic[treeNodeAy[i]].parent = curNode;

                        curNode = curNode.RdChildrenDic[treeNodeAy[i]];
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
                if (nodeList[0] != RedDotDefine.rdRoot)
                {
                    Debug.LogError("Get Wrong Root Node! current is " + nodeList[0]);
                    return;
                }
            }

            var node = mRootNode;
            for (int i = 1; i < nodeList.Length; i++)
            {
                if (!node.RdChildrenDic.ContainsKey(nodeList[i]))
                {
                    Debug.LogError("Does Not Contain child Node: " + nodeList[i]);
                    return;
                }

                node = node.RdChildrenDic[nodeList[i]];

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
                if (nodeList[0] != RedDotDefine.rdRoot)
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
                if (node.RdChildrenDic.ContainsKey(nodeList[i]))
                {
                    node = node.RdChildrenDic[nodeList[i]];

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
                    if (node.RdChildrenDic.ContainsKey(nodeList[i]))
                    {
                        node = node.RdChildrenDic[nodeList[i]];

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