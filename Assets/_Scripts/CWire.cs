using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Camera = UnityEngine.Camera;

public class CWire : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
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
        this.currentConnector.OnConnect += Connect;
        this.currentConnector.OnDisconnect += Disconnect;
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

        this.currentConnector.Disconect();
        
        Vector3 position = this.transform.position;
        this.offSet = position - this.TouchWorldPoint(eventData.position);
        this.isClicked = true;
        this.currentEventData = eventData;
        this.line.SetPosition(0, position);
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!this.isClicked)
        {
            return;
        }

        TryConnectWire(); 
        
        this.currentEventData = null;
        this.isClicked = false;
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

            if(this.currentConnector.TryConnect(connectionConnector))
            {
                return;
            }
        }

        this.currentConnector.Disconect();
    }

    private void Update()
    {
        if (this.isClicked)
        {
            this.line.SetPosition(1, this.TouchWorldPoint(currentEventData.position) + offSet);
        }
    }
    private Vector3 TouchWorldPoint(Vector3 touchPoint)
    {
        touchPoint.z = camera.WorldToScreenPoint(this.transform.position).z;
        return camera.ScreenToWorldPoint(touchPoint);
    }
    private void Connect(CConnector connector)
    {
        this.line.SetPosition(1, connector.transform.position);
    }
    private void Disconnect(CConnector connector)
    {
        this.line.SetPosition(0, this.transform.position);
        this.line.SetPosition(1, this.transform.position);
    }

    ////////////////////
    /// PUBLIC METHODS
    ////////////////////
}
