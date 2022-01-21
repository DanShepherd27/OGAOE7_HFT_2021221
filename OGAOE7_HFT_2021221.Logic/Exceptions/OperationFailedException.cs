using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OGAOE7_HFT_2021221.Logic.Exceptions
{
    public class OperationFailedException<T> : Exception
    {
        Action<T> failedMethod;
        public OperationFailedException() : base("Performing operation has failed.")
        {

        }

        public OperationFailedException(string message) : base(message)
        {
        
        }
        public OperationFailedException(string message, Action<T> failedMethod) : base(message)
        {
            this.failedMethod = failedMethod;
        }
        public OperationFailedException(Action<T> failedMethod) : base($"Performing operation {failedMethod.Method} has failed.")
        {
            this.failedMethod = failedMethod;
        }
    }
}
