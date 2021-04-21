using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSC.Network;
using DSC.Event;
using UnityEngine.Events;
using MLAPI;
using MLAPI.Messaging;
using MLAPI.Serialization.Pooled;
using MLAPI.Transports;

namespace DSC.Template
{
    public sealed class Global_NetworkManager : DSC_Network
    {

        #region Variable

        #region Variable - Inspector

        [SerializeField] bool m_bIsServerOnly;

        #endregion

        #region Variable - Property

        static Global_NetworkManager instance
        {
            get
            {
                if (m_hInstance == null && m_bAppStart && !m_bAppQuit)
                    Debug.LogWarning("Don't have Global_NetworkManager in scene.");

                return m_hInstance;
            }
        }

        protected override EventFlagCallback<DSC_NetworkEventType> hostEvent { get { return m_hHostEvent; } set { m_hHostEvent = value; } }
        protected override EventFlagCallback<DSC_NetworkEventType> clientEvent { get { return m_hClientEvent; } set { m_hClientEvent = value; } }
        protected override EventFlagCallback<DSC_NetworkEventType> serverEvent { get { return m_hServerEvent; } set { m_hServerEvent = value; } }

        public static event System.Action<ConnectStatus> connectFinishCallback
        {
            add
            {
                if (instance == null)
                    return;

                m_hInstance.m_hConnectFinishCallback += value;
            }

            remove
            {
                if (m_hInstance == null)
                    return;

                m_hInstance.m_hConnectFinishCallback -= value;
            }

        }

        #endregion

        static Global_NetworkManager m_hInstance;
        static bool m_bAppStart;
        static bool m_bAppQuit;

        EventFlagCallback<DSC_NetworkEventType> m_hHostEvent = new EventFlagCallback<DSC_NetworkEventType>();
        EventFlagCallback<DSC_NetworkEventType> m_hClientEvent = new EventFlagCallback<DSC_NetworkEventType>();
        EventFlagCallback<DSC_NetworkEventType> m_hServerEvent = new EventFlagCallback<DSC_NetworkEventType>();


        System.Action<ConnectStatus> m_hConnectFinishCallback;

        #endregion

        #region Unity

        void Awake()
        {
            if (m_hInstance == null)
            {
                m_hBaseInstance = this;
                m_hInstance = this;
            }
            else if (m_hInstance != this)
            {
                Destroy(gameObject);
                return;
            }

            Application.quitting += OnAppQuit;
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnAppStart()
        {
            m_bAppStart = true;
            m_bAppQuit = false;
        }

        void OnAppQuit()
        {
            Application.quitting -= OnAppQuit;

            m_bAppStart = false;
            m_bAppQuit = true;
        }

        #endregion

        #region Main

        protected override void NetworkStart()
        {
            if (NetworkManager.Singleton.IsHost)
            {
                m_hConnectFinishCallback?.Invoke(ConnectStatus.Success);
            }

            base.NetworkStart();
        }

        protected override void RegisterServerMessageHandlers()
        {

        }

        protected override void RegisterClientMessageHandlers()
        {
            CustomMessagingManager.RegisterNamedMessageHandler(NetMessageName.s2c_ConnectResult, (ulSenderClientId, hStream) =>
            {
                using (var hReader = PooledNetworkReader.Get(hStream))
                {
                    ConnectStatus eStatus = (ConnectStatus)hReader.ReadInt32();

                    m_hConnectFinishCallback?.Invoke(eStatus);
                }
            });
        }

        #region Main - Network Start

        protected override bool IsServerOnly()
        {
            return m_bIsServerOnly;
        }

        protected override void SetTransportData(string sIpAddress, int nPort = -1, NetworkTransport hTransport = null)
        {
            var hNetworkManager = NetworkManager.Singleton;

            if (hTransport == null)
                hTransport = hNetworkManager.NetworkConfig.NetworkTransport;
            else
                hNetworkManager.NetworkConfig.NetworkTransport = hTransport;

            switch (hTransport)
            {
                case MLAPI.Transports.UNET.UNetTransport hUnetTransport:
                    if (!string.IsNullOrEmpty(sIpAddress))
                        hUnetTransport.ConnectAddress = sIpAddress;
                    if (nPort > 0)
                        hUnetTransport.ServerListenPort = nPort;
                    break;
                default:
                    throw new System.Exception($"unhandled IpHost transport {hTransport.GetType()}");
            }
        }

        #endregion

        #endregion
    }
}