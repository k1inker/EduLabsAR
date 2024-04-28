using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.EventSystems;
using Camera = UnityEngine.Camera;

namespace EduLab
{
    public class CWire : MonoBehaviourPunCallbacks, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private LineRenderer line;
        [SerializeField] private CConnector currentConnector;

        private PointerEventData currentEventData;
        private bool isClicked;
        private Vector3 offSet;
        private Camera camera;
        private string interactingUserId;
        private void Awake()
        {
            this.camera = Camera.main;
            this.currentConnector.OnConnect += OnConnectWire;
            this.currentConnector.OnDisconnect += OnDisconnectWire;
        }

        private void OnDestroy()
        {
            this.currentConnector.OnConnect -= OnConnectWire;
            this.currentConnector.OnDisconnect -= OnDisconnectWire;
        }

        //===================//
        // UNITY METHODS
        //===================//
        public void OnPointerDown(PointerEventData eventData)
        {
            if (this.isClicked)
            {
                return;
            }
            
            Vector3 position = this.transform.position;
            this.offSet = position - this.TouchWorldPoint(eventData.position);
            this.currentEventData = eventData;
            this.photonView.RPC(nameof(this.OnPointerDownRPC), RpcTarget.All, position, PhotonNetwork.LocalPlayer.UserId);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!this.isClicked)
            {
                return;
            }
            
            bool isConnectedSuccessful = this.RaycastCameraToPoint();
            this.photonView.RPC(nameof(this.OnPointerUpRPC), RpcTarget.All, isConnectedSuccessful);
            this.currentEventData = null;
        }
        private void FixedUpdate()
        {
            if (this.isClicked && PhotonNetwork.LocalPlayer.UserId.Equals(this.interactingUserId))
            {
                this.photonView.RPC(nameof(this.OnUpdatePositionRPC), RpcTarget.All, 
                    this.TouchWorldPoint(this.currentEventData.position) + offSet);
            }
        }
        
        //===================//
        // RPC METHODS
        //===================//
        [PunRPC]
        private void OnPointerDownRPC(Vector3 startPosition, string idUser)
        {
            this.currentConnector.Disconect();
            this.interactingUserId = idUser;
            
            this.line.SetPosition(0, startPosition);
            this.isClicked = true;
        }
        [PunRPC]
        private void OnPointerUpRPC(bool isConnectedSuccessful)
        {
            this.isClicked = false;
            this.interactingUserId = string.Empty;
            if (!isConnectedSuccessful)
            {
                this.currentConnector.Disconect();
            }
        }
        [PunRPC]
        private void OnUpdatePositionRPC(Vector3 endPosition)
        {
            this.line.SetPosition(1, endPosition);
        }
        [PunRPC]
        private void ConnectRPC(Vector3 endPosition)
        {
            this.line.SetPosition(0, this.transform.position);
            this.line.SetPosition(1, endPosition);
        }
        [PunRPC]
        private void DisconnectRPC()
        {
            this.line.SetPosition(0, this.transform.position);
            this.line.SetPosition(1, this.transform.position);
        }

        [PunRPC]
        private void TryConnectWire(int connectorViewId)
        {
            PhotonView targetView = PhotonView.Find(connectorViewId);
            if (targetView == null)
            {
                this.currentConnector.Disconect();
                return;
            }
            
            CConnector connectionConnector = targetView.GetComponent<CConnector>();
            
            if (!this.currentConnector.TryConnect(connectionConnector))
            {
                this.currentConnector.Disconect();
            }
        }
        //===================//
        // PRIVATE METHODS
        //===================//
        private void OnConnectWire(CConnector connector)
        {
            this.photonView.RPC(nameof(this.ConnectRPC), RpcTarget.All, connector.transform.position);
        }
        private void OnDisconnectWire()
        {
            this.photonView.RPC(nameof(this.DisconnectRPC), RpcTarget.All);
        }
        
        private bool RaycastCameraToPoint()
        {
            Vector3 position = this.camera.transform.position;
            Vector3 rayOrigin = position;
            Vector3 rayDir = TouchWorldPoint(this.currentEventData.position) - position;

            if (Physics.Raycast(rayOrigin, rayDir, out RaycastHit hitInfo))
            {
                PhotonView photonViewConnector = hitInfo.transform.GetComponent<PhotonView>();

                if (photonViewConnector != null)
                {
                    this.photonView.RPC(nameof(this.TryConnectWire), RpcTarget.All, photonViewConnector.ViewID);
                    return true;
                }
            }

            return false;
        }
        
        private Vector3 TouchWorldPoint(Vector3 touchPoint)
        {
            touchPoint.z = camera.WorldToScreenPoint(this.transform.position).z;
            return camera.ScreenToWorldPoint(touchPoint);
        }
        //===================//
        // PUBLIC METHODS
        //===================//
    }
}