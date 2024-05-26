using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    [CreateAssetMenu(menuName = RadishMenus.PassAssetMenuPrefix + "Gizmos Pass", order = RadishMenus.MenuOrder)]
    public sealed class GizmosPass : RadishRenderPass<GizmosPassData>
    {
        [SerializeField] private GizmoSubset m_GizmoSubset = GizmoSubset.PreImageEffects;
        
        protected override void SetupPass(in PassContext passContext, in CameraContext cameraContext, ref RenderGraphBuilder builder)
        {
            var cameraPassData = passContext.passManager.GetData<ICameraCommonPassData>();
            
            passContext.data.sceneColor = builder.UseColorBuffer(cameraPassData.sceneColor, 0);
            passContext.data.sceneDepth = builder.UseDepthBuffer(cameraPassData.sceneDepth, DepthAccess.Write);

            var gizmosRenderers =
                passContext.renderGraph.CreateGizmoRendererList(cameraContext.camera, m_GizmoSubset);
            passContext.data.gizmosRendererList = builder.UseRendererList(gizmosRenderers);
            
            builder.AllowRendererListCulling(false);
            
            builder.SetRenderFunc<GizmosPassData>(static (data, context) =>
            {
                context.cmd.DrawRendererList(data.gizmosRendererList);
            });
        }

        protected override bool ShouldCullPass(in PassContext passContext, in CameraContext cameraContext)
        {
            #if UNITY_EDITOR
            return !UnityEditor.Handles.ShouldRenderGizmos();
            #else
            return true;
            #endif
        }
    }
}