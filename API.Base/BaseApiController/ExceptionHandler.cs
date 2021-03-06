﻿using System;
using System.Collections.Generic;
using System.IdentityModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using MapHive.Server.Core.DataModel.Validation;

namespace MapHive.Server.Core.API
{
    public abstract partial class BaseApiController
    {
        /// <summary>
        /// Handles exception using a customised handler
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="exceptionHandler"></param>
        /// <returns></returns>
        protected virtual IHttpActionResult HandleException(Exception ex, Func<Exception, IHttpActionResult> exceptionHandler)
        {
            return HandleException(ex, new [] { exceptionHandler });
        }

        /// <summary>
        /// Standardised exception handler with an option to pass customised handlers. Uses the DefaultExceptionHandlers
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="exceptionHandlers"></param>
        /// <returns></returns>
        protected virtual IHttpActionResult HandleException(Exception ex, IEnumerable<Func<Exception, IHttpActionResult>> exceptionHandlers = null)
        {
            IHttpActionResult handled = null;
            foreach (var handler in exceptionHandlers ?? DefaultExceptionHandlers)
            {
                handled = handler(ex);
                if (handled != null)
                    break;
            }
            return handled;
        }
        
        /// <summary>
        /// Default exception handlers
        /// </summary>
        protected virtual IEnumerable<Func<Exception, IHttpActionResult>> DefaultExceptionHandlers
            => new List<Func<Exception, IHttpActionResult>>()
            {
                //model validation exceptions
                (e) =>
                {
                    if (e is ValidationFailedException && ((ValidationFailedException)e).ValidationErrors.Any())
                        return new NegotiatedContentResult<object>(HttpStatusCode.BadRequest, ((ValidationFailedException)e).ValidationErrors, this);
                    else return null;
                },

                //bad, bad requests...
                (e) =>
                {
                    if (e is BadRequestException)
                        return new NegotiatedContentResult<object>(HttpStatusCode.BadRequest, e.Message, this);
                    else return null;
                },

                //all the unfiltered end up as 500
                (e) =>
                {
                    #if DEBUG
                    return new NegotiatedContentResult<object>(HttpStatusCode.InternalServerError, e.Message, this);
                    #endif
                    return new NegotiatedContentResult<object>(HttpStatusCode.InternalServerError, string.Empty, this);
                }
            };
    }
}
