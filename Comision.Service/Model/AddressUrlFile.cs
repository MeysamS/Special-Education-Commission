namespace Comision.Service.Model
{
    public class AddressUrlFile
    {
        public string Image { get; } = "Content\\Images\\Avatars\\";
        public string Logo { get; } = "Content\\Images\\Logo\\";
        public string Signature { get; } = "Content\\Images\\Signature\\";
        public string UploadFiles { get; } = "Content\\UploadFiles\\RequestAttachment\\";
        public string SliderImage { get; } = "Content\\Images\\SliderImage\\";
        public string Localizationfa { get; } = "Content\\Localization\\fa.xml";
        public AddressUrlFile(string rootPath)
        {
            Image = rootPath + Image;
            Signature = rootPath + Signature;
            UploadFiles = rootPath + UploadFiles;
            Logo = rootPath + Logo;
            SliderImage = rootPath + SliderImage;
            Localizationfa = rootPath + Localizationfa;
        }
    }
}
