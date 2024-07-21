using UnityEngine;

namespace Game.Services
{
    public class MeshGeneratorService : IMeshGeneratorService
    {
        public Mesh GenerateMesh(out float boundingRadius)
        {
            var mesh = new Mesh();

            var vertices = new Vector3[]
            {
                new Vector3(-0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, -0.5f, -0.5f),
                new Vector3(0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, 0.5f, -0.5f),
                new Vector3(-0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, -0.5f, 0.5f),
                new Vector3(0.5f, 0.5f, 0.5f),
                new Vector3(-0.5f, 0.5f, 0.5f)
            };

            var scaleMultiplier = Random.Range(0.5f, 1.5f);

            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i] *= scaleMultiplier;
                vertices[i] += new Vector3(
                    Random.Range(-0.15f, 0.15f),
                    Random.Range(-0.15f, 0.15f),
                    Random.Range(-0.15f, 0.15f)
                );
            }

            var triangles = new int[]
            {
                0, 2, 1,
                0, 3, 2,
                4, 5, 6,
                4, 6, 7,
                0, 7, 3,
                0, 4, 7,
                1, 2, 6,
                1, 6, 5,
                2, 3, 7,
                2, 7, 6,
                0, 1, 5,
                0, 5, 4
            };

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.RecalculateNormals();

            boundingRadius = CalculateBoundingRadius(vertices);

            return mesh;
        }

        private float CalculateBoundingRadius(Vector3[] vertices)
        {
            var maxDistanceSquared = 0f;

            foreach (var vertex in vertices)
            {
                var distanceSquared = vertex.sqrMagnitude;
                if (distanceSquared > maxDistanceSquared)
                {
                    maxDistanceSquared = distanceSquared;
                }
            }

            return Mathf.Sqrt(maxDistanceSquared);
        }
    }
}
