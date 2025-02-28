using UnityEngine;

public class Interact : MonoBehaviour
{
    [SerializeField] private Transform InteractorSource;
    [SerializeField] private float InteractRange = 2f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
            if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange))
            {
                IInteractable interactObj = hitInfo.collider.GetComponent<IInteractable>();
                if (interactObj != null)
                {
                    interactObj.Interact();
                    Debug.Log("hi");
                }
            }
        }
    }
}
