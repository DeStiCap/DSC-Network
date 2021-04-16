using UnityEngine;
using UnityEngine.Events;

namespace DSC.Network.Event
{
    public class DSC_Event_NetworkMode : MonoBehaviour
    {
        #region Variable

        #region Variable - Inspector

        [SerializeField] protected NetworkMode m_eMode;
        [SerializeField] protected UnityEvent<NetworkMode> m_evtEvent;

        #endregion

        #endregion

        #region Events

        public void SetNetworkMode(NetworkMode eMode)
        {
            m_eMode = eMode;
        }

        public void RunEvent()
        {
            m_evtEvent?.Invoke(m_eMode);
        }

        public void RunEvent(NetworkMode eMode)
        {
            m_evtEvent?.Invoke(eMode);
        }

        #endregion
    }
}
