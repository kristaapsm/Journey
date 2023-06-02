using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;


public class Portal : Interactable
{
    public Vector3 teleportPosition = new Vector3(-14.4202747f, 7.29707289f, 2.78382111f);

    protected override void Interact()
    {
        // Teleport the object that is pressed to the specified position
        transform.position = teleportPosition;
    }
}
