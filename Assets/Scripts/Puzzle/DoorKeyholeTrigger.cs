using UnityEngine;

/// <summary>
/// Trigger sensor placed directly on the door keyhole slot.
/// When the brass key enters the trigger volume (VR grab) or player interacts with it (desktop),
/// unlocks the door via DoorController.
/// </summary>
public class DoorKeyholeTrigger : MonoBehaviour, IInteractable
{
    public DoorController doorController;

    public string InteractionPrompt
    {
        get
        {
            if (doorController == null || !doorController.isLocked) return "";
            VRDesktopTester tester = Object.FindFirstObjectByType<VRDesktopTester>();
            bool isHoldingKey = (tester != null && tester.heldObject != null && tester.heldObject.GetComponent<KeyController>() != null);
            return isHoldingKey ? "★ [Left Click / E] Insert Brass Key into Socket ★" : "Keyhole Socket (Insert Brass Key to unlock)";
        }
    }

    public void Interact(GameObject user)
    {
        if (doorController != null && doorController.isLocked)
        {
            VRDesktopTester tester = user.GetComponent<VRDesktopTester>() ?? Object.FindFirstObjectByType<VRDesktopTester>();
            if (tester != null && tester.heldObject != null)
            {
                KeyController key = tester.heldObject.GetComponent<KeyController>();
                if (key != null && !key.hasBeenUsed)
                {
                    tester.DropHeldObject();
                    doorController.UnlockDoor(key);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (doorController != null && doorController.isLocked)
        {
            KeyController key = other.GetComponent<KeyController>() ?? other.GetComponentInParent<KeyController>();
            if (key != null && !key.hasBeenUsed)
            {
                doorController.UnlockDoor(key);
            }
        }
    }
}

