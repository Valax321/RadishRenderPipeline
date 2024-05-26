using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Radish.Rendering
{
    [Serializable]
    public struct SerializedRenderQueueRange
    {
        [SerializeField] private int m_Min;
        [SerializeField] private int m_Max;

        public SerializedRenderQueueRange(int min, int max)
        {
            m_Min = min;
            m_Max = max;
        }

        public RenderQueueRange value => new(m_Min, m_Max);
    }
}