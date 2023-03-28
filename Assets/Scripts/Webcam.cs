using UnityEngine;
using UnityEngine.Serialization;

public class Webcam : MonoBehaviour
{
    [FormerlySerializedAs("_rawImage")] [SerializeField]
    private UnityEngine.UI.RawImage rawImage;

    void Start()
    {
        WebCamDevice[] devices = WebCamTexture.devices;

        // for debugging purposes, prints available devices to the console
        foreach (var t in devices)
        {
            print("Webcam available: " + t.name);
        }

        //Renderer rend = this.GetComponentInChildren<Renderer>();

        // assuming the first available WebCam is desired

        WebCamTexture tex = new WebCamTexture(devices[0].name);
        //rend.material.mainTexture = tex;
        this.rawImage.texture = tex;
        tex.Play();
    }
}
