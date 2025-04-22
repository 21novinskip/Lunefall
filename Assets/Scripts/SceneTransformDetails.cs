using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransformDetails : MonoBehaviour
{
    [System.Serializable]
    public class BoxTransform
    {
        public GameObject boxObj;
        public float boxX;
        public float boxY;
        public float boxZ;
    }
    public BoxTransform[] sceneBoxes;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
