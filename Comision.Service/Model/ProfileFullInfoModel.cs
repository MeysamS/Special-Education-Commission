namespace Comision.Service.Model
{
    public class ProfileFullInfoModel
    {
        public virtual ProfileModel ProfileModel { get; set; }
        public virtual PersonelModel PersonelModel { get; set; }
        public virtual StudentModel StudentModel { get; set; }
        public virtual UserModel UserModel { get; set; }
        public virtual PasswordModel PasswordModel { get; set; }
    }
}
