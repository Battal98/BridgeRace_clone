using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    [SerializeField]
    private GameObject Collectables;

    [SerializeField]
    private List<GameObject> Spawners = new List<GameObject>();

    #region Values: For Set Colors

    [SerializeField]
    private int RedCount, BlueCount, GreenCount;

    [SerializeField]
    private List<GameObject> _createdCollectables = new List<GameObject>(); 
    #endregion


    private void Awake()
    {
        if (instance == null)
            instance = this;

    }

    private void Start()
    {
        #region Spawn Collectables

        #region Instantiate Collectable Objects

        for (int i = 0; i < Spawners.Count; i++)
        {


            GameObject _collectableObj = Instantiate(Collectables, Spawners[i].transform);
            _createdCollectables.Add(_collectableObj);
            _collectableObj.transform.localScale = Vector3.zero;
            _collectableObj.transform.localPosition = new Vector3(0, _collectableObj.transform.localPosition.y, 0);
            _collectableObj.transform.DOScale(Vector3.one, 0.5f);


        }
        #endregion

        #region Set Colors

        while (_createdCollectables.Count > 0 )
        {
            int _randomCollectableCount = Random.Range(0, _createdCollectables.Count);

            if (RedCount > 0)
            {
                SetColorRandomize(CollectableController.ColorState.Red, _randomCollectableCount);
                RedCount--;
            }
            else if (BlueCount > 0)
            {
                SetColorRandomize(CollectableController.ColorState.Blue, _randomCollectableCount);

                BlueCount--;
            }
            else if (GreenCount > 0)
            {
                SetColorRandomize(CollectableController.ColorState.Green, _randomCollectableCount);
                GreenCount--;
            }
            else
                Debug.Log("Done");
        }

        #endregion

        #endregion
    }


    /// <summary>
    /// Set Color in Random spawners for collectable
    /// </summary>
    /// <param name="_colorState"> Collectable Color State </param>
    /// <param name="_randomColCount"> Count of object to be adjusted </param>
    private void SetColorRandomize(CollectableController.ColorState _colorState, int _randomColCount)
    {

        CollectableController _collectableController = _createdCollectables[_randomColCount].gameObject.GetComponent<CollectableController>();
        _collectableController._colorState = _colorState;
        _collectableController.SetColors();

        if (_colorState != CollectableController.ColorState.Blue)
        {
            _createdCollectables[_randomColCount].GetComponent<Collider>().isTrigger = true;
        }

        _createdCollectables.RemoveAt(_randomColCount);
    }

}
