using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using UnityEngine;

public static class Extensions
{
    public static TaskAwaiter GetAwaiter(this AsyncOperation asyncOp)
    {
        var tcs = new TaskCompletionSource<object>();
        asyncOp.completed += obj => { tcs.SetResult(null); };
        return ((Task) tcs.Task).GetAwaiter();
    }

    // use to avoid copying the whole material
    public static void SetColor(this Renderer renderer, Color color)
    {
        var propertyBlock = new MaterialPropertyBlock();
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor("_Color", color);
        renderer.SetPropertyBlock(propertyBlock);
    }
    
}