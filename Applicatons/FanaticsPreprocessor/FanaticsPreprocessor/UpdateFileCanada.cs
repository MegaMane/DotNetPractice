using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanaticsPreprocessor
{
    class UpdateFileCanada: UpdateFile
    {
        public UpdateFileCanada(List<string[]> records)
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
                "SpecialInstructions",
                "ReplyTo"
            };


        }

    }
}
