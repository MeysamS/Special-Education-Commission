namespace Comision.Service.Model
{
    public class UserProfileModel
    {
        public long UserId { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string FullName => Name + " " + Family;
        public string NationalCode { get; set; }
        public string UserName { get; set; }
    }
}
