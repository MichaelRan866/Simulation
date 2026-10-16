using Unity.ProjectAuditor.Editor.Core;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorSpawner : MonoBehaviour
{
    public InputActionAsset inputs;
    public Camera cam;
    public GameObject[] objs;

    private void Awake()
    {
        inputs.Enable();
    }
    private void Update()
    {
        for (int i = 0; i < objs.Length; i++)
        {
            if (inputs.FindAction("Type" + (i + 1).ToString()).WasPressedThisFrame()) 
            {
                Vector3 spawnPos = cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
                Instantiate(objs[i], new Vector3(spawnPos.x, spawnPos.y, 0f), Quaternion.identity);
            }
        }
    }
}
