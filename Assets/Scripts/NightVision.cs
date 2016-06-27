using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/Night Vision")]
public class NightVision : MonoBehaviour
{
    // public data
    public Shader shader;
    public Color luminence;
    public float noiseFactor = 0.005f;
    //private data
    private Material mat;

    //-----------------------------------------
    // start 
    void Start()
    {
        shader = Shader.Find("Image Effects/Night Vision");
        mat = new Material(shader);
        mat.SetVector("lum", new Vector4(luminence.g, luminence.g, luminence.g, luminence.g));
        mat.SetFloat("noiseFactor", noiseFactor);
    }

    //-----------------------------------------
    // Called by camera to apply image effect
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        mat.SetFloat("time", Mathf.Sin(Time.time * Time.deltaTime));
        Graphics.Blit(source, destination, mat);
    }
}