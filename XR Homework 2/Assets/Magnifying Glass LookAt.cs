using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifyingGlassLookAt : MonoBehaviour
{

    public GameObject player;
    public GameObject glass;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt((player.transform.position - transform.position) * -100, glass.transform.up);
    }
}