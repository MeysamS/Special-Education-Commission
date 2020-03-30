namespace Comision.Ui.Areas.Personel.Models
{
    public class AttachmentListModel
    {
        public long AttachmentId { get; set; }
        public long RequestId { get; set; }
        public string FileName { get; set; }
        public string Extention { get; set; }
        public long Size { get; set; }
        public string Path { get; set; }
    }
}