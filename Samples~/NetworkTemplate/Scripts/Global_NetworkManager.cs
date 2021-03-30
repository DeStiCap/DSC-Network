using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DSC.Network;
using DSC.Event;

namespace DSC.Template
{
    public sealed class Global_NetworkManager : DSC_Network
    {

        #region Variable

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

        protected override EventCallback<DSC_NetworkEventType> networkEvent { get { return m_evtNetworkEvent; } set { m_evtNetworkEvent = value; } }

        #endregion

        static Global_NetworkManager m_hInstance;
        static bool m_bAppStart;
        static bool m_bAppQuit;

        EventCallback<DSC_NetworkEventType> m_evtNetworkEvent = new EventCallback<DSC_NetworkEventType>();

        #endregion

        #region Mono

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

            DontDestroyOnLoad(gameObject);
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
    }
}