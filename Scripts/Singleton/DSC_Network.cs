using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSC.Core;
using DSC.Event;
using UnityEngine.Events;
using MLAPI;

namespace DSC.Network
{
    public abstract class DSC_Network : MonoBehaviour
    {
        #region Variable

        #region Variable - Property

        public static bool isServerOnly
        {
            get
            {
                if (!HasBaseInstance())
                    return false;

                return m_hBaseInstance.IsServerOnly();
            }
        }

        protected abstract EventCallback<DSC_NetworkEventType> hostEvent { get; set; }
        protected abstract EventCallback<DSC_NetworkEventType> clientEvent { get; set; }
        protected abstract EventCallback<DSC_NetworkEventType> serverEvent { get; set; }

        protected abstract float? tryConnectTimeoutTime { get; set; }
        protected abstract float tryConnectTimeout { get; set; }

        #endregion

        protected static DSC_Network m_hBaseInstance;

        #endregion

        #region Unity

        protected virtual void Start()
        {
            var hNetManager = NetworkManager.Singleton;

            // Listen to NetworkStart.
            hNetManager.OnServerStarted += NetworkStart;
            hNetManager.OnClientConnectedCallback += (clientId) =>
            {
                if (clientId == hNetManager.LocalClientId)
                {
                    NetworkStart();
                }
            };
        }

        protected virtual void Update()
        {
            if (tryConnectTimeoutTime.HasValue && tryConnectTimeoutTime.Value < Time.realtimeSinceStartup)
            {
                TryConnectTimeout();
            }
        }

        #endregion

        #region Events

        #region Events - Add/Remove

        public static void AddEventListener(DSC_NetworkEventType eEvent, UnityAction hAction)
        {
            if (!HasBaseInstance())
                return;

            m_hBaseInstance.MainAddEventListener((NetworkMode)(-1), eEvent, hAction);
        }

        public static void AddEventListener(DSC_NetworkEventType eEvent, UnityAction hAction, EventOrder eOrder)
        {
            if (!HasBaseInstance())
                return;

            m_hBaseInstance.MainAddEventListener((NetworkMode)(-1), eEvent, hAction, eOrder);
        }

        public static void AddEventListener(NetworkMode eMode, DSC_NetworkEventType eEvent, UnityAction hAction)
        {
            if (!HasBaseInstance())
                return;

            m_hBaseInstance.MainAddEventListener(eMode, eEvent, hAction);
        }

        public static void AddEventListener(NetworkMode eMode, DSC_NetworkEventType eEvent, UnityAction hAction, EventOrder eOrder)
        {
            if (!HasBaseInstance())
                return;

            m_hBaseInstance.MainAddEventListener(eMode, eEvent, hAction, eOrder);
        }

        void MainAddEventListener(NetworkMode eMode, DSC_NetworkEventType eEvent, UnityAction hAction, EventOrder eOrder = EventOrder.Normal)
        {
            if (FlagUtility.HasFlagUnsafe(eMode, NetworkMode.Host))
                hostEvent?.Add(eEvent, hAction, eOrder);

            if (FlagUtility.HasFlagUnsafe(eMode, NetworkMode.Client))
                clientEvent?.Add(eEvent, hAction, eOrder);

            if (FlagUtility.HasFlagUnsafe(eMode, NetworkMode.Server))
                serverEvent?.Add(eEvent, hAction, eOrder);
        }

        public static void RemoveEventListener(DSC_NetworkEventType eEvent, UnityAction hAction)
        {
            if (m_hBaseInstance == null)
                return;

            m_hBaseInstance.MainRemoveEventListener((NetworkMode)(-1), eEvent, hAction);
        }

        public static void RemoveEventListener(DSC_NetworkEventType eEvent, UnityAction hAction, EventOrder eOrder)
        {
            if (m_hBaseInstance == null)
                return;

            m_hBaseInstance.MainRemoveEventListener((NetworkMode)(-1), eEvent, hAction, eOrder);
        }

        public static void RemoveEventListener(NetworkMode eMode, DSC_NetworkEventType eEvent, UnityAction hAction)
        {
            if (m_hBaseInstance == null)
                return;

            m_hBaseInstance.MainRemoveEventListener(eMode, eEvent, hAction);
        }

        public static void RemoveEventListener(NetworkMode eMode, DSC_NetworkEventType eEvent, UnityAction hAction, EventOrder eOrder)
        {
            if (m_hBaseInstance == null)
                return;

            m_hBaseInstance.MainRemoveEventListener(eMode, eEvent, hAction, eOrder);
        }

        void MainRemoveEventListener(NetworkMode eMode, DSC_NetworkEventType eEvent, UnityAction hAction, EventOrder eOrder = EventOrder.Normal)
        {
            if (FlagUtility.HasFlagUnsafe(eMode, NetworkMode.Host))
                hostEvent?.Remove(eEvent, hAction, eOrder);

            if (FlagUtility.HasFlagUnsafe(eMode, NetworkMode.Client))
                clientEvent?.Remove(eEvent, hAction, eOrder);

            if (FlagUtility.HasFlagUnsafe(eMode, NetworkMode.Server))
                serverEvent?.Remove(eEvent, hAction, eOrder);
        }

        #endregion

        #endregion

        #region Main

        protected virtual void NetworkStart()
        {
            var hNetManager = NetworkManager.Singleton;

            tryConnectTimeoutTime = null;

            if (hNetManager.IsHost)
            {
                hostEvent?.Run(DSC_NetworkEventType.NetworkStart);
            }
            else if (hNetManager.IsClient)
            {
                RegisterClientMessageHandlers();
                clientEvent?.Run(DSC_NetworkEventType.NetworkStart);
            }
            else if (hNetManager.IsServer)
            {
                RegisterServerMessageHandlers();
                serverEvent?.Run(DSC_NetworkEventType.NetworkStart);
            }
        }

        protected abstract void RegisterServerMessageHandlers();
        protected abstract void RegisterClientMessageHandlers();

        public static void StartNetwork(NetworkMode eMode)
        {
            if (!HasBaseInstance())
                return;

            m_hBaseInstance.MainStartNetwork(eMode);
        }

        public static void StartNetwork(string sMode)
        {
            if (TryGetNetworkModeByString(sMode, out NetworkMode eMode))
                StartNetwork(eMode);
        }

        protected virtual void MainStartNetwork(NetworkMode eMode)
        {
            var hNetworkManager = NetworkManager.Singleton;

            switch (eMode)
            {
                case NetworkMode.Host:
                    hNetworkManager.StartHost();
                    hostEvent?.Run(DSC_NetworkEventType.StartNetwork);
                    break;

                case NetworkMode.Client:
                    tryConnectTimeoutTime = Time.realtimeSinceStartup + tryConnectTimeout;
                    hNetworkManager.StartClient();
                    clientEvent?.Run(DSC_NetworkEventType.StartNetwork);
                    break;

                case NetworkMode.Server:
                    hNetworkManager.StartServer();
                    serverEvent?.Run(DSC_NetworkEventType.StartNetwork);
                    break;
            }


        }

        public static void StopNetwork()
        {
            if (m_hBaseInstance == null)
                return;

            m_hBaseInstance.MainStopNetwork();
        }

        protected virtual void MainStopNetwork()
        {
            var hNetworkManager = NetworkManager.Singleton;

            tryConnectTimeoutTime = null;

            if (hNetworkManager.IsHost)
            {
                hNetworkManager.StopHost();
                hostEvent?.Run(DSC_NetworkEventType.StopNetwork);
            }
            else if (hNetworkManager.IsClient)
            {
                hNetworkManager.StopClient();
                clientEvent?.Run(DSC_NetworkEventType.StopNetwork);
            }
            else if (hNetworkManager.IsServer)
            {
                hNetworkManager.StopServer();
                serverEvent?.Run(DSC_NetworkEventType.StopNetwork);
            }


        }

        protected abstract bool IsServerOnly();

        protected virtual void TryConnectTimeout()
        {
            tryConnectTimeoutTime = null;
            clientEvent?.Run(DSC_NetworkEventType.TryConnectTimeout);
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

        protected static bool TryGetNetworkModeByString(string sValue, out NetworkMode eOutMode)
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
