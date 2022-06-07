#nullable enable
namespace AA.CommoditiesDashboard.Api.Model
{
    public class Commodity
    {
        public Commodity(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Id { get; }

        public string Name { get; }
    }
}