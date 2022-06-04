using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerOnCollision : MonoBehaviour
{
    public static PlayerOnCollision Instance;

    #region Events 

    public event EventHandler<PickUpEvents> PickUp;

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
            if (_colController.ColorStates == CollectableController.ColorState.Blue)
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
            BridgeController _bridgeController = other.gameObject.transform.GetComponentInParent<BridgeController>();
            if (_bridgeController)
            {
                _bridgeController.SetupBridgePath();
            }
        }
    }
}
