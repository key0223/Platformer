using UnityEngine;

public class MiniMapPanel : MonoBehaviour
{
    [SerializeField] BlockController _currentMiniMap;

    GameObject _canvasGO;

    public GameObject CanvasGO { get { return _canvasGO; } }

    private void Awake()
    {
        _canvasGO = GetComponentInChildren<Canvas>().gameObject;
        _canvasGO.SetActive(false);
    }
    
    public void UpdateVisitedBlock()
    {
        _currentMiniMap.SetVisitedBlockAlpha();
    }
}
