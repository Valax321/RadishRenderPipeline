using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering
{
    public abstract class RadishRenderPass<TPassData> : RadishRenderPassBase 
        where TPassData : class, new()
    {
        protected struct PassContext
        {
            public TPassData data;
            public RenderPassManager passManager;
            public RenderGraph renderGraph;
            public RadishRenderPipeline pipeline;
        }
        
        [SerializeField] private string m_PassName;

        private ProfilingSampler m_ProfilingSampler;

        private void OnEnable()
        {
            m_ProfilingSampler = new ProfilingSampler(m_PassName);
        }

        [PublicAPI]
        public override void AddToGraph(RadishRenderPipeline pipeline, RenderGraph renderGraph, RenderPassManager passManager, in CameraContext cameraContext)
        {
            var passContext = new PassContext
            {
                passManager = passManager,
                pipeline = pipeline,
                renderGraph = renderGraph
            };

            if (ShouldCullPass(in passContext, in cameraContext))
                return;
            
            var builder = renderGraph.AddRenderPass(m_PassName, out passContext.data, m_ProfilingSampler);
            try
            {
                SetupPass(in passContext, in cameraContext, ref builder);
                passManager.CollectDataInterfacesForData(passContext.data);
            }
            finally
            {
                builder.Dispose();
            }
        }

        [PublicAPI]
        protected abstract void SetupPass(in PassContext passContext, in CameraContext cameraContext, ref RenderGraphBuilder builder);

        [PublicAPI]
        protected virtual bool ShouldCullPass(in PassContext passContext, in CameraContext cameraContext)
        {
            return false;
        }
    }
}