using OV.Models.Validation;
using System;
using System.Collections.Generic;
using System.Text;

namespace OV.Models.Response
{
    public interface IResponseObject
    {

    }

    public abstract class ResponseSuccess<T> : IResponseObject
    {
        public T ResponseObject { get; }
        protected ResponseSuccess(T responseObject)
        {
            ResponseObject = responseObject;
        }
    }

    public abstract class ResponseFailure<TCode> : IResponseObject where TCode : struct, Enum
    {
        public FailureReason<TCode>[] FailureReasons { get; }
        protected ResponseFailure(params FailureReason<TCode>[] failureReasons)
        {
            FailureReasons = failureReasons ?? new FailureReason<TCode>[0];
        }
    }

    public abstract class ResponseSuccess : IResponseObject { }

}
