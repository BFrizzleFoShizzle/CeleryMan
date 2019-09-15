using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Animator anim;
    public PlayerController pController;

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("running", pController.moving);
    }
}
