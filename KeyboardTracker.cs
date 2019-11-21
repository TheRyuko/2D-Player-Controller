using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardTracker : DeviceTracker
{
    public AxisKeys[] axisKeys;
    public KeyCode[] buttonKeys;

    void Reset()
    {
        im = GetComponent<InputManager>();
        axisKeys = new AxisKeys[im.axisCount];
        buttonKeys = new KeyCode[im.buttonCount];
    }

    public override void Refresh()
    {
        im = GetComponent<InputManager>();

        // 2 temp arrays for buttons and axis
        KeyCode[] newButtons = new KeyCode[im.buttonCount];
        AxisKeys[] newAxis = new AxisKeys[im.axisCount];

        if(buttonKeys != null)
        {
            for (int i = 0; i < Mathf.Min(newButtons.Length, buttonKeys.Length); i++)
            {
                newButtons[i] = buttonKeys[i];
            }
        }
        buttonKeys = newButtons;

        if(axisKeys != null)
        {
            for (int i = 0; i < Mathf.Min(newAxis.Length, axisKeys.Length); i++)
            {
                newAxis[i] = axisKeys[i];
            }
        }
        axisKeys = newAxis;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for input. if new input detected set true clear
        //Populate InputData to pass to the Input Manager

        for (int i = 0; i < axisKeys.Length; i++)
        {
            float val = 0f;
            if (Input.GetKey (axisKeys[i].positive))
            {
                val += 1f;
                newData = true;
            }

            if (Input.GetKey (axisKeys[i].negative))
            {
                val -= 1f;
                newData = true;
            }

            data.axis[i] = val;

        }
    
        for (int i = 0; i < buttonKeys.Length; i++)
        {
            if (Input.GetKey(buttonKeys[i]))
            {
                data.buttons[i] = true;
                newData = true;
            }
        }
        
        if (newData)
        {
            im.PassInput(data);
            newData = false;
            data.Reset();
        }
    }
}

[System.Serializable]
public struct AxisKeys
{
    public KeyCode positive;
    public KeyCode negative;
}