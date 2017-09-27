using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetableObject : MonoBehaviour
{
    public GameObject myMesh;
    Shader originalShader;
    Renderer rend;
    Shader outlineShader;
    
    void Start()
    {
        rend = myMesh.GetComponent<Renderer>();
        originalShader = rend.material.shader;
        outlineShader = Shader.Find("Custom/Outline");
    }

    private void OnEnable()
    {
        rend = myMesh.GetComponent<Renderer>();
        originalShader = rend.material.shader;
        outlineShader = Shader.Find("Custom/Outline");
    }

    public void Targeted()
    {
        rend.material.shader = outlineShader;
    }

    public void NotTargeted()
    {
        rend.material.shader = originalShader;
    }
}
