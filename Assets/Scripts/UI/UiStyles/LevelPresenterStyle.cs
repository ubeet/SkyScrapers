using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Game/UI/Styles/LevelStyle")]
public class LevelPresenterStyle : SerializedScriptableObject
{
    [Serializable]
    public struct LevelPresenterStyleData
    {
        public Sprite Background;
        public Sprite Border;
        public Sprite GlowImage;
        public Color GlowColor;
        public Color BorderColor;
        public Color BackgroundColorColor;
        public bool DoNotUseFallBackDataIfNull;

        public LevelPresenterStyleData GetNotNull(LevelPresenterStyleData from)
        {
            if (from.DoNotUseFallBackDataIfNull) return from;
            
            if (from.Background == null && Background != null) 
                from.Background = this.Background;
            if (from.Border == null && Border != null) 
                from.Border = this.Border;
            if (from.GlowImage == null && GlowImage != null) 
                from.GlowImage = this.GlowImage;
            if (from.GlowColor == default && GlowColor != default) 
                from.GlowColor = this.GlowColor;
            if (from.BorderColor == default && BorderColor != default) 
                from.BorderColor = this.BorderColor;
            if (from.BackgroundColorColor == default && BackgroundColorColor != default) 
                from.BackgroundColorColor = this.BackgroundColorColor;
            return from;
        }
    }

    public LevelPresenterStyleData FallbackStyleData;
}
