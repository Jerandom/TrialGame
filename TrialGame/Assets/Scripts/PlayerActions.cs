using TMPro;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField]
    private Transform Camera;
    [SerializeField]
    private float MaxUseDistance = 5f;
    [SerializeField]
    private LayerMask UseLayers;

    public void OnUse()
    {
        if (Physics.Raycast(Camera.position, Camera.forward, out RaycastHit hit, MaxUseDistance, UseLayers))
        {
            //Debug.Log(hit.collider.name);
            if (hit.collider.TryGetComponent<Door>(out Door door))
            {
                if (door.IsOpen)
                {
                    door.Close();
                }
                else
                {
                    door.Open(transform.position);
                }
            }
        }
    }
}

