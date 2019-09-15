using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{

    private Color alphaColor;
    public float timeToFade;
    public float timeBeforeFade;
    private Material mat;

    public void Start() {
        mat = GetComponent<MeshRenderer>().material;
        alphaColor = mat.color;
        alphaColor.a = 0;
    }

    public void Update() {
        if (timeBeforeFade > 0) {
            timeBeforeFade -= Time.deltaTime;
        } else {
            mat.color = Color.Lerp(mat.color, alphaColor, timeToFade * Time.deltaTime);
            if (mat.color.a <= 0) Destroy(gameObject);
        }
    }
}
