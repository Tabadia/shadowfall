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


        
            MeshRenderer renderer = original.GetComponent<MeshRenderer>();
            //if (renderer.bounds.size.y <= playerHeight){
            //    print(renderer.bounds.size.y + " " + playerHeight);
            //    continue;
           // }

            clone = Instantiate(original);
            GameObject emptyParent = new GameObject(original.name + " Shadow");

            // Set the parent's position to match the originalObject
            //emptyParent.transform.position = original.transform.position;

            // Set the parent's rotation to identity (no rotation)
            //emptyParent.transform.rotation = Quaternion.identity;
            clone.transform.parent = emptyParent.transform;


            // Reset the child's local position and rotation

            emptyParent.transform.localPosition = Vector3.zero;
            emptyParent.transform.localRotation = Quaternion.identity;
            emptyParent.transform.localScale = new Vector3(1, 1, 1);

            MeshRenderer cloneRenderer = clone.GetComponent<MeshRenderer>();
            

            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            cloneRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

            float yScale = (renderer.bounds.size.y - playerHeight) / renderer.bounds.size.y;
            emptyParent.transform.localScale = new Vector3(emptyParent.transform.localScale.x, yScale, emptyParent.transform.localScale.z);
           

            clone.transform.position = new Vector3(original.transform.position.x, original.transform.position.y - (playerHeight / 2), original.transform.position.z);
            

            
        }
    }
}
