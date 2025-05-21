using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BufferManager
{
    Dictionary<BufferType,InputBuffer> _buffers =new Dictionary<BufferType, InputBuffer>();

    public void Add(BufferType bufferType,float bufferTime)
    {
        _buffers[bufferType] = new InputBuffer(bufferTime);
    }

    public void Remove(BufferType bufferType)
    {
        _buffers.Remove(bufferType);
    }

    public InputBuffer Get(BufferType bufferType)
    {
        return _buffers.TryGetValue(bufferType, out InputBuffer buffer)? buffer:null; 
    }

    public void UpdateAll(float deltaTime)
    {
        foreach(InputBuffer buffer in _buffers.Values)
            buffer.Update(deltaTime);
    }
}
