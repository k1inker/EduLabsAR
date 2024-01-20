using UnityEngine;
using UnityEngine.EventSystems;
using Camera = UnityEngine.Camera;

public class CWire : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private LineRenderer line;
    private PointerEventData currentEventData;
    private bool isClicked;
    private Vector3 offSet;
    private Camera camera;
    private CWire connectedWire;
    private void Start()
    {
        this.camera = Camera.main;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (this.isClicked)
        {
            return;
        }

        if (this.connectedWire != null)
        {
            this.connectedWire.Disconnect();
            this.connectedWire = null;
        }
        
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
        
        CheckToConnectWire();
        
        this.currentEventData = null;
        this.isClicked = false;
    }

    private void CheckToConnectWire()
    {
        Vector3 position = this.camera.transform.position;
        Vector3 rayOrigin = position;
        Vector3 rayDir = TouchWorldPoint(this.currentEventData.position) - position;

        if (Physics.Raycast(rayOrigin, rayDir, out RaycastHit hitInfo))
        {
            CWire connectedWire = hitInfo.transform.GetComponent<CWire>();
            
            if (connectedWire != null)
            {
                this.line.SetPosition(1, hitInfo.transform.position);
                connectedWire.Connect(this);
            }
            else
            {
                this.line.SetPosition(1, this.transform.position);
            }
        }
        else
        {
            this.line.SetPosition(1, this.transform.position);
        }
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

    public void Connect(CWire connectedWire)
    {
        this.connectedWire = connectedWire;
    }

    public void Disconnect()
    {
        this.line.SetPosition(1, this.transform.position);
    }
}
