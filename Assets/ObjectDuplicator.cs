using UnityEngine;

public class ObjectDuplicator : MonoBehaviour
{
    public GameObject pipePrefab;
    public GameObject cube1Prefab;
    public GameObject cylinderPrefab;
    public GameObject cube2Prefab;
    public GameObject cylinderPrefab2;

    public Transform pipeSpawnPoint;
    public Transform cube1SpawnPoint;
    public Transform cylinderSpawnPoint;
    public Transform cube2SpawnPoint;
    public Transform cylinder2SpawnPoint;

    public void DuplicatePipe()
    {
        Instantiate(pipePrefab, pipeSpawnPoint.position, pipeSpawnPoint.rotation);
    }

    public void DuplicateCube1()
    {
        Instantiate(cube1Prefab, cube1SpawnPoint.position, cube1SpawnPoint.rotation);
    }

    public void DuplicateCylinder()
    {
        Instantiate(cylinderPrefab, cylinderSpawnPoint.position, cylinderSpawnPoint.rotation);
    }

    public void DuplicateCylinder2()
    {
        Instantiate(cylinderPrefab2, cylinder2SpawnPoint.position, cylinder2SpawnPoint.rotation);
    }

    public void DuplicateCube2()
    {
        Instantiate(cube2Prefab, cube2SpawnPoint.position, cube2SpawnPoint.rotation);
    }
}
