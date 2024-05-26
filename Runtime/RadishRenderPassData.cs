using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering
{
    public abstract class RadishRenderPassData
    {
        public TextureHandle sceneColor;
        public TextureHandle sceneDepth;
    }
}