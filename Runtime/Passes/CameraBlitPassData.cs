using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    public sealed class CameraBlitPassData : RadishRenderPassData
    {
        public TextureHandle cameraTarget;
    }
}