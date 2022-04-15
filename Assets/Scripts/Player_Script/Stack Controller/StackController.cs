using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class StackController : MonoBehaviour
{

    [SerializeField]
    private List<Transform> _collectablePathTarget = new List<Transform>(); // Path Targets

    public List<GameObject> CollectedList;
    public List<Vector3> CollectableRespawnList;

    private Vector3[] _collectablePath = new Vector3[2]; // Path Target Array


    private void Start()
    {
        #region Set Collectable Collecting Path 

        for (int i = 0; i < _collectablePathTarget.Count - 1; i++)
        {
            _collectablePath[i] = _collectablePathTarget[i].localPosition;
        }

        #endregion

        AddEvents();
    }

    private void OnDisable()
    {
        RemoveEvents();
    }



    private void PickUpFunc(object sender, PickUpEvents e)
    {
        GameObject _pickupObject = e.PickUpEvent;
        Collider _pickupObjCollider = _pickupObject.GetComponent<Collider>();

        #region AddList

        CollectedList.Add(_pickupObject);
        CollectableRespawnList.Add(_pickupObject.transform.position);

        #endregion

        #region Check Path Side

        if (CollectedList.Count % 2 == 0)
        {
            //right
            _collectablePath[0] = _collectablePathTarget[0].localPosition;
        }
        else
        {
            //left
            _collectablePath[0] = _collectablePathTarget[2].localPosition;
        }

        #endregion

        #region Increase Collectable End Value

        _collectablePath[1] += new Vector3(0, 0.2f, 0);

        #endregion

        #region PickUpObject Jobs

        _pickupObject.transform.parent = this.transform;
        _pickupObjCollider.enabled = false;
        _pickupObject.GetComponent<TrailRenderer>().enabled = true;

        #endregion

        #region Collectable Obj Movements

        _pickupObject.transform.DOLocalPath(_collectablePath, 0.2f, PathType.CatmullRom).OnComplete(() => _pickupObject.transform.localRotation = Quaternion.Euler(Vector3.zero)) ;

        #endregion

    }

    public void ResetPathEndValue()
    {
        _collectablePath[1] -= new Vector3(0, 0.2f, 0);
    }

    private void AddEvents()
    {
        PlayerOnCollision.Instance.PickUp += PickUpFunc;
    }

    private void RemoveEvents()
    {
        PlayerOnCollision.Instance.PickUp -= PickUpFunc;
    }
}
