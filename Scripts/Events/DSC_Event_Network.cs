using MLAPI.Transports;
using UnityEngine;

namespace DSC.Network.Event
{
    public class DSC_Event_Network : MonoBehaviour
    {
        #region Variable

        #region Variable - Inspector

        [SerializeField] protected string m_sIpAdress;
        [SerializeField] protected int m_nPort;
        [SerializeField] protected NetworkTransport m_hTransport;

        #endregion

        #endregion

        #region Events

        public virtual void StartNetwork(NetworkMode eMode)
        {
            DSC_Network.StartNetwork(eMode, m_sIpAdress, m_nPort, m_hTransport);
        }

        public virtual void StartNetwork(string sMode)
        {
            DSC_Network.StartNetwork(sMode, m_sIpAdress, m_nPort, m_hTransport);
        }

        public virtual void StopNetwork()
        {
            DSC_Network.StopNetwork();
        }

        public virtual void SetIpAddress(string sIpAddress)
        {
            m_sIpAdress = sIpAddress;
        }

        public virtual void SetPort(int nPort)
        {
            m_nPort = nPort;
        }

        public virtual void SetPort(string sPort)
        {
            int.TryParse(sPort,out m_nPort);
        }

        #endregion
    }
}
