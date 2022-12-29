namespace Projekat.DAL.Model
{
    public class Following : Entity
    {
        public User User { get; set; }
        public int? UserId { get; set; }
        public Item Item { get; set; }
        public int? ItemId { get; set; }
    }
}
