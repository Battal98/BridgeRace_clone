using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BridgeController : MonoBehaviour
{
    #region Pool Variables

    [SerializeField]
    private GameObject _creatableObj;

    [SerializeField]
    private int _poolSize = 10;

    [SerializeField]
    private List<GameObject> _poolList;

    [SerializeField]
    private Transform _startPos;

    [SerializeField]
    private Transform _pathHolder;

    #endregion

    [SerializeField]
    private GameObject _playerHolder;

    private void Start()
    {
        #region Walking Path Pool

        PathPool();

        #endregion

        AddEvents();
    }
    private void OnDisable()
    {
        RemoveEvents();
    }

    private void SetPathFunc(object sender, SetUpEvents e)
    {
        StackController _stackControl = PlayerOnCollision.Instance.gameObject.GetComponent<StackController>();

        #region Set Decrease and Increase Values 

        var _decreaseCount = _stackControl.CollectedList.Count - 1;
        var _increaseCount = 0;

        #endregion

        if (_stackControl.CollectedList.Count > 0 && _poolList.Count > 0)
        {
            if (_decreaseCount >= 0 && _decreaseCount <= _stackControl.CollectedList.Count -1 )
            {
                #region Player Stack Decrease

                _stackControl.CollectedList[_decreaseCount].transform.position = _stackControl.CollectableRespawnList[_decreaseCount];
                _stackControl.CollectedList[_decreaseCount].transform.parent = null;
                _stackControl.CollectedList[_decreaseCount].GetComponent<BoxCollider>().enabled = true;
                _stackControl.CollectedList[_decreaseCount].GetComponent<TrailRenderer>().enabled = false;
                _stackControl.ResetPathEndValue();
                _stackControl.CollectableRespawnList.RemoveAt(_decreaseCount);
                _stackControl.CollectedList.RemoveAt(_decreaseCount);

                #endregion

                #region Open Path 

                _poolList[_increaseCount].SetActive(true);
                _poolList.RemoveAt(_increaseCount);

                #endregion

                #region Decrease Jobs

                _decreaseCount--;

                #endregion

                #region Player Holder Movements

                _playerHolder.transform.position = new Vector3(_playerHolder.transform.position.x, _playerHolder.transform.position.y, _playerHolder.transform.position.z + 2.5f);

                #endregion

            }
        }
    }

    private void PathPool()
    {

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject _createdObj = Instantiate(_creatableObj);
            _createdObj.transform.localPosition = _startPos.position;
            _createdObj.SetActive(false);
            _createdObj.transform.parent = _pathHolder;
            _poolList.Add(_createdObj);
        }

        for (int i = 1; i < _poolList.Count; i++)
        {
            _poolList[i].transform.position = new Vector3(0, 0, _poolList[i - 1].transform.position.z + 2.5f);
        }

    }

    private void AddEvents()
    {
        PlayerOnCollision.Instance.SetPath += SetPathFunc;
    }


    private void RemoveEvents()
    {
        PlayerOnCollision.Instance.SetPath -= SetPathFunc;
    }
}
