using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR;


public class controllerMimick : MonoBehaviour
{
    Mouse mouse;
    Keyboard keyboard;
    XRController controller;
    UnityEngine.XR.InputDevice leftHand;
    UnityEngine.XR.InputDevice leftHandfake;

    private void Awake()
    {
        mouse = InputSystem.GetDevice<Mouse>();
        keyboard = InputSystem.GetDevice<Keyboard>();
        var devices = new List<UnityEngine.InputSystem.InputDevice>();
        leftHand = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);
        Debug.Log(leftHand);
    }
    public IEnumerator mousePress(){
        using (StateEvent.From(mouse, out InputEventPtr eventPtr))
        {
            ((ButtonControl)mouse["leftButton"]).WriteValueIntoEvent<float>(1, eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }
        Debug.Log("Trigger pressed");
        yield return null; 
        using (StateEvent.From(mouse, out InputEventPtr eventPtr))
        {
            ((ButtonControl)mouse["leftButton"]).WriteValueIntoEvent<float>(0, eventPtr);
            InputSystem.QueueEvent(eventPtr);
        }
    }
    public IEnumerator keyboardMimic(char keypressed){
        Key keyused = GetKey(keypressed);
        KeyboardState stateA = new KeyboardState();
        KeyboardState stateB = new KeyboardState();
        stateA.Press(keyused);
        stateB.Release(keyused);
        InputSystem.QueueStateEvent(keyboard, stateA);
        yield return null;
        InputSystem.QueueStateEvent(keyboard, stateB);
    }
    Key GetKey(char keypressed){
        switch(keypressed){
            case 'T': return Key.T; //Left controller
            case 'Y': return Key.Y; //Right controller
            case 'B': return Key.B; //Primary button
            case 'N': return Key.N; //Secondary button
            default: return Key.U;
        }
    }

}
