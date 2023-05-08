using UnityEngine;

public class ObjectNavigation : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    public Material selectedObjectMaterial;
    public Material defaultObjectMaterial;
    private Renderer rendererComponent;
    private bool isTranslateMode = true, isRotateMode = false;
    private Vector3 rotationOrigin;
    private float rotationSpeed = 0.001f;
    private SceneManager sceneManager;

    void Start()
    {
        rendererComponent = GetComponent<Renderer>();
        sceneManager = FindObjectOfType<SceneManager>();
    }

    void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMousePositionWorld();
        if (rendererComponent != null)
        {
            rendererComponent.material = selectedObjectMaterial;
        }
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (isTranslateMode)
                {
                    isTranslateMode = false;
                    isRotateMode = true;
                }
                else if (isRotateMode)
                {
                    isTranslateMode = true;
                    isRotateMode = false;
                }
            }
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                sceneManager.instantiatedObjects.Remove(transform);
                Destroy(gameObject);
            }

            if (isTranslateMode)
            {
                Vector3 newPosition = GetMousePositionWorld() + offset;
                transform.position = new Vector3(newPosition.x, transform.position.y, newPosition.z);
            }
            else if (isRotateMode)
            {
                ApplyRotation();
            }
        }
    }

    void OnMouseUp()
    {
        sceneManager.instantiatedObjects.Remove(transform);
        sceneManager.instantiatedObjects.Add(transform);
        isDragging = false;
        if (rendererComponent != null)
        {
            rendererComponent.material = defaultObjectMaterial;
        }
    }

    private Vector3 GetMousePositionWorld()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }

        return transform.position;
    }

    private void ApplyRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject == gameObject)
        {
            rotationOrigin = Input.mousePosition;
        }

        float rotationX = (Input.mousePosition.y - rotationOrigin.y) * rotationSpeed;
        float rotationY = -(Input.mousePosition.x - rotationOrigin.x) * rotationSpeed;

        transform.Rotate(new Vector3(rotationX, rotationY, 0f));
    }
}
