using UnityEngine;

namespace DSC.Network.Event.Helper
{
    public class DSC_Event_Network : MonoBehaviour
    {
        #region Events

        public virtual void StartNetwork(NetworkMode eMode)
        {
            DSC_Network.StartNetwork(eMode);
        }

        public virtual void StartNetwork(string sMode)
        {
            DSC_Network.StartNetwork(sMode);
        }

        public virtual void StopNetwork(NetworkMode eMode)
        {
            DSC_Network.StopNetwork(eMode);
        }

        public virtual void StopNetwork(string sMode)
        {
            DSC_Network.StopNetwork(sMode);
        }

        #endregion
    }
}
