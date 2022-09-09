using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using Unity.Collections;
using UnityEngine.Rendering;


[System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
public struct VertexData
{
    public Vector3 pos;
    public byte nx, ny, nz, nw;
    public byte tx, ty, tz, tw;
    public ushort u, v;
    public ushort bx, by, bz, bw;
    public uint bi;
}


public class MeshGenerator : EditorWindow
{
    private string assetPath = string.Empty;


    [MenuItem("Tools/メッシュ生成テスト")]
    static private void OpenGenerator()
    {
        EditorWindow.GetWindow<MeshGenerator>();
    }

    private void OnGUI()
    {
        assetPath = EditorGUILayout.TextField("出力パス", assetPath);
        if (!string.IsNullOrWhiteSpace(assetPath))
        {
            assetPath = Path.ChangeExtension(assetPath, ".asset");
        }

        if (GUILayout.Button("生成"))
        {
            Generate();
        }
    }

    private void Generate()
    {
        var mesh = new Mesh();

        mesh.SetVertexBufferParams(4, new VertexAttributeDescriptor()
        {
            attribute = VertexAttribute.Position,
            dimension = 3,
            format = VertexAttributeFormat.Float32,
        }, new VertexAttributeDescriptor()
        {
            attribute = VertexAttribute.Normal,
            dimension = 4,
            format = VertexAttributeFormat.UNorm8,
        }, new VertexAttributeDescriptor()
        {
            attribute = VertexAttribute.Tangent,
            dimension = 4,
            format = VertexAttributeFormat.UNorm8,
        }, new VertexAttributeDescriptor()
        {
            attribute = VertexAttribute.TexCoord0,
            dimension = 2,
            format = VertexAttributeFormat.Float16,

        }, new VertexAttributeDescriptor()
        {
            //VertexAttribute.BlendWeightは出力されているが、Inspector上では見えない
            attribute = VertexAttribute.BlendWeight,
            dimension = 4,
            format = VertexAttributeFormat.Float16,
        }, new VertexAttributeDescriptor()
        {
            //VertexAttribute.BlendIndicesは出力されているが、Inspector上では見えない
            attribute = VertexAttribute.BlendIndices,
            dimension = 1,
            format = VertexAttributeFormat.UInt32,
        });
        mesh.SetIndices(new[]{
            0, 1, 2, 3
        }, MeshTopology.Quads, 0);


        var hz = HalfHelper.SingleToHalf(0);
        var ho = HalfHelper.SingleToHalf(1);

        mesh.SetVertexBufferData(new VertexData[]{
            new VertexData(){
                pos = new Vector3(0, 0, 0),
                nx = 0, ny = 0, nz = 0, nw = 1,
                tx = 0, ty = 0, tz = 0, tw = 1,
                u = hz, v = hz,
                bx = hz, by = hz, bz = hz, bw = hz,
                bi = 0,
            },
            new VertexData(){
                pos = new Vector3(0, 1, 0),
                nx = 0, ny = 0, nz = 0, nw = 1,
                tx = 0, ty = 0, tz = 0, tw = 1,
                u = hz, v = ho,
                bx = hz, by = hz, bz = hz, bw = hz,
                bi = 0,
            },
            new VertexData(){
                pos = new Vector3(1, 1, 0),
                nx = 0, ny = 0, nz = 0, nw = 1,
                tx = 0, ty = 0, tz = 0, tw = 1,
                u = ho, v = ho,
                bx = hz, by = hz, bz = hz, bw = hz,
                bi = 0,
            },
            new VertexData(){
                pos = new Vector3(1, 0, 0),
                nx = 0, ny = 0, nz = 0, nw = 1,
                tx = 0, ty = 0, tz = 0, tw = 1,
                u = ho, v = hz,
                bx = hz, by = hz, bz = hz, bw = hz,
                bi = 0,
            },
        }, 0, 0, 4); ;

        //mesh.SetVertices(new NativeArray<HalfVector>(new[]{
        //    new HalfVector(){
        //        x = hz,
        //        y = hz,
        //        z = hz,
        //    },
        //    new HalfVector(){
        //        x = hz,
        //        y = hz,
        //        z = hz,
        //    },
        //    new HalfVector(){
        //        x = hz,
        //        y = hz,
        //        z = hz,
        //    },
        //    new HalfVector(){
        //        x = hz,
        //        y = hz,
        //        z = hz,
        //    },
        //}, Allocator.Temp));
        //mesh.SetColors(new[]{
        //    Color.red,
        //    Color.red,
        //    Color.red,
        //    Color.red,
        //});

        AssetDatabase.CreateAsset(mesh, assetPath);
    }
}
