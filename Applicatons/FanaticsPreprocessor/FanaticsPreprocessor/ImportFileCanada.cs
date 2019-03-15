using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanaticsPreprocessor
{
    class ImportFileCanada: ImportFile
    {
        public ImportFileCanada(List<string[]> records)
            :base(records)
        {
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
                "Manufacturer",
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

            this.Culture = new CultureInfo("en-CA");

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
                    if (docList[record][field] == "")
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

                            case 33:
                                parentLocation.Add(record + 1);
                                break;

                            case 34:
                                importToAllLocations.Add(record + 1);
                                break;

                            case 35:
                                barcodeType.Add(record + 1);
                                break;

                            case 36:
                                taxable.Add(record + 1);
                                break;

                            case 40:
                                taxGroup.Add(record + 1);
                                break;

                            case 41:
                                replyTo.Add(record + 1);
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


            //check for missing location key where import to all locations = N
            List<string[]> missingLocationKeys = docList.Where(s => s[34] == "N" && s[39] == "").ToList();
            results += missingLocationKeys.Count() > 0 ? "<p>ImportToAllLocations is set to 'N' but the location key is blank on: " + missingLocationKeys.Count().ToString() + " line(s).</p>" : "";

            //check for location key where import to all locations = Y
            List<string[]> extraLocationKeys = docList.Where(s => s[34] == "Y" && s[39] != "").ToList();
            results += missingLocationKeys.Count() > 0 ? "<p>ImportToAllLocations is set to 'Y' but the location key is not blank on: " + missingLocationKeys.Count().ToString() + " line(s). Please keep this field blank when Import to all locations is set to Yes</p>" : "";


            return results;



        }


        public override string CheckFieldLengths()
        {
            List<DataField> DataFields = new List<DataField>();

            DataFields.Add(new DataField("SKU", 1, "NVARCHAR", 50));
            DataFields.Add(new DataField("Description", 2, "NVARCHAR", 128));
            DataFields.Add(new DataField("Extended Descriptions", 3, "NVARCHAR", int.MaxValue));
            DataFields.Add(new DataField("Department", 4, "NVARCHAR", 30));
            DataFields.Add(new DataField("Category", 5, "NVARCHAR", 30));
            DataFields.Add(new DataField("MSRP", 6, "FLOAT", -1));
            DataFields.Add(new DataField("Supplier", 7, "NVARCHAR", 30));
            DataFields.Add(new DataField("SupplierCost", 8, "FLOAT", -1));
            DataFields.Add(new DataField("Reordernumber", 9, "NVARCHAR", 25));
            DataFields.Add(new DataField("Default Price", 10, "FLOAT", -1));
            DataFields.Add(new DataField("Default Cost", 11, "FLOAT", -1));
            DataFields.Add(new DataField("Size", 12, "NVARCHAR", 30));
            DataFields.Add(new DataField("Color", 13, "NVARCHAR", 30));
            DataFields.Add(new DataField("Team", 14, "NVARCHAR", 30));
            DataFields.Add(new DataField("Syncode 1 ,Product Line)", 15, "NVARCHAR", 50));
            DataFields.Add(new DataField("Barcode Number", 16, "NVARCHAR", 32));
            DataFields.Add(new DataField("Manufacturer",17,"NVARCHAR",50));
            DataFields.Add(new DataField("Style", 18, "NVARCHAR", 250));
            DataFields.Add(new DataField("League", 19, "NVARCHAR", 200));
            DataFields.Add(new DataField("Series/Sport", 20, "NVARCHAR", 200));
            DataFields.Add(new DataField("Royalty Class", 21, "NVARCHAR", 200));
            DataFields.Add(new DataField("Season", 22, "NVARCHAR", 200));
            DataFields.Add(new DataField("Driver/Player Marks", 23, "NVARCHAR", 200));
            DataFields.Add(new DataField("Car/Player #", 24, "NVARCHAR", 200));
            DataFields.Add(new DataField("Sponsor", 25, "NVARCHAR", 200));
            DataFields.Add(new DataField("Year", 26, "NVARCHAR", 200));
            DataFields.Add(new DataField("Event/Program Marks", 27, "NVARCHAR", 200));
            DataFields.Add(new DataField("Owner", 28, "NVARCHAR", 200));
            DataFields.Add(new DataField("NTP Code", 29, "NVARCHAR", 200));
            DataFields.Add(new DataField("Royalty Code", 30, "NVARCHAR", 200));
            DataFields.Add(new DataField("Brand", 31, "NVARCHAR", 200));
            DataFields.Add(new DataField("Country Origin", 32, "NVARCHAR", 200));
            DataFields.Add(new DataField("Royalty Free", 33, "NVARCHAR", 200));
            DataFields.Add(new DataField("ParentLocation", 34, "INT", -1));
            DataFields.Add(new DataField("ImportToallLocations", 35, "NVARCHAR", 50));
            DataFields.Add(new DataField("BarcodeType", 36, "NVARCHAR", 20));
            DataFields.Add(new DataField("Taxable", 37, "NVARCHAR", 3));
            DataFields.Add(new DataField("ImportCompleted", 38, "NVARCHAR", 3));
            DataFields.Add(new DataField("SpecialInstructions", 39, "NVARCHAR", int.MaxValue));
            DataFields.Add(new DataField("LocationKey", 40, "NVARCHAR", 100));
            DataFields.Add(new DataField("TaxGroup", 41, "NVARCHAR", 100));
            DataFields.Add(new DataField("ReplyTo", 42, "NVARCHAR", 400));

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
