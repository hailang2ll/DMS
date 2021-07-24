using DMS.Common.BaseResult;
using DMS.Excel.Dto;
using DMS.Excel.Models;
using DMS.Excel.Result;
using DMS.Excel.Service;
using Microsoft.AspNetCore.Mvc;
using Rong.EasyExcel;
using Rong.EasyExcel.Attributes;
using Rong.EasyExcel.Models;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Threading.Tasks;

namespace DMS.Sample31.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        //public IExcelImporter Importer = new ExcelImporter();
        private readonly IExcelImportManager _excelImportManager;

        /// <summary>
        /// 
        /// </summary>
        public ExcelController(IExcelImportManager excelImportManager)
        {
            _excelImportManager = excelImportManager;
        }

        /// <summary>
        /// 
        /// </summary>
        public IExcelImporter Importer = new ExcelImporter();
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("Import")]
        public async Task<ResponseResult> Import()
        {
            ResponseResult result = new ResponseResult();

            var filePath = @"D:\导入Excel\产品导入模板.xlsx"; //Path.Combine(Directory.GetCurrentDirectory(), "TestFiles", "Import", "产品导入模板.xlsx");
                                                      //var res = await Importer.Import<ImportProductDto>(filePath);
                                                      ////result.ShouldNotBeNull();
            filePath = @"D:\导入Excel\数据注解测试模板.xlsx";

            ////result.HasError.ShouldBeTrue();
            ////result.RowErrors.Count.ShouldBe(1);
            ////result.Data.ShouldNotBeNull();
            ////result.Data.Count.ShouldBeGreaterThanOrEqualTo(2);
            //foreach (var item in res.Data)
            //{
            //    //if (item.Name != null && item.Name.Contains("空格测试")) item.Name.ShouldBe(item.Name.Trim());

            //    //if (item.Code.Contains("不去除空格测试")) item.Code.ShouldContain(" ");
            //    ////去除中间空格测试
            //    //item.BarCode.ShouldBe("123123");
            //}



            // DMS.Excel.ExcelHelper excelHelper = new DMS.Excel.ExcelHelper();
            // var data = excelHelper.Import<ImportTest>(filePath, opt =>
            //{
            //    opt.SheetIndex = 0;
            //    opt.ValidateMode = ExcelValidateModeEnum.ThrowRow;
            //});

            var res = await Importer.Import<ImportTestDataAnnotations>(filePath);
            result.errno = res.HasError ? 1 : 0;
            result.data = res.Data;

            return await Task.FromResult(result);
        }
    }





    /// <summary>
    /// 导入类
    /// </summary>
    public class ImportTest
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name = "姓名")]
        [Required(ErrorMessage = "{0}不能为空")]
        [StringLength(4, ErrorMessage = "{0}最大长度为{1}")]
        public virtual string Name { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name = "手机号")]
        [RegularExpression(@"^1[3456789]\d{9}$", ErrorMessage = "{0}格式错误")]
        public virtual string Phone { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        [Display(Name = "年龄")]
        [Required(ErrorMessage = "{0}不能为空")]
        [Range(10, 100, ErrorMessage = "{0}区间为{1}~{2}")]
        public virtual int Age { get; set; }

        /// <summary>
        /// 成绩
        /// </summary>
        [Display(Name = "成绩")]
        [Range(0, 150, ErrorMessage = "{0}区间为{1}~{2}")]
        public virtual decimal? Score { get; set; }

        /// <summary>
        /// 日期
        /// </summary>
        [Display(Name = "日期")]
        [DefaultValue(typeof(DateTime), "2020-9-9")]
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        [Display(Name = "时间")]
        [DefaultValue(typeof(TimeSpan), "100.10:20:30")]
        public virtual TimeSpan Time { get; set; }

        /// <summary>
        /// 学历
        /// </summary>
        [Display(Name = "学历")]
        [EnumDataType(typeof(TestEnum), ErrorMessage = "{0}值不存在")]
        public virtual TestEnum? Edu { get; set; }

        /// <summary>
        /// 学历文本（忽略）
        /// </summary>
        [Display(Name = "学历文本")]
        [IgnoreColumn]
        public virtual string EduText => typeof(TestEnum).GetField(Edu.ToString())?.Name;

        /// <summary>
        /// 无 DisplayName
        /// </summary>
        public virtual string NoDisplayName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum TestEnum
    {
        /// <summary>
        /// 
        /// </summary>
        小学 = 1,
        /// <summary>
        /// 
        /// </summary>
        中学,
        /// <summary>
        /// 
        /// </summary>
        大学
    }













    ///// <summary>
    ///// 测试表头位置
    ///// </summary>
    //public class ImportTest
    //{
    //    /// <summary>
    //    ///     产品名称
    //    /// </summary>
    //    [Display(Name = "产品名称")]
    //    public string Name { get; set; }

    //    /// <summary>
    //    ///     产品代码
    //    ///     长度验证
    //    /// </summary>
    //    [Display(Name = "产品代码")]
    //    public string Code { get; set; }

    //    /// <summary>
    //    ///  测试GUID
    //    /// </summary>
    //    public Guid ProductIdTest1 { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Guid? ProductIdTest2 { get; set; }

    //    /// <summary>
    //    ///     产品条码
    //    /// </summary>
    //    [Display(Name = "产品条码")]
    //    public string BarCode { get; set; }

    //    /// <summary>
    //    ///     客户Id
    //    /// </summary>
    //    [Display(Name = "客户代码")]
    //    public long ClientId { get; set; }

    //    /// <summary>
    //    ///     产品型号
    //    /// </summary>
    //    [Display(Name = "产品型号")]
    //    public string Model { get; set; }

    //    /// <summary>
    //    ///     申报价值
    //    /// </summary>
    //    [Display(Name = "申报价值")]
    //    public double DeclareValue { get; set; }

    //    /// <summary>
    //    ///     货币单位
    //    /// </summary>
    //    [Display(Name = "货币单位")]
    //    public string CurrencyUnit { get; set; }

    //    /// <summary>
    //    ///     品牌名称
    //    /// </summary>
    //    [Display(Name = "品牌名称")]
    //    public string BrandName { get; set; }

    //    /// <summary>
    //    ///     尺寸
    //    /// </summary>
    //    [Display(Name = "尺寸(长x宽x高)")]
    //    public string Size { get; set; }

    //    /// <summary>
    //    ///     重量（支持不设置ImporterHeader）
    //    /// </summary>
    //    //[ImporterHeader(Name = "重量(KG)")]
    //    [Display(Name = "重量(KG)")]
    //    public double? Weight { get; set; }

    //    /// <summary>
    //    ///     类型
    //    /// </summary>
    //    [Display(Name = "类型")]
    //    public ImporterProductType Type { get; set; }

    //    /// <summary>
    //    ///     是否行
    //    /// </summary>
    //    [Display(Name = "是否行")]
    //    public bool IsOk { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [Display(Name = "公式测试")]
    //    public DateTime FormulaTest { get; set; }

    //    /// <summary>
    //    ///     身份证
    //    ///     多个错误测试
    //    /// </summary>
    //    [Display(Name = "身份证")]
    //    public string IdNo { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [Display(Name = "性别")]
    //    public string Sex { get; set; }
    //}




    ///// <summary>
    ///// 测试表头位置
    ///// </summary>
    //[Importer(HeaderRowIndex = 2)]
    //public class ImportProductDto
    //{
    //    /// <summary>
    //    ///     产品名称
    //    /// </summary>
    //    [ImporterHeader(Name = "产品名称", Description = "必填")]
    //    [Required(ErrorMessage = "产品名称是必填的")]
    //    public string Name { get; set; }

    //    /// <summary>
    //    ///     产品代码
    //    ///     长度验证
    //    /// </summary>
    //    [ImporterHeader(Name = "产品代码", Description = "最大长度为20", AutoTrim = false)]
    //    [MaxLength(20, ErrorMessage = "产品代码最大长度为20（中文算两个字符）")]
    //    public string Code { get; set; }

    //    /// <summary>
    //    ///  测试GUID
    //    /// </summary>
    //    public Guid ProductIdTest1 { get; set; }
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    public Guid? ProductIdTest2 { get; set; }

    //    /// <summary>
    //    ///     产品条码
    //    /// </summary>
    //    [ImporterHeader(Name = "产品条码", FixAllSpace = true)]
    //    [MaxLength(10, ErrorMessage = "产品条码最大长度为10")]
    //    [RegularExpression(@"^\d*$", ErrorMessage = "产品条码只能是数字")]
    //    public string BarCode { get; set; }

    //    /// <summary>
    //    ///     客户Id
    //    /// </summary>
    //    [ImporterHeader(Name = "客户代码", ColumnIndex = 6)]
    //    public long ClientId { get; set; }

    //    /// <summary>
    //    ///     产品型号
    //    /// </summary>
    //    [ImporterHeader(Name = "产品型号")]
    //    public string Model { get; set; }

    //    /// <summary>
    //    ///     申报价值
    //    /// </summary>
    //    [ImporterHeader(Name = "申报价值")]
    //    public double DeclareValue { get; set; }

    //    /// <summary>
    //    ///     货币单位
    //    /// </summary>
    //    [ImporterHeader(Name = "货币单位")]
    //    public string CurrencyUnit { get; set; }

    //    /// <summary>
    //    ///     品牌名称
    //    /// </summary>
    //    [ImporterHeader(Name = "品牌名称")]
    //    public string BrandName { get; set; }

    //    /// <summary>
    //    ///     尺寸
    //    /// </summary>
    //    [ImporterHeader(Name = "尺寸(长x宽x高)")]
    //    public string Size { get; set; }

    //    /// <summary>
    //    ///     重量（支持不设置ImporterHeader）
    //    /// </summary>
    //    //[ImporterHeader(Name = "重量(KG)")]
    //    [Display(Name = "重量(KG)")]
    //    public double? Weight { get; set; }

    //    /// <summary>
    //    ///     类型
    //    /// </summary>
    //    [ImporterHeader(Name = "类型")]
    //    public ImporterProductType Type { get; set; }

    //    /// <summary>
    //    ///     是否行
    //    /// </summary>
    //    [ImporterHeader(Name = "是否行")]
    //    public bool IsOk { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [ImporterHeader(Name = "公式测试", Format = "yyyy-MM-dd")]
    //    public DateTime FormulaTest { get; set; }

    //    /// <summary>
    //    ///     身份证
    //    ///     多个错误测试
    //    /// </summary>
    //    [ImporterHeader(Name = "身份证")]
    //    [RegularExpression(@"(^\d{15}$)|(^\d{18}$)|(^\d{17}(\d|X|x)$)", ErrorMessage = "身份证号码无效！")]
    //    [StringLength(18, ErrorMessage = "身份证长度不得大于18！")]
    //    public string IdNo { get; set; }

    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [Display(Name = "性别")]
    //    public string Sex { get; set; }
    //}

    ///// <summary>
    ///// 
    ///// </summary>
    //public enum ImporterProductType
    //{
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [Display(Name = "第一")] One,
    //    /// <summary>
    //    /// 
    //    /// </summary>
    //    [Display(Name = "第二")] Two
    //}
}
