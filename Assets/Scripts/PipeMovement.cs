using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public bool direction = false;
    private float m_Distance = 0;
    // Update is called once per frame
    void Update()
    {
        Transform trans = this.transform;
        Transform ch = trans.Find("pipe");
        Vector2 v = new Vector2(0, 0);



        if (m_Distance > 500)
        {
            m_Distance = 0;
            direction = !direction;
        }
        else ++m_Distance;

        if (direction) trans.Translate(v * -1 * Time.deltaTime);
        else trans.Translate(v * Time.deltaTime);
    }
}
