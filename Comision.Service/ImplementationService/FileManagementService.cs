using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Comision.Data.Context;
using Comision.Model.Domain;
using Comision.Model.Enum;
using Comision.Repository.IRepository;
using Comision.Service.IService;
using Comision.Service.Model;

namespace Comision.Service.ImplementationService
{
    public class FileManagementService : IFileManagementService
    {
        private readonly IUnitOfWork _unotOfWork;
        private readonly IAuthenticationRepository _authenticationRepository;

        public FileManagementService(IUnitOfWork unotOfWork, IAuthenticationRepository authenticationRepository1)
        {
            _unotOfWork = unotOfWork;
            _authenticationRepository = authenticationRepository1;
        }

        public virtual Tuple<bool, string> FileUpload(HttpPostedFileBase file, string savetoPath)
        {
            try
            {
                //var data = new byte[256];
                //excelFile.InputStream.Read(data, 0, 256);
                //var detector = new MimeTypeDetector();
                //detector.GetMimeType(data);
                CreateFolderIfNeeded(savetoPath);
                string path = Path.GetFileName(file.FileName);
                if (path == null)
                {
                    return new Tuple<bool, string>(false, "خطا در خواندن از فایل");
                }
                string filePath = Path.Combine(savetoPath, path);
                file.SaveAs(filePath);

                return new Tuple<bool, string>(true, "عمل ذخیره فایل در سرور به درستی انجام شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در آپلود فایل در سرور");
            }
        }
        public DataTable GetExcelContent(string savedFilePath)
        {
            try
            {
                string excelConnectionString = "";
                string sourceFilePath = savedFilePath;
                //ایجاد کانکشن استرینگ برا خواندن کل اطلاعات از فایل اکسل و ریختن آنها در یک دیتاتیبل به نام dt
                if (Path.GetExtension(savedFilePath) == ".xlsx")
                    //تشخیص نوع فایل اکسل برای ایجاد کانکشن استرینگ برای نسخه‌های مختلف اکسل
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sourceFilePath + ";Extended Properties=Excel 12.0";
                else if (Path.GetExtension(savedFilePath) == ".xls")
                    excelConnectionString = "Provider=Microsoft.Jet.Oledb.4.0;Data Source=" + sourceFilePath + ";Extended Properties=Excel 8.0";

                DataTable dt = new DataTable("table");
                using (OleDbConnection connection = new OleDbConnection(excelConnectionString))
                {
                    connection.Open();
                    OleDbDataAdapter da = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connection);//List نام شیت در فایل اکسل است
                    da.Fill(dt);
                    connection.Close();
                }
                return dt;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public List<string> GetExcelContent2(string savedFilePath)
        {
            try
            {
                string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={savedFilePath};Extended Properties=Excel 12.0;";
                //int index = (savedFilePath).LastIndexOf('.');
                //int indexs = (savedFilePath).LastIndexOf('\\');
                //string name = (savedFilePath).Substring(indexs + 1);
                //string sheet = (savedFilePath).Substring(indexs + 1, index - (indexs + 1));

                var listExcel = new List<string>();
                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand("SELECT Code FROM [Sheet1$]", connection))
                    {
                        connection.Open();
                        using (OleDbDataReader dReader = cmd.ExecuteReader())
                        {
                            while (dReader != null && dReader.Read())
                            {
                                listExcel.Add(Convert.ToString(dReader["Code"]));
                            }
                        }
                    }
                }
                return listExcel;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public Tuple<bool, string> SaveToDatabase(string savedFilePath, AuthenticationType authenticationType, AuthenticationType userAuthenticationType, long levelId)
        {
            try
            {
                string connectionString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={savedFilePath};Extended Properties=Excel 12.0;";
                int index = (savedFilePath).LastIndexOf('.');
                int indexs = (savedFilePath).LastIndexOf('\\');
                string name = (savedFilePath).Substring(indexs + 1);
                string sheet = (savedFilePath).Substring(indexs + 1, index - (indexs + 1));

                using (OleDbConnection connection = new OleDbConnection(connectionString))
                {
                    using (
                        OleDbCommand cmd = new OleDbCommand("SELECT Code FROM [Sheet1$]", connection))
                    {
                        connection.Open();
                        using (OleDbDataReader dReader = cmd.ExecuteReader())
                        {
                            var lstExcel = new List<AuthenticationModel>();
                            while (dReader != null && dReader.Read())
                            {
                                AuthenticationModel authentication = new AuthenticationModel
                                {
                                    IdentityCode = Convert.ToString(dReader["Code"]),
                                    AuthenticationType = authenticationType
                                };
                                lstExcel.Add(authentication);
                            }
                            var lstAllAuthentication =
                                _authenticationRepository.All()
                                    .Select(
                                        s =>
                                            new AuthenticationModel
                                            {
                                                IdentityCode = s.IdentityCode,
                                                AuthenticationType = s.AuthenticationType
                                            })
                                    .ToList();

                            var lstNewAuthentication = lstExcel.Except(lstAllAuthentication, new AuthenticationComparer()).ToList();

                            var lstAuthen = new List<Authentication>();
                            foreach (var item in lstNewAuthentication)
                            {
                                Authentication authentication = new Authentication
                                {
                                    IdentityCode = item.IdentityCode,
                                    AuthenticationType = item.AuthenticationType
                                };
                                if (userAuthenticationType == AuthenticationType.AdminCentral)
                                    authentication.CentralOrganizationId = 1;
                                else if (userAuthenticationType == AuthenticationType.AdminCentral)
                                    authentication.BranchProvinceId = 1;
                                else
                                    authentication.UniversityId = 1;

                                lstAuthen.Add(authentication);
                            }

                            //    var lstAuthen = lstNewAuthentication.Select(item => new Authentication()
                            //    {
                            //        IdentityCode = item.IdentityCode,
                            //        AuthenticationType = item.AuthenticationType,
                            //        UniversityId = 1
                            //}).ToList();

                            lstAuthen.ForEach(b => _authenticationRepository.AddOrUpdate(b));
                            _unotOfWork.SaveChanges();
                        }
                    }
                }
                return new Tuple<bool, string>(true, "ثبت عملیات به درستی انجام شد");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "خطا در ثبت عملیات");
            }
        }

        public string GetLocalFilePath(string saveDirectory, FileUpload fileUploadControl)
        {
            //System.Web.UI.WebControls.WebControl
            string filePath = Path.Combine(saveDirectory, fileUploadControl.FileName);
            fileUploadControl.SaveAs(filePath);
            return filePath;
        }

        public bool CreateFolderIfNeeded(string path)
        {
            bool result = true;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }
                catch (Exception)
                {
                    /*TODO: You must process this exception.*/
                    result = false;
                }
            }
            return result;
        }
    }
}
