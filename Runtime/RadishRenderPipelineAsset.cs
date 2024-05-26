using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Radish.Rendering
{
    [CreateAssetMenu(menuName = RadishMenus.PipelineAssetMenuItem, order = RadishMenus.MenuOrder)]
    public sealed class RadishRenderPipelineAsset : RenderPipelineAsset<RadishRenderPipeline>
    {
        public const string DefaultMaterialPath = "Packages/com.radish.render-pipeline/Materials/RadishLit-Default.mat";
        
        [SerializeField] private Material m_DefaultMaterial;
        public override Material defaultMaterial => m_DefaultMaterial;

        [SerializeField] private List<RadishRenderPassBase> m_Passes = new();
        public IReadOnlyList<RadishRenderPassBase> passes => m_Passes;

        protected override RenderPipeline CreatePipeline()
        {
            return new RadishRenderPipeline(this);
        }
    }
}