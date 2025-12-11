using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace GameCtor.DevToolbox
{
    /// <summary>
    /// Uses GPU instancing for mesh rendering, and a compute shader for rotating around the origin.
    /// </summary>
    public sealed class GpuInstancingOrbiter : MonoBehaviour
    {
        private static readonly int PositionsId = Shader.PropertyToID("_Positions");
        private static readonly int MatricesId = Shader.PropertyToID("_Matrices");
        private static readonly int RotationAxesId = Shader.PropertyToID("_RotationAxes");
        private static readonly int PositionCountId = Shader.PropertyToID("_PositionCount");
        private static readonly int OriginPositionId = Shader.PropertyToID("_OriginPosition");
        private static readonly int CameraPositionId = Shader.PropertyToID("_CameraPosition");
        private static readonly int PointSizeId = Shader.PropertyToID("_PointSize");
        private static readonly int SpeedId = Shader.PropertyToID("_Speeds");
        private static readonly int DeltaTimeId = Shader.PropertyToID("_DeltaTime");

        [SerializeField]
        private int _maxPoints = 20_000;
        [SerializeField]
        private ComputeShader _computeShader;
        [SerializeField]
        private Material _material;
        [SerializeField]
        private Mesh _mesh;
        [SerializeField]
        private bool _useBillboarding;
        [SerializeField, Range(0.0001f, 10f)]
        private float[] _speeds = new[] { 0.005f, 0.01f, 0.2f };
        [SerializeField, Range(0.01f, 2f)]
        private float[] _sizes = new[] { 0.1f, 0.2f, 0.3f };

        private ComputeBuffer _positionsBuffer;
        private ComputeBuffer _matricesBuffer;
        private ComputeBuffer _rotationAxesBuffer;
        private ComputeBuffer _speedBuffer;
        private ComputeBuffer _sizeBuffer;
        private int _positionCount;
        private int _kernelIndex;
        private int _threadGroupsX;
        private uint _threadGroupSizeX;
        private Bounds _bounds;
        private Transform _camTransform;
        private RenderParams _renderParams;

        private void Awake()
        {
            _positionsBuffer = new ComputeBuffer(_maxPoints, sizeof(float) * 3);
            _rotationAxesBuffer = new ComputeBuffer(_maxPoints, sizeof(float) * 3);
            _matricesBuffer = new ComputeBuffer(_maxPoints, sizeof(float) * 16);
            _speedBuffer = new ComputeBuffer(_speeds.Length, sizeof(float));
            _sizeBuffer = new ComputeBuffer(_sizes.Length, sizeof(float));

            PrepKernel();

            _material.SetBuffer(PositionsId, _positionsBuffer);
            _material.SetBuffer(MatricesId, _matricesBuffer);

            _speedBuffer.SetData(_speeds);
            _sizeBuffer.SetData(_sizes);

            _renderParams = new RenderParams
            {
                shadowCastingMode = ShadowCastingMode.Off,
                receiveShadows = false,
                material = _material,
                worldBounds = new Bounds(new(0, 0, 0), Vector3.one * 500_000),
                renderingLayerMask = RenderingLayerMask.defaultRenderingLayerMask,
            };

            _camTransform = Camera.main.transform;
        }

        private void OnDestroy()
        {
            _positionsBuffer.Release();
            _positionsBuffer = null;
            _matricesBuffer.Release();
            _matricesBuffer = null;
            _rotationAxesBuffer.Release();
            _rotationAxesBuffer = null;
            _sizeBuffer.Release();
            _sizeBuffer = null;
            _speedBuffer.Release();
            _speedBuffer = null;
        }

        private void Update()
        {
            RenderPoints();
        }

        /// <summary>
        /// Updates the list of points that will be rendered.
        /// </summary>
        /// <param name="positions">
        /// A list of positions in world space where the meshes will be rendered.
        /// </param>
        /// <param name="rotationAxes">
        /// A list of axes used to rotate a mesh at the corresponding index of the positions collection.
        /// </param>
        /// <param name="bounds">
        /// The world space bounding volume that encapsulates all positions.
        /// This is used for camera frustum culling.
        /// </param>
        public void UpdatePoints(List<Vector3> positions, Vector3[] rotationAxes, Bounds bounds)
        {
            _positionCount = positions.Count;
            _bounds = bounds;
            _renderParams.worldBounds = bounds;
            _positionsBuffer.SetData(positions);
            _rotationAxesBuffer.SetData(rotationAxes);
            _threadGroupsX = Mathf.CeilToInt((float)_positionCount / _threadGroupSizeX);
            _computeShader.SetInt(PositionCountId, _positionCount);
        }

        private void RenderPoints()
        {
            if (_positionCount == 0)
            {
                return;
            }

            _computeShader.SetFloat(DeltaTimeId, Time.deltaTime);
            _computeShader.SetVector(OriginPositionId, transform.position);
            if (_useBillboarding)
            {
                _computeShader.SetVector(CameraPositionId, _camTransform.position);
            }
            _computeShader.Dispatch(_kernelIndex, _threadGroupsX, 1, 1);

            Graphics.RenderMeshPrimitives(
                _renderParams,
                _mesh,
                submeshIndex: 0,
                _positionCount);
        }

        private void PrepKernel()
        {
            _kernelIndex = _useBillboarding
                ? _computeShader.FindKernel("GpuInstancingOrbiter_Billboarding")
                : _computeShader.FindKernel("GpuInstancingOrbiter");

            _computeShader.SetBuffer(_kernelIndex, PositionsId, _positionsBuffer);
            _computeShader.SetBuffer(_kernelIndex, MatricesId, _matricesBuffer);
            _computeShader.SetBuffer(_kernelIndex, RotationAxesId, _rotationAxesBuffer);
            _computeShader.SetBuffer(_kernelIndex, SpeedId, _speedBuffer);
            _computeShader.SetBuffer(_kernelIndex, PointSizeId, _sizeBuffer);
            _computeShader.GetKernelThreadGroupSizes(_kernelIndex, out _threadGroupSizeX, out _, out _);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!Application.isPlaying || _positionsBuffer == null)
            {
                return;
            }
            if ((_kernelIndex == 0 && _useBillboarding) || (_kernelIndex == 1 && !_useBillboarding))
            {
                PrepKernel();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        }
#endif
    }
}
