using MyGame.Framework.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMaterial
{
    private Dictionary<string, int> shaderProperties;
    private readonly ResizableArray<float> _uvToPass = new();

    private MaterialPropertyBlock propertyBlock = new();
    public MaterialPropertyBlock PropertyBlock => propertyBlock;

    public void Clear()
    {
        propertyBlock.Clear();
    }

    public void SetShape(float width, float length, Vector2[] uv, float fill, float dissolve)
    {
        Update(uv);
        propertyBlock.SetFloat("_Fill", fill);
        propertyBlock.SetFloat("_Dissolve", Mathf.Clamp(dissolve, 0, 1));

        //控制链式激光形状
        propertyBlock.SetVector("_QuadSize", new Vector4(width, length));

        //控制链式激光的波动
        propertyBlock.SetFloatArray("_LineUV", _uvToPass.Items);
        propertyBlock.SetInt("_Points", _uvToPass.Items.Length);
    }

    private void Update(Vector2[] uv)
    {
        _uvToPass.Clear();

        for (int i = 0; i < uv.Length; i += 4)
        {
            _uvToPass.Add(uv[i].y);
        }

        _uvToPass.Add(1);
    }

    /// <summary>
    /// 根据Shader属性名称返回其唯一ID
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public int AddShaderPropertie(string name)
    {
        if(shaderProperties.ContainsKey(name) != true)
        {
            int id = Shader.PropertyToID(name);
            shaderProperties[name] = id;
        }
        return shaderProperties[name];
    }
}
