using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FanaticsPreprocessor
{
    class DataField
    {
        public string FieldName;
        public int FieldNumber;
        public int FieldLength;
        public string DataType;

        public int Index
        {
            get
            {
                return (FieldNumber - 1);
            }
        }


        public DataField(string fieldName, int fieldNumber, string dataType, int fieldLength)
        {
            this.FieldName = fieldName;
            this.FieldNumber = fieldNumber;
            this.DataType = dataType;
            this.FieldLength = fieldLength;
        }

    }
}

/*
CREATE TABLE [dbo].[SSIS_IMPORTStaging](
	[SKU] [NVARCHAR](50) NULL,
	[Description] [NVARCHAR](128) NULL,
	[Extended Descriptions] [NVARCHAR](MAX) NULL,
	[Department] [NVARCHAR](30) NULL,
	[Category] [NVARCHAR](30) NULL,
	[MSRP] [FLOAT] NULL,
	[Supplier] [NVARCHAR](30) NULL,
	[SupplierCost] [FLOAT] NULL,
	[Reordernumber] [NVARCHAR](25) NULL,
	[Default Price] [FLOAT] NULL,
	[Default Cost] [FLOAT] NULL,
	[Size] [NVARCHAR](30) NULL,
	[Color] [NVARCHAR](30) NULL,
	[Team] [NVARCHAR](30) NULL,
	[Syncode 1 (Product Line)] [NVARCHAR](50) NULL,
	[Barcode Number] [NVARCHAR](32) NULL,
	[Manufacturer] [NVARCHAR](50) NULL,
	[Style] [NVARCHAR](250) NULL,
	[League] [NVARCHAR](200) NULL,
	[Series/Sport] [NVARCHAR](200) NULL,
	[Royalty Class] [NVARCHAR](200) NULL,
	[Season] [NVARCHAR](200) NULL,
	[Driver/Player Marks] [NVARCHAR](200) NULL,
	[Car/Player #] [NVARCHAR](200) NULL,
	[Sponsor] [NVARCHAR](200) NULL,
	[Year] [NVARCHAR](200) NULL,
	[Event/Program Marks] [NVARCHAR](200) NULL,
	[Owner] [NVARCHAR](200) NULL,
	[NTP Code] [NVARCHAR](200) NULL,
	[Royalty Code] [NVARCHAR](200) NULL,
	[Brand] [NVARCHAR](200) NULL,
	[Country Origin] [NVARCHAR](200) NULL,
	[Royalty Free] [NVARCHAR](200) NULL,
	[ParentLocation] [INT] NULL,
	[ImportToallLocations] [NVARCHAR](50) NULL,
	[BarcodeType] [NVARCHAR](20) NULL,
	[Taxable] [NVARCHAR](3) NULL,
	[ImportCompleted] [NVARCHAR](3) NULL,
	[SpecialInstructions] [NVARCHAR](MAX) NULL,
	[LocationKey] [NVARCHAR](100) NULL,
	[TaxGroup] [NVARCHAR](100) NULL,
	[ReplyTo] [NVARCHAR](400) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO



CREATE TABLE [dbo].[SSIS_UPDATEStaging](
	[SKU] [NVARCHAR](50) NULL,
	[Description] [NVARCHAR](128) NULL,
	[Extended Descriptions] [NVARCHAR](MAX) NULL,
	[Department] [NVARCHAR](30) NULL,
	[Category] [NVARCHAR](30) NULL,
	[MSRP] [FLOAT] NULL,
	[Supplier] [NVARCHAR](30) NULL,
	[SupplierCost] [FLOAT] NULL,
	[Reordernumber] [NVARCHAR](50) NULL,
	[Default Price] [FLOAT] NULL,
	[Default Cost] [FLOAT] NULL,
	[Size] [NVARCHAR](30) NULL,
	[Color] [NVARCHAR](30) NULL,
	[Team] [NVARCHAR](30) NULL,
	[Syncode 1 (Product Line)] [NVARCHAR](50) NULL,
	[Barcode Number] [NVARCHAR](32) NULL,
	[Manufacturer] [NVARCHAR](50) NULL,
	[Style] [NVARCHAR](250) NULL,
	[League] [NVARCHAR](200) NULL,
	[Series/Sport] [NVARCHAR](200) NULL,
	[Royalty Class] [NVARCHAR](200) NULL,
	[Season] [NVARCHAR](200) NULL,
	[Driver/Player Marks] [NVARCHAR](200) NULL,
	[Car/Player #] [NVARCHAR](200) NULL,
	[Sponsor] [NVARCHAR](200) NULL,
	[Year] [NVARCHAR](200) NULL,
	[Event/Program Marks] [NVARCHAR](200) NULL,
	[Owner] [NVARCHAR](200) NULL,
	[NTP Code] [NVARCHAR](200) NULL,
	[Royalty Code] [NVARCHAR](200) NULL,
	[Brand] [NVARCHAR](200) NULL,
	[Country Origin] [NVARCHAR](200) NULL,
	[Royalty Free] [NVARCHAR](200) NULL,
	[SpecialInstructions] [NVARCHAR](MAX) NULL,
	[ReplyTo] [NVARCHAR](400) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
*/
