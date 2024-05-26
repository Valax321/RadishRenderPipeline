using UnityEngine;
using UnityEngine.Rendering;

namespace Radish.Rendering
{
    public readonly struct CameraContext
    {
        public readonly Camera camera;
        public readonly CullingResults cullResults;

        public CameraContext(Camera camera, in CullingResults cullResults)
        {
            this.camera = camera;
            this.cullResults = cullResults;
        }
    }
}