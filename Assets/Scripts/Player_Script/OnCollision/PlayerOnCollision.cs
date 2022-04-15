using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerOnCollision : MonoBehaviour
{
    public static PlayerOnCollision Instance;

    #region Events 

    public event EventHandler<PickUpEvents> PickUp;
    public event EventHandler<SetUpEvents> SetPath;

    #endregion

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Collectables"))
        {
            CollectableController _colController = hit.gameObject.GetComponent<CollectableController>();
            if (_colController._colorState == CollectableController.ColorState.Blue)
            {
                var _pickUpData = new PickUpEvents { PickUpEvent = hit.gameObject };
                PickUp?.Invoke(this, _pickUpData);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Holder"))
        {
            var _setupData = new SetUpEvents { SetPathEvents = other.gameObject };
            SetPath?.Invoke(this, _setupData);
        }
    }
}
