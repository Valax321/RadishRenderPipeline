using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Radish.Rendering.Passes;
using UnityEngine.Rendering.RenderGraphModule;

namespace Radish.Rendering
{
    public class RenderPassManager
    {
        public RadishRenderPipeline pipeline { get; }
        public RenderGraph renderGraph { get; }

        private readonly Dictionary<Type, object> m_LastImplementorOfDataType = new();

        private static Dictionary<Type, List<Type>> s_InterfaceMap = new();
        
        public RenderPassManager(RadishRenderPipeline pipeline, RenderGraph graph)
        {
            this.pipeline = pipeline;
            renderGraph = graph;
        }

        public void BeginCameraFrame()
        {
            
        }

        public void EndCameraFrame()
        {
            m_LastImplementorOfDataType.Clear();
        }

        [PublicAPI]
        public TPassData GetData<TPassData>() where TPassData : IRadishCommonPassData
        {
            if (!m_LastImplementorOfDataType.TryGetValue(typeof(TPassData), out var data))
                throw new KeyNotFoundException();

            return (TPassData)data;
        }

        internal void CollectDataInterfacesForData<TPassData>(TPassData passData) where TPassData : class, new()
        {
            var interfaces = ExtractInterfaces<TPassData>();
            foreach (var i in interfaces)
            {
                m_LastImplementorOfDataType[i] = passData;
            }
        }

        private static List<Type> ExtractInterfaces<TPassData>() where TPassData : class, new()
        {
            if (!s_InterfaceMap.TryGetValue(typeof(TPassData), out var interfaces))
            {
                interfaces = typeof(TPassData).GetInterfaces()
                    .Where(x => typeof(IRadishCommonPassData).IsAssignableFrom(x)).ToList();
                s_InterfaceMap.Add(typeof(TPassData), interfaces);
            }

            return interfaces;
        }
    }
}