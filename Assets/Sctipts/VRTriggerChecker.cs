using UnityEngine;
using UnityEngine.InputSystem;

public class VRTriggerChecker : MonoBehaviour
{
    // Reference to the InputAction for the trigger
    private InputAction _triggerAction;

    // Start is called before the first frame update
    void Start()
    {
        // Enable the trigger action
        _triggerAction = new InputAction("Trigger", binding: "<XRController>/triggerButton");
        _triggerAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the trigger button is pressed
        if (_triggerAction.ReadValue<float>() > 0.1f)
        {
            // Do something when the trigger is pressed
            Debug.Log("Trigger button pressed!");
        }
    }

    // Clean up the InputAction when the script is disabled or destroyed
    private void OnDisable()
    {
        _triggerAction.Disable();
    }

    private void OnDestroy()
    {
        _triggerAction.Dispose();
    }
}
