using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ColorManager : MonoBehaviour
{
    public GameObject[] red;
    public GameObject[] blue;
    public bool colorRed;
    
    // Start is called before the first frame update
    void Start()
    {
        red = GameObject.FindGameObjectsWithTag("red");
        blue = GameObject.FindGameObjectsWithTag("blue");
        colorRed = false;
        ToggleColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeColor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ToggleColor();
        }
    }

    private void ToggleColor()
    {
        colorRed = !colorRed;
        foreach (GameObject r in red){
            r.SetActive(colorRed);
        }
        foreach (GameObject b in blue){
            b.SetActive(!colorRed);
        }
    }
}
