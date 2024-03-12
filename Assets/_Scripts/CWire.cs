using Photon.Pun;
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

        private void Awake()
        {
            this.camera = Camera.main;
            this.currentConnector.OnConnect += (connector) => this.photonView.RPC(nameof(this.ConnectRPC), RpcTarget.All, connector.transform.position);
            this.currentConnector.OnDisconnect += () => this.photonView.RPC(nameof(this.DisconnectRPC), RpcTarget.All);
        }

        private void OnDisable()
        {
            this.currentConnector.OnConnect -= (connector) => this.photonView.RPC(nameof(this.DisconnectRPC), RpcTarget.All, connector.transform.position);
            this.currentConnector.OnDisconnect -= () => this.photonView.RPC(nameof(this.DisconnectRPC), RpcTarget.All);
        }

        //////////////////
        /// UNITY METHODS
        //////////////////
        public void OnPointerDown(PointerEventData eventData)
        {
            if (this.isClicked)
            {
                return;
            }
            
            Vector3 position = this.transform.position;
            this.offSet = position - this.TouchWorldPoint(eventData.position);
            this.currentEventData = eventData;
            this.photonView.RPC(nameof(this.OnPointerDownRPC), RpcTarget.All, position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!this.isClicked)
            {
                return;
            }
            
            this.TryConnectWire();
            this.currentEventData = null;
            this.photonView.RPC(nameof(this.OnPointerUpRPC), RpcTarget.All);
        }
        private void Update()
        {
            if (this.isClicked && this.photonView.IsOwnerActive)
            {
                this.photonView.RPC(nameof(this.OnUpdatePositionRPC), RpcTarget.All, 
                    this.TouchWorldPoint(this.currentEventData.position) + offSet);
            }
        }
        
        /////////////////////
        /// NETWORK METHODS
        /////////////////////
        [PunRPC]
        private void OnPointerDownRPC(Vector3 startPosition)
        {
            this.currentConnector.Disconect();
            
            this.line.SetPosition(0, startPosition);
            this.isClicked = true;
        }
        [PunRPC]
        private void OnPointerUpRPC()
        {
            this.isClicked = false;
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
        /////////////////////
        /// PRIVATE METHODS
        /////////////////////
        private void TryConnectWire()
        {
            Vector3 position = this.camera.transform.position;
            Vector3 rayOrigin = position;
            Vector3 rayDir = TouchWorldPoint(this.currentEventData.position) - position;

            if (Physics.Raycast(rayOrigin, rayDir, out RaycastHit hitInfo))
            {
                CConnector connectionConnector = hitInfo.transform.GetComponent<CConnector>();

                if (this.currentConnector.TryConnect(connectionConnector))
                {
                    return;
                }
            }

            this.currentConnector.Disconect();
        }

        private Vector3 TouchWorldPoint(Vector3 touchPoint)
        {
            touchPoint.z = camera.WorldToScreenPoint(this.transform.position).z;
            return camera.ScreenToWorldPoint(touchPoint);
        }
        ////////////////////
        /// PUBLIC METHODS
        ////////////////////
    }
}