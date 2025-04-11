using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;


public class CellPresenter3D : CellPresenter, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public List<Buildings> buildings;
    [SerializeField] public GameObject building;
    [SerializeField] private SpriteRenderer Cell;
    [SerializeField] private Decorations decorations;
    [SerializeField] private AnimationCurve buildingCurve;
    [SerializeField] private AnimationCurve destroyCurve;
    [SerializeField] private MeshFilter MeshFilter;
    [SerializeField] private Renderer renderer;
    [SerializeField] private CellPresenter3D _cellPresenter3D;
    [SerializeField] private float buildTime;
    [SerializeField] private float destroyTime;
    
    private Controls _inputs;
    private bool _isSelected;
    private Camera _camera;
    private float _timer;
    private HeightInstance _next;
    private HeightInstance _current;
    private int _currentHeight = -1;
    private State _currentState;
    private List<GameObject> _toRemove;
    private int dataCell;
    private float delay;
    private float _state = 1;
    private MaterialPropertyBlock _currentProperty;
    private Renderer _currentRenderer;
    private MaterialPropertyBlock _prevProperty;
    private Renderer _prevRenderer;

    private void Start()
    {
        canvas = GameObject.Find ("Canvas").GetComponent <Canvas>();
        _camera = Camera.main;
    }

    private void OnEnable()
    {
        _inputs.Enable();
    }

    private void OnDisable()
    {
        _inputs.Disable();
    }
    
    private void Awake()
    {
        _currentProperty = new MaterialPropertyBlock();
        _toRemove = new List<GameObject>();
        _inputs = new Controls();
        _currentState = State.None;
        var heightInstance = RootData.Instance.GetByHeight(0);
        heightInstance.Take();

        MeshFilter.sharedMesh = heightInstance.mesh;
        Vector3 size = renderer.bounds.size;
        _currentProperty.SetFloat("_Value", 1f);
        _currentProperty.SetFloat("_Height", Mathf.Round(size.y * 15.6521739f));
        
        renderer.SetPropertyBlock(_currentProperty);
        _currentHeight = 0;
        _current = heightInstance;
    }
    
    public override void Reset()
    {
        for (int i = 2; i < transform.childCount; i++)
            Destroy(transform.GetChild(i).gameObject);

        if(_current != null)
            _current.Release();
    }
    
    
    public override void Set(List<int> dataRuleCell, bool force)
    {
        _heights = dataRuleCell;
        
        
        if (dataRuleCell.Count == 1)
        {
            cellGridPresenter.Clear();
            cellGridPresenter.gameObject.SetActive(false);
            if(dataRuleCell[0] == _currentHeight)
            {
                delay = _state > 0.1f ? 0f : 0.5f;
                _currentState = State.Build;
            }
            else
            {
                
                delay = 0.5f;
                _currentState = State.Destroy;
            }

            
            dataCell = dataRuleCell[0];
        }
        else
        {
            if (dataRuleCell.Count > 1)
            {
                cellGridPresenter.gameObject.SetActive(true);
                cellGridPresenter.SetNumbers(dataRuleCell);
            }
            else
            {
                cellGridPresenter.Clear();
                cellGridPresenter.gameObject.SetActive(false);
            }
            if(_currentHeight != 0)
            {
                delay = 0.5f;
                _currentState = State.Destroy;
            }
            dataCell = 0;
        }
        
        
        
    }

    private void SetMesh(int dataRuleCell)
    {
        var heightInstance = RootData.Instance.GetByHeight(dataRuleCell);
        heightInstance.Take();

        MeshFilter.sharedMesh = heightInstance.mesh;
        Vector3 size = renderer.bounds.size;
        _currentProperty.SetFloat("_Value", 0f);
        _currentProperty.SetFloat("_Height", Mathf.Round(size.y * 15.6521739f));
        
        renderer.SetPropertyBlock(_currentProperty);
        _currentHeight = dataRuleCell;
        _current = heightInstance;
        SetDecorations();
        _currentState = State.Build;
    }

    private void SetDecorations()
    {
        //if (!_current.LeftFront || !_current.RightFront || !_current.LeftRear || !_current.RightRear || !_current.Rear)
        //    return;
        CombineInstance[] combine = new CombineInstance[0];
        var scale = transform.GetChild(0).localScale.x;
        var newGo = new GameObject();
        var go = Instantiate(newGo,
                transform);
        var par = MeshFilter.transform;
        if(_current.LeftFront)
        {
            //var firstDeco = Instantiate(decorations.decorations[Random.Range(0, decorations.decorations.Count)],
            //    transform);
            combine = Add(combine, decorations.decorations[Random.Range(0, decorations.decorations.Count)].GetComponentInChildren<MeshFilter>());
            go.transform.position = new Vector3(1.0215f/scale, par.localPosition.y, 1.0215f/scale);
            combine[^1].transform = go.transform.localToWorldMatrix;
        }
        if(_current.RightFront)
        {
            //var secondDeco = Instantiate(decorations.decorations[Random.Range(0, decorations.decorations.Count)],
            //    transform);
            //secondDeco.transform.localPosition = new Vector3(-1.0215f, 0, 1.0215f);
            combine = Add(combine, decorations.decorations[Random.Range(0, decorations.decorations.Count)].GetComponentInChildren<MeshFilter>());
            go.transform.position = new Vector3(-1.0215f/scale, par.localPosition.y, 1.0215f/scale);
            combine[^1].transform = go.transform.localToWorldMatrix;
        }
        if(_current.LeftRear)
        {
            //var thirdDeco = Instantiate(decorations.decorations[Random.Range(0, decorations.decorations.Count)],
            //    transform);
            //thirdDeco.transform.localPosition = new Vector3(1.0215f, 0, -1.0215f);
            combine = Add(combine, decorations.decorations[Random.Range(0, decorations.decorations.Count)].GetComponentInChildren<MeshFilter>());
            go.transform.position = new Vector3(1.0215f/scale, par.localPosition.y, -1.0215f/scale);
            combine[^1].transform = go.transform.localToWorldMatrix;
        }
        if(_current.RightRear)
        {
            //var fourthDeco = Instantiate(decorations.decorations[Random.Range(0, decorations.decorations.Count)],
            //    transform);
            //fourthDeco.transform.localPosition = new Vector3(-1.0215f, 0, -1.0215f);
            combine = Add(combine, decorations.decorations[Random.Range(0, decorations.decorations.Count)].GetComponentInChildren<MeshFilter>());
            go.transform.position = new Vector3(-1.0215f/scale, par.localPosition.y, -1.0215f/scale);
            combine[^1].transform = go.transform.localToWorldMatrix;
        }
        combine = Add(combine, MeshFilter);
        
        go.transform.position = new Vector3(par.localPosition.x, par.localPosition.y, par.localPosition.z);
        combine[^1].transform = go.transform.localToWorldMatrix;
        
        Destroy(go);
        Destroy(newGo);
        var mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        mesh.CombineMeshes(combine);
        MeshFilter.mesh = mesh;
        building.transform.localRotation = Quaternion.Euler(transform.eulerAngles.x, -transform.eulerAngles.y, transform.eulerAngles.z);

    }

    private CombineInstance[] Add(CombineInstance[] combine, MeshFilter mesh)
    {
        var newCombine = new CombineInstance[combine.Length + 1];
        for (int i = 0; i < combine.Length; i++)
        {
            newCombine[i] = combine[i];
        }
        newCombine[combine.Length].mesh = mesh.sharedMesh;
        
        return newCombine;
    }
    
    public override void OnPointerDown(PointerEventData eventData)
    {
        //Cell.color = new Color(1 / 255f * 100, 1 / 255f * 100, 1 / 255f * 100);
        if (!Mouse.current.leftButton.isPressed || !_isChangeable) return;
        var position = Mouse.current.position.ReadValue();
        var scaleFactor = canvas.scaleFactor;
        Debug.Log(position.x + " " + position.y);
        HeightPickerBehaviour.Instance.Show(new Vector2(position.x , position.y), SetNew, _heights);
    }


    private void Update()
    {
        delay -= Time.deltaTime;
        if (_currentState == State.Destroy)
        {
            if (_state > 0)
            {
                _state -= 2.8f * Time.deltaTime;
                _currentProperty.SetFloat("_Value", destroyCurve.Evaluate(_state));
                renderer.SetPropertyBlock(_currentProperty);
                
            }

            if (_state <= 0)
            {
                _state = 0;
                _currentProperty.SetFloat("_Value", destroyCurve.Evaluate(_state));
                renderer.SetPropertyBlock(_currentProperty);
                Reset();
                SetMesh(dataCell);
            }
        }
        if (_currentState == State.Build && delay <= 0)
        {
            if (_state <= 1)
            {
                _state += 2.8f * Time.deltaTime;
                _currentProperty.SetFloat("_Value", buildingCurve.Evaluate(_state));
                renderer.SetPropertyBlock(_currentProperty);
            }

            if (_state >= 1)
            {
                _state = 1;
                _currentProperty.SetFloat("_Value", destroyCurve.Evaluate(_state));
                renderer.SetPropertyBlock(_currentProperty);
                _currentState = State.None;
            }
        }
        if (_isSelected && _isChangeable)
        {
            if(_inputs.Player.Nums.WasPressedThisFrame())
            {
                int num = Convert.ToInt32(_inputs.Player.Nums.ReadValue<float>());
                if (num >= 0 && num <= _size)
                    SetNew(num);
                
            }
            if (Mouse.current.rightButton.wasPressedThisFrame)
                SetNew(0);
        }
    }
    
    private enum State
    {
        Build,
        Destroy,
        None
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isSelected = true;
        //Cell.color = new Color(1 / 255f * 161, 1 / 255f * 161, 1 / 255f * 161);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isSelected = false;
        //Cell.color = Color.white;
    }
}