using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class GetBackBall : MonoBehaviour
{
    // Start is called before the first frame update
    private InputAction _triggerAction;
    public GameObject ball;
    void Start()
    {
        _triggerAction = new InputAction("Trigger", binding: "<XRController>/triggerButton");
        _triggerAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (_triggerAction.ReadValue<float>() > 0.5f)
        {
            ball.transform.position = this.transform.position;
        }
    }
    private void OnDisable()
    {
        _triggerAction.Disable();
    }

    private void OnDestroy()
    {
        _triggerAction.Dispose();
    }

}
