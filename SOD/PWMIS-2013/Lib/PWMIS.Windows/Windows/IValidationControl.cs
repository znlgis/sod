using System;
namespace PWMIS.Windows
{
    /// <summary>
    /// 验证控件接口
    /// </summary>
   public interface IValidationControl
    {
        string ClientValidationFunctionString { get; set; }
       
        
        string ErrorMessage { get; set; }
        bool IsClientValidation { get; set; }
        bool IsNull { get; set; }
        bool IsValid { get; }
        PWMIS.Windows.Validate.EnumMessageType MessageType { get; set; }
        string OftenType { get; set; }
        bool OnClickShowInfo { get; set; }
        string RegexName { get; set; }
        string RegexString { get; set; }
        PWMIS.Windows.ValidationDataType Type { get; set; }
        bool Validate();
      
    }
}
