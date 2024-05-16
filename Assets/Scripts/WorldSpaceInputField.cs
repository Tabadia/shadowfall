using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;

public class WorldSpaceTMPInputField : MonoBehaviour
{
    public TMP_InputField inputField;
    public float maxDistance = 10f; // Maximum distance from the UI for raycasting to occur
    public Camera mainCamera; // Serialized reference to the camera
    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private bool inputFieldSelected = false; // Track if the input field is currently selected

    void Start()
    {
        // Find the Canvas to which the input field belongs
        Canvas canvas = inputField.GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("No Canvas found for the input field.");
            return;
        }

        // Find the GraphicRaycaster associated with the Canvas
        raycaster = canvas.GetComponent<GraphicRaycaster>();
        if (raycaster == null)
        {
            Debug.LogError("No GraphicRaycaster found on the Canvas.");
            return;
        }

        pointerEventData = new PointerEventData(EventSystem.current);
    }

    void Update()
    {
        if (!inputFieldSelected && Vector3.Distance(transform.position, mainCamera.transform.position) <= maxDistance)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (IsPointerOverUIObject())
                {
                    inputField.Select();
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    inputFieldSelected = true;
                    PlayerController.canMove = false;
                }
            }
        }

        if (inputFieldSelected && Input.GetMouseButtonDown(0) && !IsPointerOverUIObject())
        {
            inputFieldSelected = false;
            PlayerController.canMove = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            inputField.DeactivateInputField();
        }
    }

    private bool IsPointerOverUIObject()
    {
        pointerEventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerEventData, results);
        return results.Count > 0;
    }
}
