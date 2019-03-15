using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanaticsPreprocessor
{
    class UpdateFile : FanaticsFile
    {

        public UpdateFile(List<string[]> records)
        {
            this.docList = records;
            this.document = records.ToArray();
            this.headers = document[0];

            this.validFields = new string[]
            {
                "SKU",
                "Description",
                "Extended Descriptions",
                "Department",
                "Category",
                "MSRP",
                "Supplier",
                "SupplierCost",
                "Reordernumber",
                "Default Price",
                "Default Cost",
                "Size",
                "Color",
                "Team",
                "Syncode 1 (Product Line)",
                "Barcode Number",
                "Style",
                "League",
                "Series/Sport",
                "Royalty Class",
                "Season",
                "Driver/Player Marks",
                "Car/Player #",
                "Sponsor",
                "Year",
                "Event/Program Marks",
                "Owner",
                "NTP Code",
                "Royalty Code",
                "Brand",
                "Country Origin",
                "Royalty Free",
                "SpecialInstructions",
                "ReplyTo"
            };

            SetEmail();

        }


       

        public override void SetEmail()
        {


            while (docList.Count < 2)
            {
                //docList.Add(new string[] { "Screnshaw@fanatics.com" });
                docList.Add(new string[] { "jludena@ptpos.com" });
            }


            int LastField = docList[1].GetUpperBound(0);
            String[] emailList = docList[1][LastField].Split(',');

            foreach (string email in emailList)
            {
                RegexUtilities emailCheck = new RegexUtilities();
                if (!emailCheck.IsValidEmail(email))
                {
                    //Invalid Email: Using Default Screnshaw@fanatics.com
                    this.EmailRecipient = "jludena@ptpos.com";
                    return;
                }

            }
            this.EmailRecipient = docList[1][LastField];
        }



        public override bool ValidHeaders()
        {
            if (headers.Length != validFields.Length)
            {
                return false;
            }

            for (int i = 0; i < validFields.Length; i++)
            {
                if (headers[i].ToLower() != validFields[i].ToLower())
                {
                    return false;
                }
            }
            return true;
        }


        public override string HeaderComparison()
        {
            string results = "";

            results += "<p>The headers in the file do not match the template. Please review at your earliest convenience and submit a revised file to the correct FTP folder." +
                            "This could be because the wrong file type was submitted to the FTP folder such as an update file in the import directory or Vice Versa, because of a typo, " +
                            "or because the number of fields do not match. Differences are highlighted in red</p><br /><br />";

            results +=
            "<table border=\"1\" bgcolor=\"white\">" +
                "<tr><th>Expected Number of fields is " + validFields.Length.ToString() + "</th><th> " + "Header Row Has " + headers.Length.ToString() + " fields.</ th></tr>";

            int maxLength = validFields.Length >= headers.Length ? validFields.Length : headers.Length;
            for (int i = 0; i < maxLength; i++)

            {
                string validHeader;
                string testedHeader;
                try
                {
                    validHeader = validFields[i];
                }

                catch (System.IndexOutOfRangeException)
                {
                    validHeader = "None";
                }

                try
                {
                    testedHeader = headers[i];
                }

                catch (System.IndexOutOfRangeException)
                {
                    testedHeader = "None";
                }

                results += "<tr>";

                if (validHeader.ToLower() != testedHeader.ToLower())
                {
                    results += "<td style=\"color: red\">Expected : " + '"' + validHeader + '"' + "</td>";
                    results += "<td style=\"color: red\" > Received :  " + '"' + (testedHeader == "" ? "Empty Field: Possible Extra Columns in Excel" : testedHeader) + '"' + "</td>";
                }
                else
                {
                    results += "<td>Expected : " + '"' + validHeader + '"' + "</td>";
                    results += "<td>Received :  " + '"' + testedHeader + '"' + "</td>";
                }



                results += "</tr>";
            }


            results += "</table>";

            results += "<br/><br/> If you have any questions regarding this email please contact support at support@ptpos.com." +
                "<br /><br />Thanks<br />The Opsuite Team";

            return results;
        }

        public override bool ValidFieldLengths()
        {
            for (int i = 0; i < docList.Count(); i++)
            {
                if (docList[i].Length != validFields.Length)
                {
                    return false;

                }
            }

            return true;
        }


        public override string FieldLengthComparison()
        {
            //check to make sure each line has the same number of fields as the header line
            string results = "";


            results += "<p>One or more lines do not have the same number of fields as the header record." +
                "Please review at your earliest convenience and submit a revised file to the correct FTP folder: </p>";

            for (int i = 0; i < docList.Count(); i++)
            {
                if (docList[i].Length != validFields.Length)
                {
                    results += "<p>Expected " + validFields.Length.ToString() +
                               " fields on line " + (i + 1).ToString() +
                               " , received " + docList[i].Length.ToString() +
                               ". Please check the file. </p>";

                }
            }

            results += "<br/><br/> If you have any questions regarding this email please contact support at support@ptpos.com." +
                "<br /><br />Thanks<br />The Opsuite Team";

            return results;
        }


        public override string ValidateBody()
        {
            string results = "";
            //remove blank lines
            docList = docList.Where(s => !string.IsNullOrWhiteSpace(String.Join("", s).Trim())).ToList();
            results += CheckRequiredFields();
            results += CheckFieldLengths();


            if (results != "")
            {
                string header = "The attached file has some data validation errors. Please review and return a revised file at your earliest convenience.<br />"
                        + "The following errors were found:<br />";

                results = header + results;
                results += "<br/><br/> If you have any questions regarding this email please contact support at support@ptpos.com." +
                    "<br /><br />Thanks<br />The Opsuite Team";
            }


            return results;

        }



        public override string CheckRequiredFields()
        {

            string results = "";

            List<List<int>> requiredFields = new List<List<int>>();

            string[] requiredFieldNames = {"SKU" };

            //hold the line numbers where these fields are blank
            List<int> sku = new List<int>();           //index 0


            requiredFields.Add(sku);



            for (int record = 0; record < docList.Count; record++)
            {
                for (int field = 0; field < docList[record].Length; field++)
                {
                    if (docList[record][field] == "")
                    {
                        //loop over each line and note the lines with missing fields that are required
                        switch (field)
                        {
                            case 0:
                                //record + 1 equals the line number in the csv where this field is blank
                                sku.Add(record + 1);
                                break;
                        }

                    }
                }
            }


            //Build error message for required fields that are empty
            for (int i = 0; i < requiredFields.Count(); i++)
            {
                if (requiredFields[i].Count > 0)
                {
                    results += "<p>" + (requiredFields[i].Count < 10 ? "The field " + requiredFieldNames[i] + " is required, but is blank on line(s): " + string.Join(",", requiredFields[i].Select(x => x.ToString()).ToArray()) + "." :
                        "The field " + requiredFieldNames[i] + " is required, but is blank on one or more lines in the file.") + "</p>";
                }
            }


            //check for supplier without supplier cost and vice versa
            List<string[]> invalidSupplierUpdate = docList.Where(s => (s[6] !="" && s[7] == "")||(s[6] == "" && s[7] != "")).ToList();
            results += invalidSupplierUpdate.Count() > 0 ? "<p>If either supplier or supplier cost needs to be updated then both fields are required. One of the values is missing on: " + invalidSupplierUpdate.Count().ToString() + " line(s).</p>" : "";

            return results;



        }

        public override string CheckFieldLengths()
        {
            //check that the fields do not exceed the limits in the DB
            string results = "";
            return results;
        }
    }
}
