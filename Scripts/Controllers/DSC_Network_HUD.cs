using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAPI;
using UnityEngine.Events;

namespace DSC.Network
{
    public sealed class DSC_Network_HUD : MonoBehaviour
    {
        #region Data

        [System.Serializable]
        struct HUDData
        {
            public float m_fHorizontalOffSet;
            public float m_fVerticalOffset;
            public float m_fWidth;
            public float m_fHeight;
        }

        #endregion

        #region Variable

        #region Variable - Inspector

        [SerializeField] HUDData m_hSizeData = new HUDData { m_fHorizontalOffSet = 10, m_fVerticalOffset = 10, m_fWidth = 300, m_fHeight = 300 };
        [SerializeField] bool m_bShowGUI = true;

        [Header("Event")]
        [SerializeField] UnityEvent m_evtStart;

        #endregion

        #endregion

        #region Mono

        void OnGUI()
        {
            if (!m_bShowGUI)
                return;

            GUILayout.BeginArea(
                new Rect(m_hSizeData.m_fHorizontalOffSet
                , m_hSizeData.m_fVerticalOffset
                , m_hSizeData.m_fWidth, m_hSizeData.m_fHeight));

            if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
            {
                StartButtons();
            }
            else
            {
                StatusLabels();
            }

            GUILayout.EndArea();
        }

        #endregion

        #region Events

        public void ClickNetworkButton(NetworkMode eMode)
        {
            m_evtStart?.Invoke();

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
        }

        #endregion

        #region Main

        void StartButtons()
        {
            if (GUILayout.Button("Host")) ClickNetworkButton(NetworkMode.Host);
            if (GUILayout.Button("Client")) ClickNetworkButton(NetworkMode.Client);
            if (GUILayout.Button("Server")) ClickNetworkButton(NetworkMode.Server);
        }

        void StatusLabels()
        {
            var mode = NetworkManager.Singleton.IsHost ?
                "Host" : NetworkManager.Singleton.IsServer ? "Server" : "Client";

            GUILayout.Label("Transport: " +
                NetworkManager.Singleton.NetworkConfig.NetworkTransport.GetType().Name);
            GUILayout.Label("Mode: " + mode);
        }

        #endregion

        #region Helper



        #endregion
    }
}
