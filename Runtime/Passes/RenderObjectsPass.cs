using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RendererUtils;
using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    [CreateAssetMenu(menuName = RadishMenus.PassAssetMenuPrefix + "Render Objects Pass", order = RadishMenus.MenuOrder)]
    public sealed class RenderObjectsPass : RadishRenderPass<RenderObjectsPassData>
    {
        [SerializeField] private List<string> m_ShaderTags = new();
        private ShaderTagId[] m_ShaderTagIds;

        [SerializeField] private SortingCriteria m_SortingCriteria = SortingCriteria.CommonOpaque;
        [SerializeField] private SerializedRenderQueueRange m_RenderQueueRange = new(0, 2500);
        
        protected override void SetupPass(in PassContext passContext, in CameraContext cameraContext, ref RenderGraphBuilder builder)
        {
            var cameraPassData = passContext.passManager.GetData<ICameraCommonPassData>();
            
            passContext.data.sceneColor = builder.UseColorBuffer(cameraPassData.sceneColor, 0);
            passContext.data.sceneDepth = builder.UseDepthBuffer(cameraPassData.sceneDepth, DepthAccess.Write);

            var renderListDesc = new RendererListDesc(m_ShaderTags.Select(x => new ShaderTagId(x)).ToArray(), cameraContext.cullResults, cameraContext.camera)
            {
                sortingCriteria = m_SortingCriteria,
                renderQueueRange = m_RenderQueueRange.value
            };

            passContext.data.renderObjects = builder.UseRendererList(
                passContext.renderGraph.CreateRendererList(renderListDesc));
            
            builder.AllowRendererListCulling(true);
            
            builder.SetRenderFunc<RenderObjectsPassData>(static (data, context) =>
            {
                context.cmd.DrawRendererList(data.renderObjects);
            });
        }

        public void OnEnable()
        {
            m_ShaderTagIds = new ShaderTagId[m_ShaderTags.Count];
            for (var i = 0; i < m_ShaderTags.Count; ++i)
                m_ShaderTagIds[i] = new ShaderTagId(m_ShaderTags[i]);
        }
    }
}