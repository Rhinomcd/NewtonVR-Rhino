using UnityEngine;
using System.Collections;
using NewtonVR_Rhino;

public class NVRExampleSpawner : MonoBehaviour
{
    public NVRButton Button;

    public GameObject ToCopy;
    public Transform SpawnLocation;

    private void Update()
    {
        if (Button.ButtonDown)
        {
            GameObject newGo = GameObject.Instantiate(ToCopy);
            newGo.transform.position = SpawnLocation.position;
            newGo.transform.localScale = ToCopy.transform.lossyScale;
        }
    }
}
