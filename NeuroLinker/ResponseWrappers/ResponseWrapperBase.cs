using System;
using System.Net;

namespace NeuroLinker.ResponseWrappers
{
    /// <summary>
    /// Wrapper for retrieved data, it includes whether the retrieval was a success as well as the actual staus code received
    /// </summary>
    public class ResponseWrapperBase
    {
        #region Constructor

        /// <summary>
        /// Construct with a Status code and indicate if the push was a success
        /// </summary>
        /// <param name="responseStatusCode">HttpStatus code received from the remote server</param>
        /// <param name="success">Was the request successful or not</param>
        public ResponseWrapperBase(HttpStatusCode responseStatusCode, bool success)
        {
            ResponseStatusCode = responseStatusCode;
            Success = success;
            Exception = null;
        }

        /// <summary>
        /// Construct with an exception
        /// </summary>
        /// <param name="exception">Exception that occured during retrieval</param>
        public ResponseWrapperBase(Exception exception)
        {
            Exception = exception;
            ResponseStatusCode = null;
            Success = false;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicate if an exception has occured during the data retrieval
        /// <remarks>
        ///     If this value is null no error occured.
        ///     If this value is populate an exception occured during data retrieval
        /// </remarks>
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Status code received from the Mal server.
        /// <remarks>
        /// If this value is null it means some error occured and the <see cref="Exception"/> should be inspected for the problem
        /// </remarks>
        /// </summary>
        public HttpStatusCode? ResponseStatusCode { get; }

        /// <summary>
        /// Indicate if the retrieval was a success
        /// </summary>
        public bool Success { get; }

        #endregion
    }
}