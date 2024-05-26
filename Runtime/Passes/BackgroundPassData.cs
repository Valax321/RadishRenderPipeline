using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    public sealed class BackgroundPassData : RadishRenderPassData
    {
        public RendererListHandle skyboxRenderers;
    }
}