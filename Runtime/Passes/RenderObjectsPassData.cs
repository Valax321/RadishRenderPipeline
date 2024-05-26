using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    public sealed class RenderObjectsPassData : RadishRenderPassData
    {
        public RendererListHandle renderObjects;
    }
}