using UnityEngine;
using UnityEngine.Events;

namespace DSC.Network.Event.Helper
{
    public class DSC_Event_RunEventsByNetworkEvent : MonoBehaviour
    {
        #region Variable

        #region Variable - Inspector

        [SerializeField] protected DSC_NetworkEventType m_eEventType;
        [SerializeField] protected UnityEvent m_evtEvent;

        #endregion

        protected DSC_NetworkEventType? m_ePreviousEventType;

        protected UnityAction m_actRunEvents;

        #endregion

        #region Mono

        protected virtual void Awake()
        {
            m_ePreviousEventType = m_eEventType;
            m_actRunEvents = RunEvents;
        }

        protected virtual void OnValidate()
        {
            if (m_ePreviousEventType != null && m_ePreviousEventType != m_eEventType)
            {
                RemoveNetworkEventListener(m_ePreviousEventType.Value);
                AddNetworkEventListener(m_eEventType);
                m_ePreviousEventType = m_eEventType;

            }
        }

        protected virtual void OnEnable()
        {
            AddNetworkEventListener(m_eEventType);
        }

        protected virtual void OnDisable()
        {
            RemoveNetworkEventListener(m_eEventType);
        }

        #endregion

        #region Events

        protected void AddNetworkEventListener(DSC_NetworkEventType eEvent)
        {
            DSC_Network.AddEventListener(eEvent, m_actRunEvents);
        }

        protected void RemoveNetworkEventListener(DSC_NetworkEventType eEvent)
        {
            DSC_Network.RemoveEventListener(eEvent, m_actRunEvents);
        }

        protected virtual void RunEvents()
        {
            m_evtEvent?.Invoke();
        }

        #endregion
    }
}
