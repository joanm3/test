﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using EditorSupport;

[ExecuteInEditMode]
[RequireComponent(typeof(BoxCollider))]
public class LevelGrid : MonoBehaviour
{
    //public GameObject testToGrid; 
    public static LevelGrid Ins;

    public bool hideUnityHandles = false;
    public enum Pow2 { g0 = 0, g1 = 1, g2 = 2, g4 = 4, g8 = 8, g16 = 16, g32 = 32, g64 = 64, g128 = 128, g256 = 256, g512 = 512, g1024 = 1024, g2048 = 2048 }
    public bool snapToGrid = true;
    public Pow2 gridSize = Pow2.g128;
    public Pow2 heightGridSize = Pow2.g0;
    public float height;
    [Tooltip("Standard 3DSMax = 0.0254")]
    public float scaleFactor = 0.0254f;
    public int heightIndex;
    [HideInInspector]
    public GameObject selectedGameObject;
    public bool showVerticalGrid;




    public float sizeColums = 25;

    public float sizeRows = 10;
    public BoxCollider boxCollider;


    private readonly Color _normalColor = Color.grey;
    private readonly Color _selectedColor = Color.yellow;

    public float SizeColumns
    {
        get { return sizeColums; }
        set { sizeColums = value; }
    }

    public float SizeRows
    {
        get { return sizeRows; }
        set { sizeRows = value; }
    }

    private void GridFrameGizmo(float length, float width, float height)
    {
        float cols;
        float rows;

        cols = (length / ((float)(gridSize) * scaleFactor));
        rows = (width / ((float)(gridSize) * scaleFactor));

        Gizmos.DrawLine(new Vector3(0, (float)height, 0), new Vector3(0, (float)height, rows * (float)gridSize * scaleFactor));
        Gizmos.DrawLine(new Vector3(0, (float)height, 0), new Vector3(cols * (float)gridSize * scaleFactor, (float)height, 0));
        Gizmos.DrawLine(new Vector3(0, (float)height, rows * (float)gridSize * scaleFactor), new Vector3(cols * (float)gridSize * scaleFactor, (float)height, rows * (float)gridSize * scaleFactor));
        Gizmos.DrawLine(new Vector3(cols * (float)gridSize * scaleFactor, (float)height, rows * (float)gridSize * scaleFactor), new Vector3(cols * (float)gridSize * scaleFactor, (float)height, 0));
    }

    private void GridGizmo(float length, float width, float height)
    {
        float cols;
        float rows;

        cols = (length / ((float)(gridSize) * scaleFactor));
        rows = (width / ((float)(gridSize) * scaleFactor));

        for (int i = 1; i < cols; i++)
        {
            Gizmos.DrawLine(
                new Vector3(i * ((float)gridSize) * scaleFactor, (float)height, 0),
                new Vector3(i * (float)gridSize * scaleFactor, (float)height, rows * (float)gridSize * scaleFactor));
        }


        for (int j = 1; j < rows; j++)
        {

            Gizmos.DrawLine(
                new Vector3(0, height, (j * (float)gridSize) * scaleFactor),
                new Vector3((cols * (float)gridSize * scaleFactor), height, (j * (float)gridSize * scaleFactor)));

        }
    }

    private void VerticalGridGizmo(Vector3 objectPosition, int numberOfRows, bool xAxis)
    {

        if (xAxis)
        {
            //columna 1
            Gizmos.DrawLine(
                new Vector3(objectPosition.x, objectPosition.y, objectPosition.z),
                new Vector3(objectPosition.x, objectPosition.y + ((float)heightGridSize * scaleFactor * (numberOfRows / 2)), objectPosition.z)
                );
            Gizmos.DrawLine(
                new Vector3(objectPosition.x, objectPosition.y, objectPosition.z),
                new Vector3(objectPosition.x, objectPosition.y + ((float)heightGridSize * scaleFactor * (-numberOfRows / 2)), objectPosition.z)
                );

            //columna 2 x
            Gizmos.DrawLine(
                new Vector3(objectPosition.x + (((float)gridSize) * scaleFactor), objectPosition.y, objectPosition.z),
                new Vector3(objectPosition.x + (((float)gridSize) * scaleFactor), objectPosition.y + ((float)heightGridSize * scaleFactor * (numberOfRows / 2)), objectPosition.z)
                );
            Gizmos.DrawLine(
                new Vector3(objectPosition.x + (((float)gridSize) * scaleFactor), objectPosition.y, objectPosition.z),
                new Vector3(objectPosition.x + (((float)gridSize) * scaleFactor), objectPosition.y + ((float)heightGridSize * scaleFactor * (-numberOfRows / 2)), objectPosition.z)
                );
            for (int j = -(numberOfRows / 2); j < (numberOfRows / 2) + 1; j++)
            {
                Gizmos.DrawLine(
                    new Vector3(objectPosition.x, objectPosition.y + (j * (float)heightGridSize * scaleFactor), objectPosition.z),
                    new Vector3(objectPosition.x + (((float)gridSize) * scaleFactor), objectPosition.y + (j * (float)heightGridSize * scaleFactor), objectPosition.z)
                    );
            }
        }
        else
        {
            //columna 1
            Gizmos.DrawLine(
                new Vector3(objectPosition.x, objectPosition.y, objectPosition.z),
                new Vector3(objectPosition.x, objectPosition.y + ((float)heightGridSize * scaleFactor * (numberOfRows / 2)), objectPosition.z)
                );
            Gizmos.DrawLine(
                new Vector3(objectPosition.x, objectPosition.y, objectPosition.z),
                new Vector3(objectPosition.x, objectPosition.y + ((float)heightGridSize * scaleFactor * (-numberOfRows / 2)), objectPosition.z)
                );

            //columna 2 x
            Gizmos.DrawLine(
                new Vector3(objectPosition.x, objectPosition.y, (objectPosition.z + (((float)gridSize) * scaleFactor))),
                new Vector3(objectPosition.x, objectPosition.y + ((float)heightGridSize * scaleFactor * (numberOfRows / 2)), (objectPosition.z + (((float)gridSize) * scaleFactor)))
                );
            Gizmos.DrawLine(
                new Vector3(objectPosition.x, objectPosition.y, (objectPosition.z + (((float)gridSize) * scaleFactor))),
                new Vector3(objectPosition.x, objectPosition.y + ((float)heightGridSize * scaleFactor * (-numberOfRows / 2)), (objectPosition.z + (((float)gridSize) * scaleFactor)))
                );



            for (int j = -(numberOfRows / 2); j < (numberOfRows / 2) + 1; j++)
            {
                Gizmos.DrawLine(
                    new Vector3(objectPosition.x, objectPosition.y + (j * (float)heightGridSize * scaleFactor), objectPosition.z),
                    new Vector3(objectPosition.x, objectPosition.y + (j * (float)heightGridSize * scaleFactor), objectPosition.z + (((float)gridSize) * scaleFactor))
                    );
            }

        }
    }

    public Vector3 WorldToGridCoordinates(Vector3 point)
    {
        Vector3 gridPoint = new Vector3(
            (float)((point.x - transform.position.x) / (float)gridSize) * scaleFactor, (float)height * scaleFactor,
            (float)((point.z - transform.position.z) / (float)gridSize) * scaleFactor);
        return gridPoint;
    }

    public Vector3 GridToWorldCoordinates(float col, float row, float height)
    {
        Vector3 worldPoint = new Vector3(
            transform.position.x + (col * (float)gridSize) * scaleFactor, (float)height,
            transform.position.z + (row * (float)gridSize) * scaleFactor);
        return worldPoint;
    }

    public Vector3 VerticalGridToWorldCoordinates(float col, float row, Vector3 objectPosition, bool xAxis)
    {
        Vector3 worldPoint = Vector3.zero;
        switch (xAxis)
        {
            case true:
                worldPoint = new Vector3(
                    transform.position.x + (col * (float)heightGridSize) * scaleFactor,
                    transform.position.y + (row * (float)heightGridSize) * scaleFactor,
                    objectPosition.z);
                break;
            case false:
                worldPoint = new Vector3(
                    objectPosition.x,
                    transform.position.y + (row * (float)heightGridSize) * scaleFactor,
                    transform.position.x + (col * (float)heightGridSize) * scaleFactor);
                break;
        }

        return worldPoint;
    }

    public bool IsInsideGridBounds(Vector3 point)
    {
        float minX = transform.position.x;
        float maxX = minX + sizeColums;
        float minZ = transform.position.z;
        float maxZ = minZ + sizeColums;
        return (point.x >= minX && point.x <= maxX && point.z >= minZ && point.z <= maxZ);
    }

    public bool IsInsideGridBounds(float col, float row)
    {
        return (col >= 0 && col < (sizeColums) && row >= 0 && row < (sizeColums));
    }

    private void Awake()
    {
#if !UNITY_EDITOR
        Destroy(this); 
#endif
        if (Ins != null && Ins != this)
            DestroyImmediate(this);

        if (Ins == null)
            Ins = this;

        gameObject.name = "LevelGrid";

        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnEnable()
    {
        if (Ins != null && Ins != this)
            DestroyImmediate(this);

        if (Ins == null)
            Ins = this;

        boxCollider = GetComponent<BoxCollider>();

    }

    private void OnDrawGizmos()
    {
        Color oldColor = Gizmos.color;
        Gizmos.color = _normalColor;

        if ((float)gridSize == 0)
            return;


        height = heightIndex * scaleFactor * (float)heightGridSize;

        GridGizmo(sizeColums, sizeRows, height);
        Gizmos.color = _selectedColor;
        //Function not yet integrated. to do later. 
        // VerticalGridGizmo(Selection.activeGameObject.transform.position, 8, false);
        GridFrameGizmo(sizeColums, sizeRows, height);
        Gizmos.color = oldColor;
    }

    public void UpdateInputGridHeight()
    {

        if (Event.current.type == EventType.keyUp)
        {

            if (Event.current.keyCode == KeyCode.R)
                heightIndex++;

            else if (Event.current.keyCode == KeyCode.T)
                heightIndex--;

            else if (Event.current.keyCode == KeyCode.Alpha0 || Event.current.keyCode == KeyCode.E)
                heightIndex = 0;

            //else if (Event.current.keyCode == KeyCode.Alpha1)
            //    heightIndex = 1;

            //else if (Event.current.keyCode == KeyCode.Alpha2)
            //    heightIndex = 2;

            //else if (Event.current.keyCode == KeyCode.Alpha3)
            //    heightIndex = 3;

            //else if (Event.current.keyCode == KeyCode.Alpha4)
            //    heightIndex = 4;

            //else if (Event.current.keyCode == KeyCode.Alpha5)
            //    heightIndex = 5;

            //else if (Event.current.keyCode == KeyCode.Alpha6)
            //    heightIndex = 6;

            //else if (Event.current.keyCode == KeyCode.Alpha7)
            //    heightIndex = 7;

            //else if (Event.current.keyCode == KeyCode.Alpha8)
            //    heightIndex = 8;

            //else if (Event.current.keyCode == KeyCode.Alpha9)
            //    heightIndex = 9;

        }
    }

    [MenuItem("GameObject/3D Object/Snap To Grid GameObject")]
    public static void CreateObject()
    {
        GameObject go = Instantiate(Resources.Load("Standard SnapToGrid", typeof(GameObject))) as GameObject;
        go.transform.position = Vector3.zero;
        go.name = "SnapToGrid";
        //GameObject go = PrefabUtility.InstantiatePrefab(PrefabUtility.GetPrefabParent(Selection.activeObject) as GameObject) as GameObject;
    }

}
