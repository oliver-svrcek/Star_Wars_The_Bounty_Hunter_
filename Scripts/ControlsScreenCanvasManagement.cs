
using UnityEngine;

public class ControlsScreenCanvasManagement : MonoBehaviour
{
    private Vector3 FixedPosition { get; set; }
    
    private void Start()
    {
        FixedPosition = transform.position;
    }

    private void LateUpdate()
    {
        // Used to fix rotation of controls screen canvas game object.
        transform.position = FixedPosition;
    }
}
