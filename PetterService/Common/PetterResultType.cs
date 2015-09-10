using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetterService.Common
{
    public class PetterResultType<T>
    {
        public List<T> JsonDataSet;
        public bool IsSuccessful;
        public int AffectedRow;
        public string ErrorCode;
        public string ErrorMessage;
        public int ScalarValue;
        public string StringValue;
    }
}