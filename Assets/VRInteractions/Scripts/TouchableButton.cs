using UnityEngine;
using UnityEngine.Events;

public class TouchableButton : InteractiveObject
{
    private Vector3 initialPosition;
    public Transform buttonMesh;
    public Transform pressedPosition;
    public UnityEvent onPressed;
    public UnityEvent onReleased;

    private void Start()
    {
        initialPosition = transform.position;
    }

    public override void OnTouchStart()
    {
        // Move the button mesh to the pressed position
        buttonMesh.position = pressedPosition.position;

        // Trigger the on pressed event
        onPressed.Invoke();
    }

    public override void OnTouchEnd()
    {
        // Move the button mesh back to its initial position
        buttonMesh.position = initialPosition;

        // Trigger the on released event
        onReleased.Invoke();
    }
}
