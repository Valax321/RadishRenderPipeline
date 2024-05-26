using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering
{
    public abstract class RadishRenderPassBase : ScriptableObject
    {
        public abstract void AddToGraph(RadishRenderPipeline pipeline, RenderGraph renderGraph, RenderPassManager passManager,
            in CameraContext cameraContext);
    }
}