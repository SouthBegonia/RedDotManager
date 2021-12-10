using UnityEngine;
using UnityEngine.UI;

namespace RedDotTutorial_1
{
    /// <summary>
    /// UGUI红点物体脚本
    /// </summary>
    public class RedDotItem : MonoBehaviour
    {
        [Header("红点父节点")]
        [SerializeField]
        public GameObject m_DotObj;

        [Header("红点数文本")]
        [SerializeField]
        private Text m_DotCountText;

        /// <summary>
        /// 设定红点状态
        /// </summary>
        /// <param name="isShow">是否显示</param>
        /// <param name="dotCount">红点数</param>
        public void SetDotState(bool isShow, int dotCount = -1)
        {
            if (isShow)
            {
                m_DotObj.gameObject.SetActive(true);

                if (m_DotCountText)
                    m_DotCountText.text = dotCount >= 0 ? dotCount.ToString() : "";
            }
            else
            {
                m_DotObj.gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            m_DotObj = null;
            m_DotCountText = null;
        }
    }
}