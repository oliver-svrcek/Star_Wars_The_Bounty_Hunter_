using UnityEngine;

public class AudioListenerManagement : MonoBehaviour
{
    private Quaternion FixedRotation { get; set; } = new Quaternion();
    
    private void Awake()
    {
        FixedRotation = transform.rotation;
    }

    private void LateUpdate()
    {
        // Used to fix rotation of audio source game object.
        transform.rotation = FixedRotation;
    }
}
