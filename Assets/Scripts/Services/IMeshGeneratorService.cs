using UnityEngine;

namespace Game.Services
{
    public interface IMeshGeneratorService
    {
        Mesh GenerateMesh(out float boundingRadius);
    }
}
