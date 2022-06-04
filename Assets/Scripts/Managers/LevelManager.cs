using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    #region SpawnSystem

    [SerializeField]
    private GameObject Collectables;

    [System.Serializable]
    public class SpawnSystem
    {
        public List<GameObject> Spawners = new List<GameObject>();
    }

    public List<SpawnSystem> SpawnSystems;

    #endregion

    #region Values: For Set Colors

    [SerializeField]
    private int RedCount, BlueCount, GreenCount;

    private int defRedCount, defBlueCount, defGreenCount;

    [SerializeField]
    public List<GameObject> _createdCollectables = new List<GameObject>();
    #endregion

    #region Values: Wave System Level Managment

    public int MaxWaveCount = 1;
    public int currentWaveCount = 0;
    public int PoolSize = 10;

    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    private void Start()
    {

        Init();

        #region Spawn Collectables

        SpawnCollectables();

        #endregion
    }

    private void Init()
    {
        defRedCount = RedCount;
        defGreenCount = GreenCount;
        defBlueCount = BlueCount;
    }

    /// <summary>
    /// Set Color in Random spawners for collectable
    /// </summary>
    /// <param name="_colorState"> Collectable Color State </param>
    /// <param name="_randomColCount"> Count of object to be adjusted </param>
    private void SetColorRandomize(CollectableController.ColorState _colorState, int _randomColCount)
    {

        CollectableController _collectableController = _createdCollectables[_randomColCount].gameObject.GetComponent<CollectableController>();
        _collectableController.ColorStates = _colorState;
        _collectableController.SetColors();

        if (_colorState != CollectableController.ColorState.Blue)
        {
            _createdCollectables[_randomColCount].GetComponent<Collider>().isTrigger = true;
        }

        _createdCollectables.RemoveAt(_randomColCount);
    }

    public void SpawnCollectables()
    {
        #region Instantiate Collectable Objects

        if (SpawnSystems[currentWaveCount] != null)
        {
            for (int i = 0; i < SpawnSystems[currentWaveCount].Spawners.Count; i++)
            {
                GameObject _collectableObj = Instantiate(Collectables, SpawnSystems[currentWaveCount].Spawners[i].transform);
                _createdCollectables.Add(_collectableObj);
                _collectableObj.transform.localScale = Vector3.zero;
                _collectableObj.transform.localPosition = new Vector3(0, _collectableObj.transform.localPosition.y, 0);
                _collectableObj.transform.DOScale(Vector3.one, 0.5f);
            }
        }

        #endregion

        #region Set Colors

        if (SpawnSystems[currentWaveCount] != null)
        {
            while (_createdCollectables.Count > 0)
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
                {
                    RedCount = defRedCount;
                    GreenCount = defGreenCount;
                    BlueCount = defBlueCount;
                    Debug.Log("Done 1");
                }
            }

            RedCount = defRedCount;
            GreenCount = defGreenCount;
            BlueCount = defBlueCount;
            Debug.Log("Done 2");
        }

        #endregion

    }

}
