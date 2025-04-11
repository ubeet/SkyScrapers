using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class FireworkPresenter : MonoBehaviour
{
    [SerializeField] private List<Texture2D> Texture2D;
    [SerializeField] private VisualEffect Icons;
    [SerializeField] private VisualEffect NoIcons;
    [SerializeField] private GameObject Followed;
    [SerializeField] private float time;

    private float _timer;
    private int _sizeX;
    private int _sizeY;

    private void Awake()
    {
        _timer = time;
        _sizeX = RootData.Instance.CurrentStage.Width;
        _sizeY = RootData.Instance.CurrentStage.Height;
        Icons.SetFloat("SizeX", _sizeX * 3);
        NoIcons.SetFloat("SizeX", _sizeX * 3);
        Icons.SetFloat("SizeY", _sizeY * 3);
        NoIcons.SetFloat("SizeY", _sizeY * 3);
        Icons.Stop();
        NoIcons.Stop();
    }


    private void FixedUpdate()
    {
        if(transform.position != Followed.transform.position)
            transform.position = Followed.transform.position;
        _timer -= Time.fixedDeltaTime;
        if (_timer <= 0)
        {
            Icons.SetTexture("t1", Texture2D[0]);
            Texture2D.Add(Texture2D[0]);
            Texture2D.RemoveAt(0);
            _timer = time;
        }
        
    }

    public void Play()
    {
        Icons.gameObject.SetActive(true);
        NoIcons.gameObject.SetActive(true);
        Icons.Play();
        NoIcons.Play();
    }
}