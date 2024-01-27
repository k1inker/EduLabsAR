using UnityEngine;

public abstract class CElectricalComponent : MonoBehaviour 
{
    [SerializeField] private CConnector[] connectors = new CConnector[2];
    public bool IsOurComponentConnector(CConnector chekenConnector)
    {
        foreach (CConnector connector in connectors)
        {
            if(connector.Equals(chekenConnector))
            {
                return true;
            }
        }
        return false;
    }
}
