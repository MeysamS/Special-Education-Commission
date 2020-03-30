using System.Collections.Generic;

namespace Comision.Service.Model.PrintModel
{
    public class PSignersModel
    {
        public string PostName1 { get; set; }
        public string SignatureUrl1 { get; set; }

        public string PostName2 { get; set; }
        public string SignatureUrl2 { get; set; }

        public string PostName3 { get; set; }
        public string SignatureUrl3 { get; set; }

        public string PostName4 { get; set; }
        public string SignatureUrl4 { get; set; }

        public string PostName5 { get; set; }
        public string SignatureUrl5 { get; set; }

        public string PostName6 { get; set; }
        public string SignatureUrl6 { get; set; }

        public string PostName7 { get; set; }
        public string SignatureUrl7 { get; set; }

        public PSignersModel()
        { }
        public PSignersModel(List<PSignersListModel> pSignersListModel)
        {
            foreach (PSignersListModel item in pSignersListModel)
            {
                int number = item.RowNumber;
                switch (number)
                {
                    case 1:
                     
                        PostName1 = item.PostName;
                        if (item.SignatureUrl != null)// اگر امضا دارد
                            SignatureUrl1 = item.SignatureUrl;
                        break;

                    case 2:
                        PostName2 = item.PostName;
                        if (item.SignatureUrl != null)// اگر امضا دارد
                            SignatureUrl2 = item.SignatureUrl;
                        break;

                    case 3:
                        PostName3 = item.PostName;
                        if (item.SignatureUrl != null)// اگر امضا دارد
                            SignatureUrl3 = item.SignatureUrl;
                        break;

                    case 4:
                        PostName4 = item.PostName;
                        if (item.SignatureUrl != null)// اگر امضا دارد
                            SignatureUrl4 = item.SignatureUrl;
                        break;

                    case 5:
                        PostName5 = item.PostName;
                        if (item.SignatureUrl != null)// اگر امضا دارد
                            SignatureUrl5 = item.SignatureUrl;
                        break;

                    case 6:
                        PostName6 = item.PostName;
                        if (item.SignatureUrl != null)// اگر امضا دارد
                            SignatureUrl6 = item.SignatureUrl;
                        break;

                    case 7:
                        PostName7 = item.PostName;
                        if (item.SignatureUrl != null)// اگر امضا دارد
                            SignatureUrl7 = item.SignatureUrl;
                        break;

                }
            }
        }
    }
}
