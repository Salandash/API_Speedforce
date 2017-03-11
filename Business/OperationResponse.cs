using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace API_Speedforce.Business
{
    public enum OperationState
    {
        Completed = 1,
        Failed =2,
        None = 0
    }

    public class OperationResponse<T>
    {
        public T Body { get; set; }
        public OperationState State { get; set; }
        public String Message { get; set; }

        public OperationResponse<T> Failed<T>(T body) where T : class
        {
            return new OperationResponse<T>() { State = OperationState.Failed, Body = body };
        }

        public OperationResponse<T> Failed(T body)
        {
            this.State = OperationState.Failed;
            this.Body = body;
            return this;
        }

        public OperationResponse<T> Failed(String message)
        {
            this.State = OperationState.Failed;
            this.Message = message;
            return this;
        }
        public OperationResponse<T> Failed(Exception ex)
        {
            this.State = OperationState.Failed;
            this.Message = ex.StackTrace;
            return this;
        }
        public OperationResponse<T> Failed<W>(OperationResponse<W> operationResponse)
        {
            return Failed(operationResponse.Message);
        }
        public OperationResponse<T> Failed()
        {
            return Failed(String.Empty);
        }

        public OperationResponse<T> Complete(T body)
        {
            this.State = OperationState.Completed;
            this.Body = body;
            return this;
        }
        public OperationResponse<T> Complete()
        {
            this.State = OperationState.Completed;
            return this;
        }

        public bool IsComplete()
        {
            return this.State == OperationState.Completed;
        }
    }
}