using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UI
{
    [Serializable]
    public class NewButton : Button
    {
        [SerializeField, Required] private TMP_Text buttonText;
        [SerializeField, Required] private bool useLockImage = true;
        [SerializeField, Required] private Image lockImage;
        [SerializeField, Required] private bool applyGrayUIOnTargetGraphics;
        [Header("Audio")]
        [SerializeField] private bool useSounds = true;
        [ShowIf(nameof(useSounds))]
        [SerializeField] private bool useStandardSound = true;

        [SerializeField] public List<Image> graphicsToTint = new ();
        
        [FoldoutGroup("AltTint",Expanded = true)]
        [SerializeField] private bool useAltTint = false;
        [ShowIf(nameof(useAltTint))] [FoldoutGroup("AltTint")]
        [SerializeField] public ColorBlock altTingColorBlock = new ();
        [ShowIf(nameof(useAltTint))] [FoldoutGroup("AltTint")]
        [SerializeField, Required] public List<Image> graphicsToAltTint = new ();
        [Space]
        [SerializeField] public bool selectOnStart = false;
        [Space]
        [SerializeField, ReadOnly] private bool _locked = false;
        
        [ShowInInspector] public bool Locked { get => _locked; set => SetLock(value); }

        public UnityEvent OnSelectEvent;
        public UnityEvent OnDeselectEvent;
        
        public Action OnSelection;
        public Action OnDeselection;
        
        private bool _thisSelected;
        
        [ShowInInspector] public bool Selected
        {
            get => currentSelectionState == SelectionState.Selected;
            set
            {
                if(value)
                {
                    //if (currentSelectionState != SelectionState.Selected)
                        Select();
                }
                else
                    if(EventSystem.current.currentSelectedGameObject == this.gameObject)
                        EventSystem.current.SetSelectedGameObject(null, new BaseEventData(EventSystem.current));
            }
        }
        
        public void SetLock(bool state)
        {
            _locked = state;
            if (_locked)
                Lock();
            else
                Unlock();
        }

        public void Lock() => Lock(useLockImage);

        public void Lock(bool showLockImage)
        {
            _locked = true;
            SetTargetGraphicsMaterial(applyGrayUIOnTargetGraphics ? UIStyleLib.Instance.GrayScaleUIMaterial : null);

            if (lockImage && showLockImage) lockImage.enabled = true;
            DisableInteract();
        }


        public void Unlock(bool setInteractable = false)
        {
            _locked = false;
            SetTargetGraphicsMaterial(null);
            if (lockImage) lockImage.enabled = false;
            SetButtonInteraction(setInteractable);
        }

        private void SetTargetGraphicsMaterial(Material newMaterial)
        {
            if(targetGraphic != null)
                targetGraphic.material = newMaterial;
            for (var i = 0; i < graphicsToTint.Count; i++)
            {
                var item = graphicsToTint[i];
                if(item == null) continue;
                item.material = newMaterial;
            }
        }


        public void EnableInteract() => Unlock(true);
        public void DisableInteract() => SetButtonInteraction(false);
        public void SetInteract(bool state) => SetButtonInteraction(state);

        private void SetButtonInteraction(bool state)
        {
            if (state && _locked)
                SetLock(false);
            interactable = state;
            if (applyGrayUIOnTargetGraphics && targetGraphic != null)
            {
                targetGraphic.material = state? null : UIStyleLib.Instance.GrayScaleUIMaterial ;
            }
        }



        private void Update()
        {
            if (!Application.isPlaying) return;
        }
        
        protected override void DoStateTransition(SelectionState state, bool instant)
        {
            if(graphicsToTint.IsValid())
            {
                Color tintColor = state switch
                {
                    SelectionState.Normal => colors.normalColor,
                    SelectionState.Highlighted => colors.highlightedColor,
                    SelectionState.Pressed => colors.pressedColor,
                    SelectionState.Selected => colors.selectedColor,
                    SelectionState.Disabled => colors.disabledColor,
                    _ => Color.black
                };
                for (var i = 0; i < graphicsToTint.Count; i++)
                {
                    var item = graphicsToTint[i];
                    if(item == null) continue;
                    item.CrossFadeColor(tintColor, instant ? 0f : colors.fadeDuration, true, true);
                }
            }

            if (useAltTint && graphicsToAltTint.IsValid())
            {
                Color altTintColor = state switch
                {
                    SelectionState.Normal => altTingColorBlock.normalColor,
                    SelectionState.Highlighted => altTingColorBlock.highlightedColor,
                    SelectionState.Pressed => altTingColorBlock.pressedColor,
                    SelectionState.Selected => altTingColorBlock.selectedColor,
                    SelectionState.Disabled => altTingColorBlock.disabledColor,
                    _ => Color.black
                };
                for (var i = 0; i < graphicsToAltTint.Count; i++)
                {
                    var item = graphicsToAltTint[i];
                    if(item == null) continue;
                    item.CrossFadeColor(altTintColor, instant ? 0f : altTingColorBlock.fadeDuration, true, true);
                }
            }

            base.DoStateTransition(state, instant);
        }

        public void SetText(string textForTheButton)
        {
            if(buttonText == null) return;
            buttonText.SetText(textForTheButton);
        }
        
        public override void OnSelect(BaseEventData eventData)
        {
            _thisSelected = true;
            base.OnSelect(eventData);
            OnSelection?.Invoke();
            OnSelectEvent?.Invoke();
        }

        public override void OnDeselect(BaseEventData eventData)
        {
            _thisSelected = false;
            base.OnDeselect(eventData);
            OnDeselection?.Invoke();
            OnDeselectEvent?.Invoke();
        }

        public void TryInvokeOnClick()
        {
            if (this != null && gameObject.activeInHierarchy && interactable) onClick?.Invoke();
        }

        protected override void Start()
        {
            base.Start();
            if (selectOnStart)
                Select();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            DoStateTransition(currentSelectionState, true);
            if (_locked)
                Lock(true);
            else
                Unlock(interactable);
        }
#endif
    }
}
