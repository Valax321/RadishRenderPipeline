using UnityEditor;

namespace Radish.Rendering
{
    internal sealed class RadishRenderPipelineAssetProperties
    {
        public SerializedObject obj { get; }
        public SerializedProperty defaultMaterial { get; }
        public SerializedProperty passes { get; }
        
        public RadishRenderPipelineAssetProperties(SerializedObject obj)
        {
            this.obj = obj;
            defaultMaterial = obj.FindProperty("m_DefaultMaterial");
            passes = obj.FindProperty("m_Passes");
        }
    }
}