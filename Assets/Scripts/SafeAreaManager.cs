using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SafeAreaManager : MonoBehaviour
{
    Vector2 minAnchor;
    Vector2 maxAnchor;

    // Start is called before the first frame update
    void Start()
    {
        var myRect = GetComponent<RectTransform>();    

        minAnchor = Screen.safeArea.min;   
        maxAnchor = Screen.safeArea.max;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;

        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        myRect.anchorMin = minAnchor;
        myRect.anchorMax = maxAnchor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
