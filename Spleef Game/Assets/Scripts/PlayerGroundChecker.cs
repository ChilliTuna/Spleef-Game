using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    private MeshCollider meshCollider;

    public int currentCollisions = 0;

    private void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        currentCollisions++;
    }

    private void OnTriggerExit(Collider other)
    {
        currentCollisions--;
    }
}