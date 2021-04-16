using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSC.Network;
using DSC.Network.Event;
using MLAPI.Transports;

namespace DSC.Template
{
    public class Event_NetworkManager : DSC_Event_Network
    {
        #region Variable

        #region Variable - Inspector

        [SerializeField] string m_sIpAdress;
        [SerializeField] int m_nPort;
        [SerializeField] NetworkTransport m_hTransport;

        #endregion

        #endregion

        #region Events

        public override void StartNetwork(NetworkMode eMode)
        {
            Global_NetworkManager.StartNetwork(eMode, m_sIpAdress, m_nPort, m_hTransport);
        }

        public override void StartNetwork(string sMode)
        {
            Global_NetworkManager.StartNetwork(sMode, m_sIpAdress, m_nPort, m_hTransport);
        }

        #endregion
    }
}