using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    public sealed class CameraPassGlobalData : ICameraCommonPassData
    {
        public TextureHandle sceneColor { get; set; }
        public TextureHandle sceneDepth { get; set; }
    }
}