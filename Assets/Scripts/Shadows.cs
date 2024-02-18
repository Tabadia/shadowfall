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

            clone = Instantiate(original);
            clone.name = (original.name + "'s Shadow");
            GameObject emptyParent = new GameObject("Temp");

            // Set the parent's position to match the originalObject
            //emptyParent.transform.position = original.transform.position;

            // Set the parent's rotation to identity (no rotation)
            //emptyParent.transform.rotation = Quaternion.identity;
            clone.transform.parent = emptyParent.transform;


            // Reset the child's local position and rotation

            emptyParent.transform.localPosition = clone.transform.position;
            clone.transform.localPosition = new Vector3(0,0,0);
            emptyParent.transform.localRotation = Quaternion.identity;
            emptyParent.transform.localScale = new Vector3(1, 1, 1);

            MeshRenderer cloneRenderer = clone.GetComponent<MeshRenderer>();
            

            renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            cloneRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

            float yScale = (renderer.bounds.size.y - playerHeight) / renderer.bounds.size.y;
            if ((original.name == "Player Object") || (renderer.bounds.size.y <= playerHeight)){
                yScale = .5f;
            }

            emptyParent.transform.localScale = new Vector3(emptyParent.transform.localScale.x, yScale, emptyParent.transform.localScale.z);

            emptyParent.transform.position = new Vector3(emptyParent.transform.position.x, emptyParent.transform.position.y - (playerHeight / 2), emptyParent.transform.position.z);
            
            clone.transform.SetParent(original.transform, true);
            Destroy(emptyParent);

            if (original.name == "Player Object"){
                clone.transform.localPosition = new Vector3(0, -.5f, 0);
            }
            else if (renderer.bounds.size.y <= playerHeight){
                clone.transform.localPosition = new Vector3(clone.transform.localPosition.x, -0.25f, clone.transform.localPosition.z);
            }
            

            
        }
    }
}
