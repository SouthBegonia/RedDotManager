namespace RedDotTutorial_1
{
    /// <summary>
    /// 游戏管理器
    /// </summary>
    public static class ManagerComponent
    {
        #region 红点管理器

        private static RedDotSystem m_RedDotManager;

        /// <summary>
        /// 红点管理器
        /// </summary>
        public static RedDotSystem RedDotManager
        {
            get
            {
                //通常放在项目的初始化逻辑中，此处只是demo临时写法
                if (m_RedDotManager == null)
                    m_RedDotManager = new RedDotSystem();
                return m_RedDotManager;
            }
        }

        #endregion
    }
}