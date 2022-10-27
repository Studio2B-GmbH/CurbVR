using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotAirBallonTest : MonoBehaviour
{
    public Transform VRPlayer;
    public float speed = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);

        if (Input.GetKey(KeyCode.UpArrow))
        {
            Move(1);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            Move(-1);
        }

        Move(input.y);

    }

    void Move(float input)
    {
        Vector3 pos = VRPlayer.position;
        Vector3 thispos = this.transform.position;

        thispos.y += input * Time.deltaTime * speed;
        pos.y += input * Time.deltaTime * speed;

        VRPlayer.position = pos;
        this.transform.position = thispos;
    }
}
