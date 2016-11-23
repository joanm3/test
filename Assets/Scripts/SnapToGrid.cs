using UnityEngine;
using System.Collections;
using UnityEditor;
using UnityEngine.EventSystems;


[ExecuteInEditMode] [RequireComponent(typeof(BoxCollider))]
public class SnapToGrid : MonoBehaviour
{
    public BoxCollider childToApplyBoxCollider; 

    private void Awake()
    {
#if !UNITY_EDITOR
        Destroy(this); 
#endif

    }

}
