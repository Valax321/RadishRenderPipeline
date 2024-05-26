using System;
using UnityEditor;
using UnityEngine;

namespace Radish.Rendering
{
    [CustomEditor(typeof(RadishRenderPipelineAsset))]
    public class RadishRenderPipelineAssetEditor : Editor
    {
        private static class Sections
        {
            public const string Main = "Radish:RenderPipelineAsset:MainSection";
            public const string Passes = "Radish:RenderPipelineAsset:PassesSection";
        }

        private static class Content
        {
            public static readonly GUIContent MainSectionHeader 
                = EditorGUIUtility.TrTextContent("Main", "Main properties");

            public static readonly GUIContent PassesSectionHeader
                = EditorGUIUtility.TrTextContent("Passes", "Required passes for this pipeline");
        }
        
        private RadishRenderPipelineAssetProperties m_Properties;

        public override void OnInspectorGUI()
        {
            m_Properties = new RadishRenderPipelineAssetProperties(serializedObject);
            
            EditorGUI.BeginChangeCheck();
            
            MainSectionGUI();
            
            EditorGUILayout.PropertyField(m_Properties.passes);

            if (!EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();
        }

        private void MainSectionGUI()
        {
            if (RadishEditorGUI.EditorPrefsFoldoutHeaderGroup(Sections.Main, Content.MainSectionHeader))
            {
                // m_DefaultMaterial
                if (!m_Properties.defaultMaterial.objectReferenceValue)
                {
                    m_Properties.defaultMaterial.objectReferenceValue =
                        AssetDatabase.LoadAssetAtPath<Material>(RadishRenderPipelineAsset.DefaultMaterialPath);
                }
                EditorGUILayout.PropertyField(m_Properties.defaultMaterial);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();
        }
    }
}