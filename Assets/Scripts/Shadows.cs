using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadows : MonoBehaviour
{
    public GameObject wall;
    private GameObject clone;

    public MeshRenderer playerRenderer;

    void Start() {
        GetObjects();
    }

    public void GetObjects(){
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("ShadowFix");
        FixShadows(allObjects);
    }

    public void FixShadows(GameObject[] allObjects){
        for (int i = 0; i < allObjects.Length; i++){
            GameObject original = allObjects[i];

            clone = Instantiate(original);
            clone.transform.parent = original.transform;
            
            MeshRenderer renderer = original.GetComponent<MeshRenderer>();
            MeshRenderer cloneRenderer = clone.GetComponent<MeshRenderer>();
            

            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            cloneRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

            print(renderer.bounds.size);
            float yScale = (renderer.bounds.size.y - playerRenderer.bounds.size.y) / renderer.bounds.size.y;
            clone.transform.localScale = new Vector3(clone.transform.localScale.x, yScale, clone.transform.localScale.z);

            clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y - (playerRenderer.bounds.size.y / 2), clone.transform.position.z);
        }
    }
}
