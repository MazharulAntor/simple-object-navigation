using UnityEngine;

public class ObjectInstantiation : MonoBehaviour
{
    public Vector3 cubeSize;
    public Vector3 sphereSize;
    public GameObject cubePrefrab;
    public GameObject spherePrefab;
    private bool isCubeButtonClicked = false, isSphereButtonClicked = false;
    private SceneManager sceneManager;

    void Start()
    {
        cubePrefrab.transform.localScale = cubeSize;
        spherePrefab.transform.localScale = sphereSize;
        sceneManager = FindObjectOfType<SceneManager>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.name == "InstantiationArea")
                {
                    if (isCubeButtonClicked)
                    {
                        GameObject newPrefab = Instantiate(cubePrefrab, hit.point, Quaternion.identity);
                        newPrefab.transform.position = new Vector3(hit.point.x, cubePrefrab.transform.localScale.y / 2f, hit.point.z);
                        sceneManager.instantiatedObjects.Add(newPrefab.transform);
                    }
                    else if (isSphereButtonClicked)
                    {
                        GameObject newPrefab = Instantiate(spherePrefab, hit.point, Quaternion.identity);
                        newPrefab.transform.position = new Vector3(hit.point.x, cubePrefrab.transform.localScale.y / 2f, hit.point.z);
                        sceneManager.instantiatedObjects.Add(newPrefab.transform);
                    }
                }
            }
        }
    }

    public void OnClickObjectButton(string buttonName)
    {
        if (buttonName == "Cube")
        {
            isCubeButtonClicked = true;
            isSphereButtonClicked = false;
        }
        else if (buttonName == "Sphere")
        {
            isCubeButtonClicked = false;
            isSphereButtonClicked = true;
        }
    }
}
