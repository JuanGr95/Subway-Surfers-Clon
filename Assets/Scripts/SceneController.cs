using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Transform parentTransform = other.gameObject.transform.parent;
        CharacterController characterController = parentTransform.GetComponent<CharacterController>();
        characterController.enabled = false;
        parentTransform.position = new Vector3(parentTransform.position.x, parentTransform.position.y, 0f);
        characterController.enabled = true;
    }
}

