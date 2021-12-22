using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DMS.XUnitTest.Models.xml
{

    [Serializable]
    [XmlRoot("上海市工程量清单电子文件")]
    public class GldEntity
    {
        [XmlElementAttribute(ElementName = "正文")]
        public 正文 正文 { get; set; }
        [XmlElementAttribute(ElementName = "数字签名信息")]
        public 数字签名信息 数字签名信息 { get; set; }
        [XmlAttribute]
        public string 文件类型 { get; set; }
        [XmlAttribute]
        public string 版本号 { get; set; }
    }

    public class 正文
    {
        public 项目基本信息 项目基本信息 { get; set; }
        public 清单文件编制属性信息 清单文件编制属性信息 { get; set; }
        public 工程量清单 工程量清单 { get; set; }
    }


    public class 项目基本信息
    {
        public string 工程报建号 { get; set; }
        public string 工程名称 { get; set; }
        public string 标段号 { get; set; }
    }
    public class 清单文件编制属性信息
    {
        public 最高投标限价清单编制信息 最高投标限价清单编制信息 { get; set; }
    }
    public class 最高投标限价清单编制信息
    {
        [XmlElement(ElementName = "最高投标限价-招标人")]
        public string 最高投标限价招标人 { get; set; }
        [XmlElement(ElementName = "最高投标限价-招标人-法定代表人或其授权人")]
        public string 最高投标限价招标人法定代表人或其授权人 { get; set; }

        [XmlElement(ElementName = "最高投标限价-工程造价咨询企业或招标代理机构")]
        public string 最高投标限价工程造价咨询企业或招标代理机构 { get; set; }
        [XmlElement(ElementName = "最高投标限价-工程造价咨询企业或招标代理机构-法定代表人或其授权人")]
        public string 最高投标限价工程造价咨询企业或招标代理机构法定代表人或其授权人 { get; set; }
        [XmlElement(ElementName = "最高投标限价-编制人")]
        public string 最高投标限价编制人 { get; set; }
        [XmlElement(ElementName = "最高投标限价-编制时间")]
        public string 最高投标限价编制时间 { get; set; }
        [XmlElement(ElementName = "最高投标限价-复核人")]
        public string 最高投标限价复核人 { get; set; }
        [XmlElement(ElementName = "最高投标限价-复核时间")]
        public string 最高投标限价复核时间 { get; set; }
        [XmlElement(ElementName = "最高投标限价-最高投标限价")]
        public string 最高投标限价最高投标限价 { get; set; }
        [XmlElement(ElementName = "最高投标限价-最高投标限价大写")]
        public string 最高投标限价最高投标限价大写 { get; set; }
        [XmlElement(ElementName = "最高投标限价-总说明")]
        public string 最高投标限价总说明 { get; set; }

        public 文件创建信息 文件创建信息 { get; set; }
    }
    public class 文件创建信息
    {
        public string 文件创建时间 { get; set; }
        public string CPU序列号 { get; set; }
        public string 硬盘序列号 { get; set; }
        public string 网卡MAC地址 { get; set; }
        public string 创建文件的软件厂商 { get; set; }
    }

    [Serializable]
    [XmlRoot("工程量清单")]
    public class 工程量清单
    {
        [XmlAttribute]
        public string 分部分项合计 { get; set; }
        [XmlAttribute]
        public string 措施项目合计 { get; set; }
        [XmlAttribute]
        public string 措施项目中安全防护文明施工措施合计 { get; set; }
        [XmlAttribute]
        public string 其他项目合计 { get; set; }
        [XmlAttribute]
        public string 规费项目合计 { get; set; }
        [XmlAttribute]
        public string 税金项目合计 { get; set; }
        [XmlAttribute]
        public string 总合计 { get; set; }
        [XmlElementAttribute(ElementName = "单项工程")]
        public List<单项工程> 单项工程 { get; set; }
        [XmlElement(ElementName = "工程项目-措施项目")]
        public 工程项目措施项目 工程项目措施项目 { get; set; }
    }

    public class 单项工程
    {
        public 单项工程序号 单项工程序号 { get; set; }
        public string 单项工程名称 { get; set; }
        [XmlElement(ElementName = "单位工程")]
        public List<单位工程> 单位工程 { get; set; }

    }

    public class 工程项目措施项目
    {
        public 总价措施项目 总价措施项目 { get; set; }
    }

    public class 总价措施项目
    {
        public 安全防护文明施工措施项目 安全防护文明施工措施项目 { get; set; }
    }

    public class 安全防护文明施工措施项目
    {
        [XmlElement]
        public List<总价措施项目子目> 总价措施项目子目 { get; set; }
    }

    public class 总价措施项目子目
    {
        public string 总价措施项目序号 { get; set; }
        public string 总价措施项目名称 { get; set; }
        [XmlElement(ElementName = "总价措施项目子目")]
        public List<总价措施项目子目> 总价措施项目子目p { get; set; }

    }



    public class 单项工程序号
    {
        [XmlAttribute]
        public string 序号GUID { get; set; }
        [XmlText]
        public string Value { get; set; }
    }

    public class 单位工程
    {
        public string 单位工程序号 { get; set; }
        public string 单位工程名称 { get; set; }
        public 分部分项 分部分项 { get; set; }
    }
    public class 分部分项
    {
        public 分部分项专业工程清单 分部分项专业工程清单 { get; set; }
    }
    public class 分部分项专业工程清单
    {
        public string 清单序号 { get; set; }
        public string 清单名称 { get; set; }
        public string 专业类别 { get; set; }
        [XmlElementAttribute(ElementName = "清单项子目")]
        public List<清单项子目> 清单项子目 { get; set; }
    }
    public class 清单项子目
    {
        public string 清单项子目序号 { get; set; }
        public string 项目编码 { get; set; }
        public string 项目名称 { get; set; }
        public string 计量单位 { get; set; }
        public string 工程量 { get; set; }
        public string 综合单价 { get; set; }
        public string 合价 { get; set; }
        public string 备注是否打印综合单价分析表 { get; set; }
        [XmlElementAttribute(ElementName = "项目特征描述")]
        public List<项目特征描述> 项目特征描述 { get; set; }
        [XmlElementAttribute(ElementName = "工程内容")]
        public List<工程内容> 工程内容 { get; set; }
        public 综合单价分析明细 综合单价分析明细 { get; set; }
    }
    public class 项目特征描述
    {
        public string 项目特征序号 { get; set; }
        public string 项目特征名称 { get; set; }
        public string 项目特征内容 { get; set; }
    }
    public class 工程内容
    {
        public string 工程内容序号 { get; set; }
        public string 工程内容描述 { get; set; }
        public string 工程内容说明 { get; set; }
    }
    public class 综合单价分析明细
    {
        public 综合单价组成 综合单价组成 { get; set; }
        public 人工材料机械明细 人工材料机械明细 { get; set; }
    }
    public class 综合单价组成
    {
        [XmlAttribute]
        public string 人工平均单价 { get; set; }
        [XmlAttribute]
        public string 未计价材料费合计 { get; set; }
        public 综合单价组成明细 综合单价组成明细 { get; set; }
    }
    public class 综合单价组成明细
    {
        public string 组成子目序号 { get; set; }
        public string 组成子目定额编号 { get; set; }
        public string 组成子目定额名称 { get; set; }

        public string 组成子目定额单位 { get; set; }
        public string 组成子目数量 { get; set; }
        public string 组成子目人工费 { get; set; }
        public string 组成子目材料费 { get; set; }
        public string 组成子目机械费 { get; set; }
        public string 组成子目管理费和利润 { get; set; }

        public string 组成子目人工费合价 { get; set; }
        public string 组成子目材料费合价 { get; set; }
        public string 组成子目机械费合价 { get; set; }
        public string 组成子目管理费和利润合价 { get; set; }
        public string 组成子目合价 { get; set; }
    }

    public class 人工材料机械明细
    {
        [XmlAttribute]
        public string 非主要其他材料费 { get; set; }
        [XmlElementAttribute(ElementName = "主要人材机明细")]
        public List<主要人材机明细> 主要人材机明细 { get; set; }
    }
    public class 主要人材机明细
    {
        public string 人材机明细序号 { get; set; }

        [XmlElement(ElementName = "人材机明细-类别")]
        public string 人材机明细类别 { get; set; }

        public string 人材机材料编码 { get; set; }

        [XmlElement(ElementName = "人材机明细-名称")]
        public string 人材机明细名称 { get; set; }

        [XmlElement(ElementName = "人材机明细-规格型号")]
        public string 人材机明细规格型号 { get; set; }

        [XmlElement(ElementName = "人材机明细-单位")]
        public string 人材机明细单位 { get; set; }

        [XmlElement(ElementName = "人材机明细-数量")]
        public string 人材机明细数量 { get; set; }

        [XmlElement(ElementName = "人材机明细-单价")]
        public string 人材机明细单价 { get; set; }

        [XmlElement(ElementName = "人材机明细-合价")]
        public string 人材机明细合价 { get; set; }

        [XmlElement(ElementName = "人材机明细-暂估单价")]
        public string 人材机明细暂估单价 { get; set; }
        [XmlElement(ElementName = "人材机明细-暂估合价")]
        public string 人材机明细暂估合价 { get; set; }

    }
    public class 数字签名信息
    {
        public string 签名结果 { get; set; }
        public string 证书序列号 { get; set; }
        public string 证书内容 { get; set; }
    }

}
