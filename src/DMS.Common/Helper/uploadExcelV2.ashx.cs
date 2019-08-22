using Aspose.Cells;
using CSTJR.Enum;
using CSTJR.Member.Entity.DBEntity;
using CSTJR.PageLib;
using DMSFrame;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using TYSystem.BaseFramework.Common;
using TYSystem.BaseFramework.Common.Enum;
using TYSystem.BaseFramework.Common.Extension;
using TYSystem.BaseFramework.Common.Helper;
using TYSystem.BaseFramework.Common.Serializer;

namespace CSTJR.Admin.WebSite.common
{
    /// <summary>
    /// upload_json 的摘要说明
    /// </summary>
    public class uploadExcelV2 : AdminPageBase, IHttpHandler
    {
        public override void ProcessRequest(HttpContext context)
        {
            DateTime thisTime = DateTime.Now;
            FileUploadInfoResult uploadResult = new FileUploadInfoResult();

            try
            {
                #region 接收文件目录，后缀名
                //上传的位置："DYNAMICTYPEIMAGE"
                string vfolder = context.Request.Params["sitename"];
                string fileext = context.Request.Params["fileext"];//可传可不传
                if (vfolder.IsNullOrEmpty())
                {
                    uploadResult.err.Add("上传参数错误，未找到上传位置");
                    writeMsg(context, thisTime, "", uploadResult, false);
                    return;
                }
                #endregion


                HttpPostedFile files = context.Request.Files[0];
                Aspose.Cells.Workbook workbook = new Aspose.Cells.Workbook();
                workbook.Open(files.InputStream);
                Aspose.Cells.Worksheets worksheets = workbook.Worksheets;
                foreach (Worksheet worksheet in worksheets)
                {
                    string errMsg = string.Empty;
                    DMSTransactionScopeEntity tsEntity = new DMSTransactionScopeEntity();


                    string sheetName = worksheet.Name;
                    Cells cells = worksheet.Cells;
                    //DataTable datatable = cells.ExportDataTableAsString(0, 0, cells.MaxDataRow + 1, cells.MaxDataColumn + 1, false);//这里用到Aspose.Cells的ExportDataTableAsString方法来读取excel数据  

                    int vestType = 0;
                    if (sheetName == "公司")
                    {
                        vestType = 1;
                    }
                    else if (sheetName == "所有职位")
                    {
                        vestType = 2;
                    }
                    else if (sheetName == "真实姓名")
                    {
                        vestType = 3;
                    }
                    if (vestType <= 0)
                    {
                        uploadResult.err.Add("未找到Excel工作表（公司/所有职位/真实姓名）");
                        writeMsg(context, thisTime, "", uploadResult, false);
                        return;
                    }

                    for (int i = 0; i < cells.MaxDataRow + 1; i++)
                    {
                        string VestName = cells[i, 0].StringValue.Trim();
                        string VestTypeName = cells[i, 1].StringValue.Trim();

                        //添加实体
                        Mem_VestMemberInfo entity = new Mem_VestMemberInfo()
                        {
                            VestName = VestName,
                            VestType = vestType,
                            VestTypeName = VestTypeName,
                            Logo = "",
                            UseCount = 0,
                        };


                        if (vestType == 3)
                        {
                            string logo = cells[i, 2].StringValue.Trim();
                            if (logo.IsNullOrEmpty())
                            {
                                uploadResult.err.Add(sheetName + "：导入数据错误，请在Excel中编辑图片地址" + errMsg);
                                writeMsg(context, thisTime, vfolder, uploadResult, false);
                                return;
                            }
                            else
                            {
                                entity.Logo = logo;
                            }
                        }


                        tsEntity.AddTS<Mem_VestMemberInfo>(entity);

                    }

                    bool result = new DMSTransactionScopeHandler().Update(tsEntity, ref errMsg);
                    if (!result)
                    {
                        uploadResult.err.Add(sheetName + "：导入数据错误，" + errMsg);
                        writeMsg(context, thisTime, vfolder, uploadResult, result);
                        return;
                    }
                    else
                    {
                        uploadResult.err.Add(sheetName + "：数据导入成功");
                    }

                }


                writeMsg(context, thisTime, vfolder, uploadResult, true);
                return;
            }
            catch (Exception ex)
            {
                uploadResult.err.Add(ex.Message);
                writeMsg(context, thisTime, "", uploadResult, false);
            }
        }
        private void writeMsg(HttpContext context, DateTime thisTime, string vfolder, FileUploadInfoResult uploadResult, bool resultFlag)
        {
            UploadFileResult result = new UploadFileResult();
            for (int i = 0; i < uploadResult.err.Count; i++)
            {
                result.errmsg += uploadResult.err[i] + "======\r\n";
            }
            string callback = context.Request.Params["callback"];
            string fmt = context.Request.Params["fmt"];
            if (!string.IsNullOrEmpty(fmt) && !string.IsNullOrEmpty(callback))
            {
                System.Text.StringBuilder strScript = new System.Text.StringBuilder("");
                strScript.Append("<script type=\"text/javascript\">");
                strScript.AppendLine("      document.domain = \"" + SettingHandler.CookieDomain + "\";");
                strScript.Append("if(window.parent[\"" + callback + "\"]){");
                strScript.Append("window.parent." + callback + "(" + SerializerJson.SerializeObject(result) + ");");
                strScript.Append("}");
                strScript.Append("</script>");
                context.Response.Clear();
                context.Response.Write(strScript.ToString());
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write(SerializerJson.SerializeObject(result));
            }


        }

        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}