using UnityEngine;

// 
public class OperationChange : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer = default;
    [SerializeField]
    string mapName;

    void OnTriggerEnter(Collider other)
    {
        var layerMask = 1 << other.gameObject.layer;
        if ((layerMask & playerLayer) != 0)
        {
            GameManager.Instance.SetActionMap(mapName);
        }
    }

    void OnTriggerExit(Collider other)
    {
        var layerMask = 1 << other.gameObject.layer;
        if ((layerMask & playerLayer) != 0)
        {
            
        }
    }
}
