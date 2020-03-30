using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Comision.Model.Enum;

namespace Comision.Service.IService
{
    public interface IFileManagementService
    {
        Tuple<bool, string> FileUpload(HttpPostedFileBase file, string savetoPath);
        DataTable GetExcelContent(string savedFilePath);
        List<string> GetExcelContent2(string savedFilePath);
        Tuple<bool, string> SaveToDatabase(string savedFileName, AuthenticationType authenticationType, AuthenticationType userAuthenticationType, long levelId);
        bool CreateFolderIfNeeded(string path);
        string GetLocalFilePath(string saveDirectory, FileUpload fileUploadControl);
    }
}
