namespace HDH.UserData.Dto
{
    public interface IUserDataConfig
    {
        public float SaveDelayInSeconds { get; }
        public string SaveFolderPath { get; }
        public string FileExtension { get; }
    }
}