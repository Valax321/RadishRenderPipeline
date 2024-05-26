using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    public sealed class UIOverlayPassData : RadishRenderPassData
    {
        public RendererListHandle uiRenderList;
    }
}