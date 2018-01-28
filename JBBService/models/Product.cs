using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JBBService.models
{
    public class Product
    {
        public Product()
        {
        }

        public Product(int intProdID,string strRfidNo, string strProdName, string strProdCode,string strMyCode, string strMyCodeName, int intStatus,string strRemark, decimal decPrice)
        {
            prodId = intProdID;
            rfidNo = strRfidNo;
            prodName = strProdName;
            prodCode = strProdCode;
            MyCode = strMyCode;
            MyCodeName = strMyCodeName;
            Status = intStatus;
            remark = strRemark;
            Price = decPrice;
        }
        public int prodId { get; set; }
        public string rfidNo { get; set; }
        public string prodName { get; set; }
        public string prodCode { get; set; }
        public string MyCode { get; set; }
        public string MyCodeName { get; set; }
        public int Status { get; set; }
        public string remark { get; set; }
        public decimal Price { get; set; }
    }
}


