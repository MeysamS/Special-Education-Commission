namespace Comision.Service.Model
{
    public class DropDownModel
    {
        public string Value { get; set; }
        public string Text { get; set; }

        public DropDownModel(){}
        public DropDownModel(string value, string text)
        {
            Value = value;
            Text = text;
        }
    }
}
