using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SceneManager : MonoBehaviour
{
    private List<FinalSceneState> finalSceneState;
    public int totalObjects = 0;
    public List<Transform> instantiatedObjects;
    public GameObject cubePrefab;
    public GameObject spherePrefab;
    private string filePath;

    void Start()
    {
        finalSceneState = new List<FinalSceneState>();
        instantiatedObjects = new List<Transform>();
        filePath = Path.Combine(Application.persistentDataPath, "ObjectsTransform.json");

        string loadedJson = File.ReadAllText(filePath);
        List<FinalSceneState> savedFinalSceneState = JsonConvert.DeserializeObject<List<FinalSceneState>>(loadedJson);
        int numberOfObjects = PlayerPrefs.GetInt("NumberOfObjects");
        Debug.Log("Number of Objects: " + numberOfObjects);

        GameObject newPrefab = null;
        foreach (FinalSceneState obj in savedFinalSceneState)
        {
            if (obj.objectName == "Cube")
            {
                newPrefab = Instantiate(cubePrefab);
            }
            else if (obj.objectName == "Sphere")
            {
                newPrefab = Instantiate(spherePrefab);
            }
            newPrefab.transform.position = new Vector3(obj.positionOfObject[0], obj.positionOfObject[1], obj.positionOfObject[2]);
            newPrefab.transform.eulerAngles = new Vector3(obj.rotationOfObject[0], obj.rotationOfObject[1], obj.rotationOfObject[2]);
            newPrefab.transform.localScale = new Vector3(obj.scaleOfObject[0], obj.scaleOfObject[1], obj.scaleOfObject[2]);

            instantiatedObjects.Add(newPrefab.transform);
        }
    }

    private void OnApplicationQuit()
    {
        foreach (Transform obj in instantiatedObjects)
        {
            totalObjects++;
            FinalSceneState newFinalSceneState = new FinalSceneState();
            newFinalSceneState.objectName = obj.name.Replace("(Clone)", "");
            newFinalSceneState.positionOfObject[0] = obj.position.x;
            newFinalSceneState.positionOfObject[1] = obj.position.y;
            newFinalSceneState.positionOfObject[2] = obj.position.z;
            newFinalSceneState.rotationOfObject[0] = obj.eulerAngles.x;
            newFinalSceneState.rotationOfObject[1] = obj.eulerAngles.y;
            newFinalSceneState.rotationOfObject[2] = obj.eulerAngles.z;
            newFinalSceneState.scaleOfObject[0] = obj.localScale.x;
            newFinalSceneState.scaleOfObject[1] = obj.localScale.y;
            newFinalSceneState.scaleOfObject[2] = obj.localScale.z;

            finalSceneState.Add(newFinalSceneState);
        }

        PlayerPrefs.SetInt("NumberOfObjects", totalObjects);

        string json = JsonConvert.SerializeObject(finalSceneState);
        File.WriteAllText(filePath, json);
    }
}
