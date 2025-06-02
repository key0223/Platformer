
public interface ISavable
{
    void RegisterSave();
    void DeregisterSave();
    object CaptureData();
    void RestoreData(object loadedata);
}
