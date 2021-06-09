using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pencil : ThrowableObject
{
    public GameObject pencilStrokePrefab;
    public Transform pencilTip;
    public string undoButtonName;

    private GameObject currentStroke;
    private Stack<GameObject> strokes = new Stack<GameObject>();

    public override void OnTriggerStart()
    {
        base.OnTriggerStart();

        // Create a pencil stroke and parent it to the pencil tip
        currentStroke = Instantiate(pencilStrokePrefab, pencilTip.position, Quaternion.identity, pencilTip);
    }

    public override void OnTriggerEnd()
    {
        base.OnTriggerEnd();

        // Unparent the current pencil stroke from the pencil tip
        currentStroke.transform.SetParent(null);

        // Add the pencil stroke to the stack of previous strokes
        strokes.Push(currentStroke);
    }

    private void Update()
    {
        // If the undo button is pressed and there are any stroke to be undone
        if(Input.GetButtonDown(undoButtonName) && strokes.Count > 0)
        {
            // Pop the last stroke from the stack
            GameObject lastStroke = strokes.Pop();

            // Destroy the stroke
            Destroy(lastStroke);
        }

    }
}
