using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSC.Event;
using UnityEngine.Events;
using MLAPI;

namespace DSC.Network
{
    public abstract class DSC_Network : MonoBehaviour
    {
        #region Variable

        #region Variable - Property

        protected abstract EventCallback<DSC_NetworkEventType> networkEvent { get; set; }

        #endregion

        protected static DSC_Network m_hBaseInstance;

        #endregion

        #region Events

        #region Events - Add/Remove

        public static void AddEventListener(DSC_NetworkEventType eEvent, UnityAction hAction)
        {
            if (!HasBaseInstance())
                return;

            m_hBaseInstance.MainAddEventListener(eEvent, hAction);
        }

        public static void AddEventListener(DSC_NetworkEventType eEvent, UnityAction hAction, EventOrder eOrder)
        {
            if (!HasBaseInstance())
                return;

            m_hBaseInstance.MainAddEventListener(eEvent, hAction, eOrder);
        }

        void MainAddEventListener(DSC_NetworkEventType eEvent, UnityAction hAction, EventOrder eOrder = EventOrder.Normal)
        {
            networkEvent?.Add(eEvent, hAction, eOrder);
        }

        public static void RemoveEventListener(DSC_NetworkEventType eEvent, UnityAction hAction)
        {
            if (m_hBaseInstance == null)
                return;

            m_hBaseInstance.MainRemoveEventListener(eEvent, hAction);
        }

        public static void RemoveEventListener(DSC_NetworkEventType eEvent, UnityAction hAction, EventOrder eOrder)
        {
            if (m_hBaseInstance == null)
                return;

            m_hBaseInstance.MainRemoveEventListener(eEvent, hAction, eOrder);
        }

        void MainRemoveEventListener(DSC_NetworkEventType eEvent, UnityAction hAction, EventOrder eOrder = EventOrder.Normal)
        {
            networkEvent?.Remove(eEvent, hAction, eOrder);
        }

        #endregion

        #endregion

        #region Main

        public static void StartNetwork(NetworkMode eMode)
        {
            if (!HasBaseInstance())
                return;

            m_hBaseInstance.networkEvent?.Run(DSC_NetworkEventType.PreStartNetwork);

            switch (eMode)
            {
                case NetworkMode.Host:
                    NetworkManager.Singleton.StartHost();
                    break;

                case NetworkMode.Client:
                    NetworkManager.Singleton.StartClient();
                    break;

                case NetworkMode.Server:
                    NetworkManager.Singleton.StartServer();
                    break;
            }

            m_hBaseInstance.networkEvent?.Run(DSC_NetworkEventType.PostStartNetwork);
        }

        public static void StartNetwork(string sMode)
        {
            if (TryGetNetworkModeByString(sMode, out NetworkMode eMode))
                 StartNetwork(eMode);
        }

        public static void StopNetwork(NetworkMode eMode)
        {
            if (m_hBaseInstance == null)
                return;

            m_hBaseInstance.networkEvent?.Run(DSC_NetworkEventType.PreStopNetwork);

            switch (eMode)
            {
                case NetworkMode.Host:
                    NetworkManager.Singleton.StopHost();
                    break;

                case NetworkMode.Client:
                    NetworkManager.Singleton.StopClient();
                    break;

                case NetworkMode.Server:
                    NetworkManager.Singleton.StopServer();
                    break;
            }

            m_hBaseInstance.networkEvent?.Run(DSC_NetworkEventType.PostStopNetwork);
        }

        public static void StopNetwork(string sMode)
        {
            if(TryGetNetworkModeByString(sMode,out NetworkMode eMode))
                StopNetwork(eMode);
        }

        #endregion

        #region Helper

        static bool HasBaseInstance()
        {
            bool bResult = m_hBaseInstance != null;

            if (!bResult)
            {
                Debug.LogWarning("Don't have DSC_Network derived class in scene.");
            }

            return bResult;
        }

        static bool TryGetNetworkModeByString(string sValue, out NetworkMode eOutMode)
        {
            bool bResult = true;
            eOutMode = NetworkMode.Host;

            switch (sValue.ToLower())
            {
                case "host":
                    break;

                case "client":
                    eOutMode = NetworkMode.Client;
                    break;

                case "server":
                    eOutMode = NetworkMode.Server;
                    break;

                default:
                    bResult = false;
                    break;

            }

            return bResult;
        }

        #endregion
    }
}
