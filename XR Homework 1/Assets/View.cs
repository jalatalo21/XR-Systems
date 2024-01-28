using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class View : MonoBehaviour
{
    public InputActionReference action;
    public string paikka="sisalla";
    // Start is called before the first frame update
    void Start()
    {
        action.action.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        action.action.performed += (ctx) =>
        {
            if (paikka == "sisalla") {
                this.transform.position = new Vector3(-31, 15, 40);
                paikka = "ulkona";
            }
            else {
                paikka = "sisalla";
                this.transform.position = new Vector3(0, 0, 0);
            }
        };
    }
}
