using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    [CreateAssetMenu(menuName = RadishMenus.PassAssetMenuPrefix + "Unsupported Shaders Pass", order = RadishMenus.MenuOrder)]
    public sealed class UnsupportedShadersPass : RadishRenderPass<UnsupportedShadersPassData>
    {
        private static ShaderTagId[] s_ShaderTagIds;

        private static Material m_ErrorMaterial;
        
        protected override void SetupPass(in PassContext passContext, in CameraContext cameraContext, ref RenderGraphBuilder builder)
        {
            var cameraPassData = passContext.passManager.GetData<ICameraCommonPassData>();
            
            passContext.data.sceneColor = builder.UseColorBuffer(cameraPassData.sceneColor, 0);
            passContext.data.sceneDepth = builder.UseDepthBuffer(cameraPassData.sceneDepth, DepthAccess.Write);

            if (!m_ErrorMaterial)
                m_ErrorMaterial = CoreUtils.CreateEngineMaterial("Hidden/InternalErrorShader");

            s_ShaderTagIds ??= new ShaderTagId[]
            {
                new("Always"),
                new("ForwardBase"),
                new("PrepassBase"),
                new("Vertex"),
                new("VertexLMRGBM"),
                new("VertexLM")
            };

            var desc = new RendererListDesc(s_ShaderTagIds, cameraContext.cullResults, cameraContext.camera)
            {
                overrideMaterial = m_ErrorMaterial,
                renderQueueRange = RenderQueueRange.all
            };

            passContext.data.renderList = builder.UseRendererList(passContext.renderGraph.CreateRendererList(desc));
            
            builder.AllowRendererListCulling(true);
            
            builder.SetRenderFunc<UnsupportedShadersPassData>(static (data, context) =>
            {
                context.cmd.DrawRendererList(data.renderList);
            });
        }

        protected override bool ShouldCullPass(in PassContext passContext, in CameraContext cameraContext)
        {
            return !Application.isEditor;
        }
    }
}