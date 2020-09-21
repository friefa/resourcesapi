namespace ResourcesAPI.Models
{
    public interface ISaveable
    {
        public void Save(string filename);

        public void Load(string filename);
    }
}
