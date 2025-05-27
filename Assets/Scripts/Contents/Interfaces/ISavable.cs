using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISavable
{
    void SaveRegister();
    void SaveDeregister();
    object CaptureData();
    void RestoreData(object data);
}
