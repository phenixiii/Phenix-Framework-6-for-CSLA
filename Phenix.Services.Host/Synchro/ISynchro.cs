namespace Phenix.Services.Host.Synchro
{
  public interface ISynchro
  {
    #region 方法

    void ClearServiceLibrarySubdirectory();

    void Upload(string subdirectoryName, string fileName, int fileLength, byte[] fileBytes);

    #endregion
  }
}
