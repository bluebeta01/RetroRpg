using System.Xml;
using OpenTK.Graphics.OpenGL;

namespace RetroRpg;

public class ColladaParser
{
    private Dictionary<string, List<float>> _floatArrayDict = new();
    private XmlNode _meshNode = default!;
    private XmlNamespaceManager _namespaceManager = default!;
    private List<int> _triangleElements = new();
    private List<float> _vertexData = new();
    private List<Mesh> _meshes = new();
    private List<int> _meshElements = new();
    private int _elementCounter;
    
    public Model? ParseFile(string filepath)
    {
        try
        {
            return ParseFileInternal(filepath);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to parse {filepath}. {e.Message}");
            return null;
        }
    }

    private void LoadFloatArray(string sourceId)
    {
        if (_floatArrayDict.ContainsKey(sourceId)) return;

        _floatArrayDict[sourceId] = GetSourceFloats(sourceId);
    }

    private List<float> GetSourceFloats(string sourceId)
    {
        List<float> floatList = new();
        var sourceNodes = _meshNode.SelectNodes("descendant::dae:source", _namespaceManager);
        if (sourceNodes is not {Count: > 0})
            throw new Exception($"Failed to find source node {sourceId}");
        for (int i = 0; i < sourceNodes.Count; i++)
        {
            var srcNode = sourceNodes.Item(i)!;
            if (srcNode.Attributes?.GetNamedItem("id")?.Value == sourceId)
            {
                var floatNode = srcNode.SelectSingleNode("descendant::dae:float_array", _namespaceManager);
                if (floatNode is null)
                    throw new Exception($"Failed to find float array for source {sourceId}");
                var floatStrings = floatNode.InnerText.Split(" ");
                foreach (var floatString in floatStrings)
                {
                    floatList.Add(float.Parse(floatString));
                }

                return floatList;
            }
        }
        throw new Exception($"Failed to find source node {sourceId}");
    }

    private string GetSrcIdOfVertexPosition(string vertexSourceId)
    {
        var vertexNodes = _meshNode.SelectNodes("descendant::dae:vertices", _namespaceManager);
        if (vertexNodes is not { Count: > 0 })
            throw new Exception("Failed to find vertices node.");
        for (int vnIndex = 0; vnIndex < vertexNodes.Count; vnIndex++)
        {
            var vertexNode = vertexNodes.Item(vnIndex)!;
            if (vertexNode.Attributes?.GetNamedItem("id")?.Value == vertexSourceId)
            {
                var inputNodes = vertexNode.SelectNodes("descendant::dae:input", _namespaceManager);
                if (inputNodes is not { Count: > 0 })
                    throw new Exception("Failed to find input node of vertices node.");
                for (int inIndex = 0; inIndex < inputNodes.Count; inIndex++)
                {
                    var inputNode = inputNodes.Item(inIndex)!;
                    if (inputNode.Attributes?.GetNamedItem("semantic")?.Value == "POSITION")
                    {
                        var inputNodeSourceId = inputNode.Attributes?.GetNamedItem("source")?.Value?.TrimStart('#');
                        if (inputNodeSourceId is null)
                            throw new Exception("Failed to find source id of vertices input node.");
                        return inputNodeSourceId;
                    }
                }
            }
        }

        throw new Exception($"Failed to find vertex node {vertexSourceId}");
    }
    
    private Model ParseFileInternal(string filepath)
    {
        _floatArrayDict.Clear();
        _triangleElements.Clear();
        _vertexData.Clear();
        _meshes.Clear();
        _meshElements.Clear();
        
        var model = new Model();
        model.Vao = GL.GenVertexArray();
        GL.BindVertexArray(model.Vao);

        var daeXml = File.ReadAllText(filepath);
        var daeDocument = new XmlDocument();
        daeDocument.LoadXml(daeXml);
        _namespaceManager = new XmlNamespaceManager(daeDocument.NameTable);
        _namespaceManager.AddNamespace("dae", "http://www.collada.org/2005/11/COLLADASchema");
        var tempMeshNode = daeDocument.DocumentElement?.SelectSingleNode("descendant::dae:library_geometries", _namespaceManager)
            ?.SelectSingleNode("descendant::dae:geometry", _namespaceManager)?.SelectSingleNode("descendant::dae:mesh", _namespaceManager);
        if (tempMeshNode is null)
            throw new Exception("Failed to find mesh node.");
        _meshNode = tempMeshNode;

        var triangleNodes = _meshNode.SelectNodes("descendant::dae:triangles", _namespaceManager);
        if (triangleNodes is not { Count: > 0 })
            throw new Exception("Failed to find triangles node.");
        for (int triNodeIndex = 0; triNodeIndex < triangleNodes.Count; triNodeIndex++)
        {
            var triNode = triangleNodes.Item(triNodeIndex)!;
            var inputNodes = triNode.SelectNodes("descendant::dae:input", _namespaceManager);
            if (inputNodes is not { Count: > 0 })
                throw new Exception("Failed to find triangles input node.");
            var vertexInputSourceId = "";
            int vertexInputOffset = 0;
            var normalInputSourceId = "";
            int normalInputOffset = 0;
            var uvInputSourceId = "";
            var uvInputOffset = 0;
            for (int i = 0; i < inputNodes.Count; i++)
            {
                var inputNode = inputNodes.Item(i)!;
                if (inputNode.Attributes?.GetNamedItem("semantic")?.Value == "NORMAL")
                {
                    normalInputSourceId = inputNode.Attributes?.GetNamedItem("source")?.Value?.TrimStart('#') ?? "";
                    normalInputOffset = int.Parse(inputNode.Attributes?.GetNamedItem("offset")?.Value ?? "0");
                    LoadFloatArray(normalInputSourceId);
                }
                if (inputNode.Attributes?.GetNamedItem("semantic")?.Value == "TEXCOORD")
                {
                    uvInputSourceId = inputNode.Attributes?.GetNamedItem("source")?.Value?.TrimStart('#') ?? "";
                    uvInputOffset = int.Parse(inputNode.Attributes?.GetNamedItem("offset")?.Value ?? "0");
                    LoadFloatArray(uvInputSourceId);
                }
                if (inputNode.Attributes?.GetNamedItem("semantic")?.Value == "VERTEX")
                {
                    var vertexSourceId = inputNode.Attributes?.GetNamedItem("source")?.Value?.TrimStart('#') ?? "";
                    vertexInputSourceId = GetSrcIdOfVertexPosition(vertexSourceId);
                    vertexInputOffset = int.Parse(inputNode.Attributes?.GetNamedItem("offset")?.Value ?? "0");
                    LoadFloatArray(vertexInputSourceId);
                }
            }
            var vertexComponentCount = inputNodes.Count;
            var pNode = triNode.SelectSingleNode("descendant::dae:p", _namespaceManager);
            if (pNode is null)
                throw new Exception("Failed to find triangles <p> node.");
            var triangleElementStrings = pNode.InnerText.Split(" ");
            _triangleElements.Clear();
            foreach (var elementString in triangleElementStrings)
            {
                _triangleElements.Add(int.Parse(elementString));
            }

            _meshElements.Clear();
            var mesh = new Mesh()
            {
                Ebo = GL.GenBuffer()
            };
            _meshes.Add(mesh);
            for (int i = 0; i < _triangleElements.Count; i += vertexComponentCount)
            {
                //TODO: This method can result in duplicate vertices, making EBOs pointless. This needs to be fixed.
                var vertexIndex = _triangleElements[i + vertexInputOffset];
                var normalIndex = _triangleElements[i + normalInputOffset];
                var uvIndex = _triangleElements[i + uvInputOffset];

                var vertexInputData = _floatArrayDict[vertexInputSourceId];
                _vertexData.Add(vertexInputData[vertexIndex * 3]);
                _vertexData.Add(vertexInputData[vertexIndex * 3 + 1]);
                _vertexData.Add(vertexInputData[vertexIndex * 3 + 2]);
                
                var normalInputData = _floatArrayDict[normalInputSourceId];
                _vertexData.Add(normalInputData[normalIndex * 3]);
                _vertexData.Add(normalInputData[normalIndex * 3 + 1]);
                _vertexData.Add(normalInputData[normalIndex * 3 + 2]);
                
                var uvInputData = _floatArrayDict[uvInputSourceId];
                _vertexData.Add(uvInputData[uvIndex * 2]);
                _vertexData.Add(uvInputData[uvIndex * 2 + 1]);
                
                _meshElements.Add(_elementCounter);
                _elementCounter++;
            }

            mesh.ElementCount = _meshElements.Count;
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mesh.Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _meshElements.Count * sizeof(uint), _meshElements.ToArray(), BufferUsageHint.StaticDraw);
        }

        model.VertexCount = _vertexData.Count / 8;
        model.Meshes = _meshes.ToArray();
        GL.BindVertexArray(model.Vao);
        
        var positionVbo = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, positionVbo);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertexData.Count * sizeof(float), _vertexData.ToArray(), BufferUsageHint.StaticDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 8, 0);
        GL.EnableVertexAttribArray(0);
        GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 8, sizeof(float) * 3);
        GL.EnableVertexAttribArray(1);
        GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, sizeof(float) * 8, sizeof(float) * 6);
        GL.EnableVertexAttribArray(2);

        return model;
    }
}