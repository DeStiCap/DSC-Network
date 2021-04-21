using UnityEngine;
using UnityEngine.Events;
using DSC.Core;
using DSC.Event;

namespace DSC.Network.Event.Helper
{
    public class DSC_Event_RunEventsByNetworkEvent : MonoBehaviour
    {
        #region Variable

        #region Variable - Inspector

#if UNITY_EDITOR
        [TextField("Events Description", 3)]
        [SerializeField] protected string m_sEventsDescription;

#endif

        [Header("Condition")]
        [EnumMask]
        [SerializeField] protected NetworkMode m_eNetworkMode;
        [EnumMask]
        [SerializeField] protected DSC_NetworkEventType m_eEventType;
        [SerializeField] protected EventCondition[] m_arrCondition;


        [Header("Events")]
        [SerializeField] protected UnityEvent m_hEvent;

        #endregion

        protected NetworkMode? m_ePreviousNetworkMode;
        protected DSC_NetworkEventType? m_ePreviousEventType;

        protected UnityAction m_hRunEvents;

        protected EventConditionData m_hConditionData;

        #endregion

        #region Mono

        protected virtual void Awake()
        {
            m_hConditionData = new EventConditionData(transform);

            m_ePreviousNetworkMode = m_eNetworkMode;
            m_ePreviousEventType = m_eEventType;
            m_hRunEvents = RunEvents;
        }

        protected virtual void OnValidate()
        {
            if ((m_ePreviousNetworkMode != null && m_ePreviousNetworkMode != m_eNetworkMode)
                || (m_ePreviousEventType != null && m_ePreviousEventType != m_eEventType))
            {
                RemoveNetworkEventListener(m_ePreviousNetworkMode.Value, m_ePreviousEventType.Value);
                AddNetworkEventListener(m_eNetworkMode, m_eEventType);
                m_ePreviousNetworkMode = m_eNetworkMode;
                m_ePreviousEventType = m_eEventType;
            }
        }

        protected virtual void OnEnable()
        {
            AddNetworkEventListener(m_eNetworkMode, m_eEventType);
        }

        protected virtual void OnDisable()
        {
            RemoveNetworkEventListener(m_eNetworkMode, m_eEventType);
        }

        #endregion

        #region Events

        protected void AddNetworkEventListener(NetworkMode eMode, DSC_NetworkEventType eEvent)
        {
            DSC_Network.AddEventListener(eMode, eEvent, m_hRunEvents);
        }

        protected void RemoveNetworkEventListener(NetworkMode eMode, DSC_NetworkEventType eEvent)
        {
            DSC_Network.RemoveEventListener(eMode, eEvent, m_hRunEvents);
        }

        protected virtual void RunEvents()
        {
            if (!IsPassCondition())
                return;

            m_hEvent?.Invoke();
        }

        #endregion

        #region Helper

        protected bool IsPassCondition()
        {
            return m_arrCondition.PassAllCondition(m_hConditionData);
        }

        #endregion
    }
}
