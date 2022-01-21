using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic.Exceptions
{
    public class UnsupportedValueException : Exception
    {
        public int Id { get; set; }
        public UnsupportedValueException(int id) : base($"Value is not supported: {id} Don't be stoopid!")
        {
            Id = id;
        }
    }
}
