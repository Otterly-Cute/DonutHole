using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hole_Movement : MonoBehaviour
{
    [Header ("Hole mesh")]
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshCollider meshCollider;

    [Header("Hole vertices radius")]
    [SerializeField] Vector2 moveLimits;
    [SerializeField] float radius;
    [SerializeField] Transform holeCenter;

    [Space]
    [SerializeField] float moveSpeed;

    Mesh mesh;
    List<int> holeVertices;
    List<Vector3> offsets;
    int holeVerticesCount;

    float x, y;
    Vector3 touch, targetPos;
    
    void Start()
    {
        Game_Data.isMoving = false;
        Game_Data.isGameOver = false;

        holeVertices = new List<int> ();
        offsets = new List<Vector3> ();

        mesh = meshFilter.mesh;

        //Find holde vertices on the mesh
        FindHoleVertices();
    }


    void Update()
    {
        Game_Data.isMoving = Input.GetMouseButton(0);

        if (!Game_Data.isGameOver && Game_Data.isMoving ) 
        {
            // Move hole center
            MoveHole();

            // Update hole vertices
            UpdateHoleVerticesPosition();
        }
    }

    void MoveHole()
    {
        x = Input.GetAxis("Mouse X");
        y = Input.GetAxis("Mouse Y");

        touch = Vector3.Lerp (
            holeCenter.position,
            holeCenter.position + new Vector3(x, 0f, y), 
            moveSpeed * Time.deltaTime
        );

        targetPos = new Vector3 (
            Mathf.Clamp (touch.x, -moveLimits.x, moveLimits.x),
            touch.y,
             Mathf.Clamp(touch.z, -moveLimits.y, moveLimits.y)
        );

        //holeCenter.position = targetPos;
        holeCenter.position = touch;
    }

    void UpdateHoleVerticesPosition()
    {
        Vector3[] vertices = mesh.vertices;
        for (int i = 0; i < holeVerticesCount; i++) 
        { 
            vertices[holeVertices[i]] = holeCenter.position + offsets[i];
        }

        // Update mesh
        mesh.vertices = vertices;
        meshFilter.mesh = mesh;
        meshCollider.sharedMesh = mesh;
    }

    void FindHoleVertices ()
    {
        for (int i = 0; i < mesh.vertices.Length; i++) 
        { 
            float distance = Vector3.Distance (holeCenter.position, mesh.vertices[i]);

            if (distance < radius)
            {
                holeVertices.Add (i);
                offsets.Add(mesh.vertices[i] - holeCenter.position);
            }
        }

        holeVerticesCount = holeVertices.Count;
    }

    // Use this to visualize the radius in the editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere (holeCenter.position, radius);
    }
}
