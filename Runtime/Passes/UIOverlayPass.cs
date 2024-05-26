using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    [CreateAssetMenu(menuName = RadishMenus.PassAssetMenuPrefix + "UI Overlay Pass", order = RadishMenus.MenuOrder)]
    public sealed class UIOverlayPass : RadishRenderPass<UIOverlayPassData>
    {
        protected override void SetupPass(in PassContext passContext, in CameraContext cameraContext, ref RenderGraphBuilder builder)
        {
            var cameraPassData = passContext.passManager.GetData<ICameraCommonPassData>();
            
            passContext.data.sceneColor = builder.UseColorBuffer(cameraPassData.sceneColor, 0);
            passContext.data.sceneDepth = builder.UseDepthBuffer(cameraPassData.sceneDepth, DepthAccess.Write);

            var uiRenderList = passContext.renderGraph.CreateUIOverlayRendererList(cameraContext.camera);
            passContext.data.uiRenderList = builder.UseRendererList(uiRenderList);
            
            builder.AllowRendererListCulling(true);
            
            builder.SetRenderFunc<UIOverlayPassData>(static (data, context) =>
            {
                context.cmd.DrawRendererList(data.uiRenderList);
            });
        }

        protected override bool ShouldCullPass(in PassContext passContext, in CameraContext cameraContext)
        {
            return cameraContext.camera.cameraType is not (CameraType.Game or CameraType.VR);
        }
    }
}