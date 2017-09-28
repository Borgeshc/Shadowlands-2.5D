using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationBaker : MonoBehaviour
{
    public List<NavMeshSurface> surfaces;
    public Transform[] objectsToRotate;

    void OnEnable()
    {
        LevelGenerator.OnFloorCompleted += StartGeneration;
    }

    void OnDisable()
    {
        LevelGenerator.OnFloorCompleted -= StartGeneration;
    }

    void StartGeneration(List<GameObject> floors)
    {
        StartCoroutine(GenerateNavMesh(floors));
    }

    public IEnumerator GenerateNavMesh(List<GameObject> floors)
    {
        for(int k = 0; k < floors.Count; k++)
        {
            surfaces.Add(floors[k].GetComponentInChildren<NavMeshSurface>());
        }

        //for (int j = 0; j < objectsToRotate.Length; j++)
        //{
        //    objectsToRotate[j].localRotation = Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0));
        //}

        for (int i = 0; i < surfaces.Count; i++)
        {
            surfaces[i].BuildNavMesh();
            yield return 0;
        }
    }
}