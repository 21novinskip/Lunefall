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
    public void SavePos()
    {
        if (sceneBoxes != null || sceneBoxes.Length != 0)
        {
            foreach (BoxTransform box in sceneBoxes)
            {
                box.boxX = box.boxObj.transform.position.x;
                box.boxY = box.boxObj.transform.position.y;
                box.boxZ = box.boxObj.transform.position.z;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
