using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Mobile_Deura.Const;

namespace Mobile_Deura.Untils
{
    public class ValidateUtils
    {
        public string CheckRequiredField (string data , string name, int lengthMin, int lengthMax = -1)
        {
            if (string.IsNullOrEmpty(data))
            {
                return string.Format(SystemMessageConst.ValidateConst.CheckNotEmpty,name);
            }
            if (lengthMax == -1)
            {
                if (data.Length < lengthMin)
                {
                    return string.Format(SystemMessageConst.ValidateConst.MinlengthOfText, name, lengthMin);
                }
                return null;
            }
            if (data.Length < lengthMin || data.Length > lengthMax)
            {
                return string.Format(SystemMessageConst.ValidateConst.MinMaxlengthOfText, name, lengthMax, lengthMin);
            }
            return null;
        }

        public string CheckNonRequiredField(string data, string name, int lengthMax)
        {
            if (data.Length > lengthMax)
            {
                return string.Format(SystemMessageConst.ValidateConst.MaxlengthOfText, name, lengthMax);
            }
            return null;
        }

        public string CheckEmail(string emailString)
        {
            bool isEmail = Regex.IsMatch(emailString, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
            if (!isEmail)
            {
                return SystemMessageConst.ValidateConst.EmailNotCorrectFormatEn;
            }
            return null;
        }

        public bool PhoneService(string phone)
        {
            bool viptype = false;
            List<string> tenNumber = new List<string>() { "086", "096", "097", "098", "088", "091", "094", "092", "089", "090", "093" };
            List<string> eleventNumber = new List<string>() { "016", "0123", "0124", "0125", "0127", "0129", "0186", "0188", "0120", "0121", "0122", "0126", "0128" };
            List<string> Viettel = new List<string>() { "086", "096", "097", "098", "016" };
            List<string> Vinaphone = new List<string>() { "088", "091", "094", "0123", "0124", "0125", "0127", "0129" };
            List<string> Vietnammobile = new List<string>() { "092", "0186", "0188" };
            List<string> Mobiphone = new List<string>() { "089", "090", "093", "0120", "0121", "0122", "0126", "0128" };

            if (Viettel.Where(p => phone.StartsWith(p)).Any())
            {
                viptype = true;
            }

            if (Vinaphone.Where(p => phone.StartsWith(p)).Any())
            {
                viptype = true;
            }

            if (Vietnammobile.Where(p => phone.StartsWith(p)).Any())
            {
                viptype = true;
            }

            if (Mobiphone.Where(p => phone.StartsWith(p)).Any())
            {
                viptype = true;
            }

            return viptype;
        }
    }
}