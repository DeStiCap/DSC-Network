using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSC.Event;

namespace DSC.Network.Event.Condition
{
    [CreateAssetMenu(fileName = "EventCondition_IsServerOnly", menuName = "DSC/Network/Events/Condition/Is Server Only")]
    public class DSC_EventCondition_IsServerOnly : EventCondition
    {
        #region Variable - Inspector

        [SerializeField] bool m_bIsServerOnly;

        #endregion

        public override bool PassCondition(EventConditionData hData)
        {
            return DSC_Network.isServerOnly == m_bIsServerOnly;
        }
    }
}
