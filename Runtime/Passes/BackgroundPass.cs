using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    [CreateAssetMenu(menuName = RadishMenus.PassAssetMenuPrefix + "Background Pass", order = RadishMenus.MenuOrder)]
    public sealed class BackgroundPass : RadishRenderPass<BackgroundPassData>
    {
        protected override void SetupPass(in PassContext passContext, in CameraContext cameraContext, ref RenderGraphBuilder builder)
        {
            var cameraPassData = passContext.passManager.GetData<ICameraCommonPassData>();
            
            passContext.data.sceneColor = builder.UseColorBuffer(cameraPassData.sceneColor, 0);
            passContext.data.sceneDepth = builder.UseDepthBuffer(cameraPassData.sceneDepth, DepthAccess.Write);

            var skyboxRenderers = passContext.renderGraph.CreateSkyboxRendererList(cameraContext.camera);
            passContext.data.skyboxRenderers = builder.UseRendererList(skyboxRenderers);
            
            builder.AllowRendererListCulling(false);
            
            builder.SetRenderFunc<BackgroundPassData>(static (data, context) =>
            {
                context.cmd.DrawRendererList(data.skyboxRenderers);
            });
        }

        protected override bool ShouldCullPass(in PassContext passContext, in CameraContext cameraContext)
        {
            return cameraContext.camera.clearFlags != CameraClearFlags.Skybox;
        }
    }
}