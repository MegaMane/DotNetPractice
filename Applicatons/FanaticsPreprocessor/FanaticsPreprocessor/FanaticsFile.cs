using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanaticsPreprocessor
{
    public abstract class FanaticsFile
    {
        protected string[] headers;
        protected string[][] document;
        protected string[] validFields;
        protected CultureInfo Culture;
        public string EmailRecipient;
        public List<String[]> docList;




        public virtual void SetEmail()
        {
            throw new NotImplementedException();
        }
        public virtual bool ValidHeaders()
        {
            if (headers.Length != validFields.Length)
            {
                return false;
            }
            return true;
        }

        public virtual string HeaderComparison()
        {
            throw new NotImplementedException();
        }


        public virtual bool ValidFieldLengths()
        {
            throw new NotImplementedException();
        }

        public virtual string FieldLengthComparison()
        {
            throw new NotImplementedException();
        }
        public virtual string ValidateBody()
        {
            throw new NotImplementedException();
        }
        public virtual string CheckRequiredFields()
        {
            throw new NotImplementedException();
        }

        public virtual string CheckFieldLengths()
        {
            throw new NotImplementedException();
        }

        public virtual string CheckCurrencyFormat()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            string results = "";
            for (int line = 0; line < docList.Count; line++)
            {
                results += String.Format("[{0}]\n", string.Join("|", docList[line]));
            }

            return results;
        }
    }
}
