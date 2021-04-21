## [0.0.8-preview.1] - 2021-04-21
- Change DSC_NetworkEventType to flag type.
- DSC_Network event callback now support use flag as event type.

## [0.0.7-preview.1] - 2021-04-19
- Move StartNetwork ipaddress,port and transporter method in Global_NetworkManager to DSC_Network.
- Move all method in Event_NetworkManager to DSC_Event_Network.
- Remove Event_NetworkManager in NetworkTemplate Sample.

## [0.0.6-preview.1] - 2021-04-18
- DSC_Event_RunEventsByNetworkEvent now have events description text field.

## [0.0.5-preview.1] - 2021-04-17
- Add DSC_EventCondition_IsServerOnly script.
- DSC_Event_RunEventsByNetworkEvent now support EventCondition.

## [0.0.4-preview.1] - 2021-04-16
- Add ConnectStatus enum in NetworkTemplate sample.
- Global_NetworkManager support to set ip address, port and Transporter in StartNetwork method.
- DSC_Network support NetworkStart and TryConnectTimeout event callback.
- DSC_Network will stop network after start network as client but can't connect to server.

## [0.0.3-preview.3] - 2021-04-03
- Change DSC_Network to derived from NetworkBehaviour instead MonoBehaviour
- Change StopNetwork method in DSC_Network not receive argument anymore.

## [0.0.2-preview.2] - 2021-04-02
- Change DSC_Event_RunEventsByNetworkEvent add/remove event in OnValidate to run only during game playing.

## [0.0.1-preview.1] - 2021-03-30
- Add DSC_NetworkEventType enum.
- Add DSC_Network script.
- Add DSC_Event_Network script.
- Add DSC_Event_NetworkMode script.
- Add DSC_Event_RunEventsByNetworkEvent script.
- Add NetworkTemplate sample.

## [0.0.0-preview.5] - 2021-03-25
- Add NetworkMode enum.
- Add DSC_Network_HUD script.
- Add DSC_Event_NetworkMode script.