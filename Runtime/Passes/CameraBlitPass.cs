using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering.Passes
{
    [CreateAssetMenu(menuName = RadishMenus.PassAssetMenuPrefix + "Camera Blit Pass", order = RadishMenus.MenuOrder)]
    public sealed class CameraBlitPass : RadishRenderPass<CameraBlitPassData>
    {
        protected override void SetupPass(in PassContext passContext, in CameraContext cameraContext, ref RenderGraphBuilder builder)
        {
            var cameraPassData = passContext.passManager.GetData<ICameraCommonPassData>();
            
            passContext.data.sceneColor = builder.ReadTexture(cameraPassData.sceneColor);
            passContext.data.cameraTarget =
                passContext.renderGraph.ImportBackbuffer(BuiltinRenderTextureType.CameraTarget);
            
            builder.SetRenderFunc<CameraBlitPassData>(static (data, context) =>
            {
                context.cmd.Blit(data.sceneColor, data.cameraTarget);
            });
        }
    }
}