namespace ResourcesAPI.Models.Interfaces
{
    public interface ISaveable
    {
        public void Save(string filename);

        public void Load(string filename);
    }
}
