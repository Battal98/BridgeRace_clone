using UnityEngine;

public class CollectableController : MonoBehaviour
{
    [SerializeField]
    private Color Blue, Red, Green;

    private MeshRenderer m_Renderer;
    private TrailRenderer m_TrailRenderer;

    #region Color States
    public enum ColorState
    {
        None, Blue, Red, Green
    }

    public ColorState ColorStates;

    #endregion

    public void SetColors()
    {
        m_Renderer = this.gameObject.GetComponentInChildren<MeshRenderer>();
        m_TrailRenderer = this.gameObject.GetComponent<TrailRenderer>();

        switch (ColorStates)
        {
            case ColorState.None:
                Debug.LogWarning("Error BYP. \n Assign Color state.");
                break;
            case ColorState.Blue:
                m_Renderer.material.color = Blue;
                m_TrailRenderer.material.color = Blue;
                m_TrailRenderer.material.SetColor("_EmissionColor", Blue);
                break;
            case ColorState.Red:
                m_Renderer.material.color = Red;
                m_TrailRenderer.material.color = Red;
                m_TrailRenderer.material.SetColor("_EmissionColor", Red);
                break;
            case ColorState.Green:
                m_Renderer.material.color = Green;
                m_TrailRenderer.material.color = Green;
                m_TrailRenderer.material.SetColor("_EmissionColor", Green);
                break;
        }
    }
}
