using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shadows : MonoBehaviour
{
    public GameObject wall;
    private GameObject clone;

    public MeshRenderer playerRenderer;

    private float playerHeight;

    void Start() {
        playerHeight = playerRenderer.bounds.size.y;
        GetObjects();
    }

    public void GetObjects(){
        GameObject[] allObjects = GameObject.FindGameObjectsWithTag("ShadowFix");
        FixShadows(allObjects);
    }

    public void FixShadows(GameObject[] allObjects){
        for (int i = 0; i < allObjects.Length; i++){
            GameObject original = allObjects[i];

            // Quaternion oRotation = original.transform.rotation;
            // original.transform.rotation = Quaternion.identity;

        
            MeshRenderer renderer = original.GetComponent<MeshRenderer>();
            if (renderer.bounds.size.y <= playerHeight){
                print(renderer.bounds.size.y + " " + playerHeight);
                continue;
            }

            clone = Instantiate(original);
            clone.transform.parent = original.transform;
            
            MeshRenderer cloneRenderer = clone.GetComponent<MeshRenderer>();
            

            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            cloneRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

            float yScale = (renderer.bounds.size.y - playerHeight) / renderer.bounds.size.y;
            clone.transform.localScale = new Vector3(clone.transform.localScale.x, yScale, clone.transform.localScale.z);
            
            //original.transform.rotation = oRotation;

            clone.transform.position = new Vector3(clone.transform.position.x, clone.transform.position.y - (playerHeight / 2), clone.transform.position.z);

            
        }
    }
}
