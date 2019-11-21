using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [Range(0, 10)]
    public int axisCount;
    [Range(0, 20)]
    public int buttonCount;

    public Controller controller;

    public void PassInput(InputData data)
    {
       
        //Debug.Log ("Movement:" + data.axis[0]);
        controller.ReadInput(data);

    }

    public void RefreshTracker()
    {
        DeviceTracker dt = GetComponent<DeviceTracker>();
        if (dt != null)
        {
            dt.Refresh();
        }
    }
    
}

public struct InputData
{
    public float[] axis;
    public bool[] buttons;

    public InputData(int axesCount, int buttonCount)
    {
        axis = new float[axesCount];
        buttons = new bool[buttonCount];
    }
    
    public void Reset()
    {
        for (int i = 0; i < axis.Length; i++)
        {
            axis[i] = 0f;
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i] = false;
        }
    }
}
