using System;
using Radish.Rendering.Passes;
using UnityEngine;

namespace Radish.Rendering
{
    [Serializable]
    public sealed class RequiredPipelinePasses
    {
        [SerializeField] private BackgroundPass m_BackgroundPass;
        public BackgroundPass backgroundPass => m_BackgroundPass;

        [SerializeField] private GizmosPass m_GizmosPass;
        public GizmosPass gizmosPass => m_GizmosPass;

        [SerializeField] private CameraBlitPass m_CameraBlitPass;
        public CameraBlitPass cameraBlitPass => m_CameraBlitPass;
    }
}