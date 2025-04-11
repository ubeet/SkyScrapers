using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/Singletons/UIStyleLib")]
public class UIStyleLib : SerializedScriptableObject
{
    
#region Singletone

    public static bool HasInstance => _instance != null;
    private static UIStyleLib _instance;
    public static UIStyleLib Instance
    {
        get
        {
            if (_instance != null) return _instance;
#if UNITY_EDITOR
            _instance = UnityEditor.AssetDatabase.LoadAssetAtPath<UIStyleLib>("UIStyleLib/UIStyleLib");
            if (_instance == null)
#endif
                _instance = UnityEngine.Resources.Load<UIStyleLib>("UIStyleLib/UIStyleLib");
            if (_instance != null) return _instance;
            Debug.LogError("UIStyleLib doesn't found"); return null;
        }
    }
#endregion

    public Material StandartUnderlayedMaterial;
    public Material StandartDefaultMaterial;
    
    public Shader GrayScale;
    public Material GrayScaleMaterial;
    
    public Shader GrayScaleUI;
    public Material GrayScaleUIMaterial;

    [SerializeField] public Sprite[] completeBorders;
    public LevelPresenterStyle.LevelPresenterStyleData FallbackStyleData;

}
