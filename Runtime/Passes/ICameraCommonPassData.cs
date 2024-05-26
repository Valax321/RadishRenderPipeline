using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    public interface ICameraCommonPassData : IRadishCommonPassData
    {
        TextureHandle sceneColor { get; }
        TextureHandle sceneDepth { get; }
    }
}