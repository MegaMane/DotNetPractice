using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Globalization;

namespace FanaticsPreprocessor
{
    class ImportFile: FanaticsFile
    {

        

        public ImportFile(List<string[]> records)
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
                "ParentLocation",
                "ImportToAllLocations",
                "BarcodeType",
                "Taxable",
                "ImportCompleted",
                "SpecialInstructions",
                "LocationKey",
                "TaxGroup",
                "ReplyTo"
            };

            SetEmail();

            this.Culture = new CultureInfo("en-US");
        }


        

        public override void SetEmail()
        {
            if (docList.Count < 2)
            {
                docList.Add(new string[] { "Screnshaw@fanatics.com" });
                //docList.Add(new string[] { "jludena@ptpos.com" });
            }
            int LastField = docList[1].GetUpperBound(0);
            String[] emailList = docList[1][LastField].Split(',');

            foreach(string email in emailList)
            {
                RegexUtilities emailCheck = new RegexUtilities();
                if (!emailCheck.IsValidEmail(email))
                {
                    //Invalid Email: Using Default Screnshaw@fanatics.com
                    //this.EmailRecipient = "jludena@ptpos.com";
                    this.EmailRecipient = "Screnshaw@fanatics.com";
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


        public override string HeaderComparison ()
        {
            string results = "";

            results += "<p>The headers in the file do not match the template. Please review at your earliest convenience and submit a revised file to the correct FTP folder." +
                            "This could be because the wrong file type was submitted to the FTP folder such as an update file in the import directory or Vice Versa, because of a typo, " +
                            "or because the number of fields do not match. Differences are highlighted in red</p><br /><br />";

            results +=
            "<table border=\"1\" bgcolor=\"white\">" +
                "<tr><th>Expected Number of fields is " + validFields.Length.ToString() + "</th><th> " +  "Header Row Has " + headers.Length.ToString() + " fields.</ th></tr>";
         
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
                    results += "<td style=\"color: red\" > Received :  " + '"' + (testedHeader =="" ? "Empty Field: Possible Extra Columns in Excel" :testedHeader) + '"' + "</td>";
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


        public override string ValidateBody ()
        {
            string results = "";
            //remove blank lines
            docList = docList.Where(s => !string.IsNullOrWhiteSpace(String.Join("", s).Trim())).ToList();
            results += CheckRequiredFields();
            results += CheckFieldLengths();
            results += CheckCurrencyFormat();


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

        public override string CheckCurrencyFormat()
        {
            string results = "";


            var currencyFields = new Dictionary<string, int>
            {
                    {"MSRP",5 },
                    {"SupplierCost",7 },
                    {"Default Price",9},
                    {"Default Cost",10 }


            };

            //start at index 1 and skip the headers
            for (int i = 1; i < docList.Count; i++)
            {
                string[] line = docList[i];

                foreach (KeyValuePair<string, int> currencyField in currencyFields.OrderBy(key => key.Value))
                {
                    double dollarAmount;
                    if(! double.TryParse(line[currencyField.Value], NumberStyles.Currency, this.Culture, out dollarAmount) )
                    {
                        results += $"<p>Invalid currency format for column \"{currencyField.Key}\" at line {i + 1}. The invalid value is: \"{line[currencyField.Value] }\"</p>";
                    }
                }
            }


            return results;
        }

        public override string CheckRequiredFields()
        {

            string results = "";

            List<List<int>> requiredFields = new List<List<int>>();
            
            string[] requiredFieldNames = {"Description", "Department", "Category", "MSRP",
                                           "Supplier", "Supplier Cost", "ParentLocation",
                                           "ImportToAllLocations", "BardcodeType","Taxable", "TaxGroup", "ReplyTo" };

            //hold the line numbers where these fields are blank
            List<int> description = new List<int>();           //index 1
            List<int> department = new List<int>();            //index 3
            List<int> category = new List<int>();              //index 4
            List<int> msrp = new List<int>();                  //index 5
            List<int> supplier = new List<int>();              //index 6
            List<int> supplierCost = new List<int>();          //index 7
            List<int> parentLocation = new List<int>();       //index 32
            List<int> importToAllLocations = new List<int>(); //index 33
            List<int> barcodeType = new List<int>();          //index 34
            List<int> taxable = new List<int>();              //index 35
            List<int> taxGroup = new List<int>();             //index 39
            List<int> replyTo = new List<int>();              //index 40

            requiredFields.Add(description);
            requiredFields.Add(department);
            requiredFields.Add(category);
            requiredFields.Add(msrp);
            requiredFields.Add(supplier);
            requiredFields.Add(supplierCost);
            requiredFields.Add(parentLocation);
            requiredFields.Add(importToAllLocations);
            requiredFields.Add(barcodeType);
            requiredFields.Add(taxable);
            requiredFields.Add(taxGroup);
            requiredFields.Add(replyTo);


            for (int record = 0; record < docList.Count; record++)
            {
                for (int field = 0; field < docList[record].Length; field++)
                {
                    if(docList[record][field] == "")
                    {
                        //loop over each line and note the lines with missing fields that are required
                        switch (field)
                        {
                            case 1:
                                //record + 1 equals the line number in the csv where this field is blank
                                description.Add(record + 1);
                                break;

                            case 3:
                                department.Add(record + 1);
                                break;

                            case 4:
                                category.Add(record + 1);
                                break;

                            case 5:
                                msrp.Add(record + 1);
                                break;

                            case 6:
                                supplier.Add(record + 1);
                                break;

                            case 7:
                                supplierCost.Add(record + 1);
                                break;

                            case 32:
                                parentLocation.Add(record + 1);
                                break;

                            case 33:
                                importToAllLocations.Add(record + 1);
                                break;

                            case 34:
                                barcodeType.Add(record + 1);
                                break;

                            case 35:
                                taxable.Add(record + 1);
                                break;

                            case 39:
                                taxGroup.Add(record + 1);
                                break;

                            case 40:
                                replyTo.Add(record + 1);
                                break;


                        }
                        
                    }
                }
            }


            //Build error message for required fields that are empty
            for (int i = 0; i < requiredFields.Count(); i ++)
            {
                if (requiredFields[i].Count > 0)
                {
                    results += "<p>" + (requiredFields[i].Count < 10 ? "The field " + requiredFieldNames[i] + " is required, but is blank on line(s): " + string.Join(",", requiredFields[i].Select(x => x.ToString()).ToArray()) + "." :
                        "The field " + requiredFieldNames[i] + " is required, but is blank on one or more lines in the file.") + "</p>";
                }
            }

            
            //check for missing location key where import to all locations = N
            List<string[]> missingLocationKeys = docList.Where(s => s[33] == "N" && s[38] == "").ToList();
            results += missingLocationKeys.Count() > 0 ? "<p>ImportToAllLocations is set to 'N' but the location key is blank on: " + missingLocationKeys.Count().ToString() + " line(s).</p>" : "";

            //check for location key where import to all locations = Y
            List<string[]> extraLocationKeys = docList.Where(s => s[33] == "Y" && s[38] != "").ToList();
            results += missingLocationKeys.Count() > 0 ? "<p>ImportToAllLocations is set to 'Y' but the location key is not blank on: " + missingLocationKeys.Count().ToString() + " line(s). Please keep this field blank when Import to all locations is set to Yes</p>" : "";

            return results;

      

        }

        public override string CheckFieldLengths()
        {
            List <DataField> DataFields = new List<DataField>();

            DataFields.Add(new DataField("SKU", 1, "NVARCHAR", 50));
            DataFields.Add(new DataField("Description",2,"NVARCHAR",128));
            DataFields.Add(new DataField("Extended Descriptions",3,"NVARCHAR",int.MaxValue));
            DataFields.Add(new DataField("Department",4,"NVARCHAR",30));
            DataFields.Add(new DataField("Category",5,"NVARCHAR",30));
            DataFields.Add(new DataField("MSRP",6,"FLOAT",-1));
            DataFields.Add(new DataField("Supplier",7,"NVARCHAR",30));
            DataFields.Add(new DataField("SupplierCost",8,"FLOAT",-1));
            DataFields.Add(new DataField("Reordernumber",9,"NVARCHAR",25));
            DataFields.Add(new DataField("Default Price",10,"FLOAT",-1));
            DataFields.Add(new DataField("Default Cost",11,"FLOAT",-1));
            DataFields.Add(new DataField("Size",12,"NVARCHAR",30));
            DataFields.Add(new DataField("Color",13,"NVARCHAR",30));
            DataFields.Add(new DataField("Team",14,"NVARCHAR",30));
            DataFields.Add(new DataField("Syncode 1 ,Product Line)",15,"NVARCHAR",50));
            DataFields.Add(new DataField("Barcode Number",16,"NVARCHAR",32));
            DataFields.Add(new DataField("Style",17,"NVARCHAR",250));
            DataFields.Add(new DataField("League",18,"NVARCHAR",200));
            DataFields.Add(new DataField("Series/Sport",19,"NVARCHAR",200));
            DataFields.Add(new DataField("Royalty Class",20,"NVARCHAR",200));
            DataFields.Add(new DataField("Season",21,"NVARCHAR",200));
            DataFields.Add(new DataField("Driver/Player Marks",22,"NVARCHAR",200));
            DataFields.Add(new DataField("Car/Player #",23,"NVARCHAR",200));
            DataFields.Add(new DataField("Sponsor",24,"NVARCHAR",200));
            DataFields.Add(new DataField("Year",25,"NVARCHAR",200));
            DataFields.Add(new DataField("Event/Program Marks",26,"NVARCHAR",200));
            DataFields.Add(new DataField("Owner",27,"NVARCHAR",200));
            DataFields.Add(new DataField("NTP Code",28,"NVARCHAR",200));
            DataFields.Add(new DataField("Royalty Code",29,"NVARCHAR",200));
            DataFields.Add(new DataField("Brand",30,"NVARCHAR",200));
            DataFields.Add(new DataField("Country Origin",31,"NVARCHAR",200));
            DataFields.Add(new DataField("Royalty Free",32,"NVARCHAR",200));
            DataFields.Add(new DataField("ParentLocation",33,"INT",-1));
            DataFields.Add(new DataField("ImportToallLocations",34,"NVARCHAR",50));
            DataFields.Add(new DataField("BarcodeType",35,"NVARCHAR",20));
            DataFields.Add(new DataField("Taxable",36,"NVARCHAR",3));
            DataFields.Add(new DataField("ImportCompleted",37,"NVARCHAR",3));
            DataFields.Add(new DataField("SpecialInstructions",38,"NVARCHAR",int.MaxValue));
            DataFields.Add(new DataField("LocationKey",39,"NVARCHAR",100));
            DataFields.Add(new DataField("TaxGroup",40,"NVARCHAR",100));
            DataFields.Add(new DataField("ReplyTo",41,"NVARCHAR",400));

            //check that the fields do not exceed the limits in the DB
            string results = "";
            //set record to 1 and skip the header file
            for (int record = 1; record < docList.Count; record++)
            {
                for (int field = 0; field < docList[record].Length; field++)
                {
                    if (docList[record][field].Length > DataFields[field].FieldLength && DataFields[field].FieldLength != -1)
                    {
                        string result = string.Format(
                            "The max length for {0} is {1}: Length of {2} exceeded the max on line {3}", 
                            DataFields[field].FieldName,
                            DataFields[field].FieldLength, 
                            docList[record][field].Length,
                            (record + 1)
                            );
                        results += "<p>" + result + "</p>";
                        
                    }
                }
            }

            return results;
        }

    }
}
