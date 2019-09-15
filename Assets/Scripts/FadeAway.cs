using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{

    private Color alphaColor;
    public float timeToFade;
    private Material mat;

    public void Start() {
        mat = GetComponent<MeshRenderer>().material;
        alphaColor = mat.color;
        alphaColor.a = 0;
    }
    public void Update() {
        mat.color = Color.Lerp(mat.color, alphaColor, timeToFade * Time.deltaTime);
    }
}
