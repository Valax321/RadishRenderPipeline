using Radish.Rendering.Passes;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering
{
    public sealed class RadishRenderPipeline : RenderPipeline
    {
        public static bool isSRGB => QualitySettings.activeColorSpace == ColorSpace.Linear;
        
        public RadishRenderPipelineAsset asset { get; }

        private RenderGraph m_RenderGraph;
        private RenderPassManager m_RenderPassManager;
        
        internal RadishRenderPipeline(RadishRenderPipelineAsset asset)
        {
            this.asset = asset;

            GraphicsSettings.useScriptableRenderPipelineBatching = true;
            GraphicsSettings.lightsUseLinearIntensity = true;
            GraphicsSettings.lightsUseColorTemperature = true;
        }

        private void InitializeRenderGraph()
        {
            m_RenderGraph ??= new RenderGraph("RadishPipeline");
            m_RenderPassManager ??= new RenderPassManager(this, m_RenderGraph);
        }

        private void CleanupRenderGraph()
        {
            m_RenderPassManager = null;
            m_RenderGraph?.Cleanup();
            m_RenderGraph = null;
        }
        
        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            BeginFrameRendering(context, cameras);
            InitializeRenderGraph();

            foreach (var camera in cameras)
            {
                RenderCamera(camera, context);
            }
            
            m_RenderGraph.EndFrame();
            EndFrameRendering(context, cameras);
        }

        private void RenderCamera(Camera camera, ScriptableRenderContext context)
        {
            BeginCameraRendering(context, camera);

            if (!camera.TryGetCullingParameters(out var cullParams))
                return;
            var cullResults = context.Cull(ref cullParams);
            
            context.SetupCameraProperties(camera);

            var sampler = new ProfilingSampler(camera.name);

            var buffer = CommandBufferPool.Get(camera.name);
            var graphParams = new RenderGraphParameters
            {
                scriptableRenderContext = context,
                commandBuffer = buffer,
                currentFrameIndex = Time.frameCount,
                executionName = sampler.name,
                rendererListCulling = true
            };
            
            m_RenderGraph.BeginRecording(in graphParams);
            {
                using var _ = new RenderGraphProfilingScope(m_RenderGraph, sampler);
                RecordRenderPassesForCamera(new CameraContext(camera, in cullResults));
            }
            m_RenderGraph.EndRecordingAndExecute();
            
            context.ExecuteCommandBuffer(buffer);
            CommandBufferPool.Release(buffer);
            
            context.Submit();
            EndCameraRendering(context, camera);
        }

        private void RecordRenderPassesForCamera(in CameraContext cameraContext)
        {
            m_RenderPassManager.BeginCameraFrame();
            
            // Setup frame inputs
            var inputs = new CameraPassGlobalData
            {
                sceneColor = m_RenderGraph.CreateTexture(CreateColorTextureDesc(cameraContext.camera, "Color")),
                sceneDepth = m_RenderGraph.CreateTexture(CreateDepthTextureDesc(cameraContext.camera))
            };
            m_RenderPassManager.CollectDataInterfacesForData(inputs);

            // Render the passes in order
            foreach (var pass in asset.passes)
            {
                pass.AddToGraph(this, m_RenderGraph, m_RenderPassManager, in cameraContext);
            }
            
            m_RenderPassManager.EndCameraFrame();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            CleanupRenderGraph();
        }
        
        private static TextureDesc CreateColorTextureDesc(Camera camera, string name)
        {
            return new TextureDesc(camera.pixelWidth, camera.pixelHeight, true)
            {
                colorFormat = GraphicsFormatUtility.GetGraphicsFormat(RenderTextureFormat.DefaultHDR, isSRGB),
                clearBuffer = true,
                depthBufferBits = DepthBits.None,
                clearColor = isSRGB ? camera.backgroundColor.linear : camera.backgroundColor,
                name = name
            };
        }

        private static TextureDesc CreateDepthTextureDesc(Camera camera)
        {
            return new TextureDesc(camera.pixelWidth, camera.pixelHeight, true)
            {
                colorFormat = GraphicsFormatUtility.GetGraphicsFormat(RenderTextureFormat.Depth, isSRGB),
                clearBuffer = true,
                clearColor = Color.black,
                depthBufferBits = DepthBits.Depth24,
                name = "Depth"
            };
        }
    }
}
