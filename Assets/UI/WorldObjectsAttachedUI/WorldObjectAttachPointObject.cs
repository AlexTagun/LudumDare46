using UnityEngine;

public class WorldObjectAttachPointObject : MonoBehaviour
{
    public Vector3 attachPoint => gameObject.transform.position;
}