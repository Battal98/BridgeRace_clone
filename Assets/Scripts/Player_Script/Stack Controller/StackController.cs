using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class StackController : MonoBehaviour
{

    [SerializeField]
    private List<Transform> _collectableCollectPathTarget = new List<Transform>(); // Collect Path Targets
    [SerializeField]
    private List<Transform> _collectableSetupPathTarget = new List<Transform>(); // Set up Path Targets

    public List<GameObject> CollectedList;
    public List<Vector3> CollectableRespawnList;

    private Vector3[] _collectableCollectPath = new Vector3[2]; // Path Target Array
    //[HideInInspector]
    public Vector3[] CollectableSetupPath = new Vector3[2]; // Path Target Array

    private void Start()
    {
        #region Set Collectable Collecting & Setup Path 

        for (int i = 0; i < _collectableCollectPathTarget.Count - 1; i++)
        {
            _collectableCollectPath[i] = _collectableCollectPathTarget[i].localPosition;
        }

        for (int i = 0; i < _collectableSetupPathTarget.Count - 1; i++)
        {
            CollectableSetupPath[i] = _collectableSetupPathTarget[i].localPosition;
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
            _collectableCollectPath[0] = _collectableCollectPathTarget[0].localPosition;
        }
        else
        {
            //left
            _collectableCollectPath[0] = _collectableCollectPathTarget[2].localPosition;
        }

        #endregion

        #region Increase Collectable End Value

        _collectableCollectPath[1] += new Vector3(0, 0.2f, 0);

        #endregion

        #region PickUpObject Jobs

        _pickupObject.transform.parent = this.transform;
        _pickupObjCollider.enabled = false;
        _pickupObject.GetComponent<TrailRenderer>().enabled = true;

        #endregion

        #region Collectable Obj Movements

        _pickupObject.transform.DOLocalPath(_collectableCollectPath, 0.2f, PathType.CatmullRom).OnComplete(() =>
        {
            _pickupObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            _pickupObject.gameObject.GetComponent<TrailRenderer>().enabled = false;
        }) ;

        #endregion

    }

    public void ResetPathEndValue()
    {
        _collectableCollectPath[1] -= new Vector3(0, 0.2f, 0);
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
