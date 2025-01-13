using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Test : MonoBehaviour
{

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(IsTrue());
        }
    }

    bool IsTrue()
    {
        if (Random.Range((int)0, (int)2) == 0)
            return true;
        else return false;
    }
}
