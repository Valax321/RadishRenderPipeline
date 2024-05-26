using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    public sealed class GizmosPassData : RadishRenderPassData
    {
        public RendererListHandle gizmosRendererList;
    }
}