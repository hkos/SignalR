// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.SignalR.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNet.SignalR.Hubs
{
    public class DefaultParameterResolver : IParameterResolver
    {
        /// <summary>
        /// Resolves a parameter value based on the provided object.
        /// </summary>
        /// <param name="descriptor">Parameter descriptor.</param>
        /// <param name="value">Value to resolve the parameter value from.</param>
        /// <returns>The parameter value.</returns>
        public virtual object ResolveParameter(ParameterDescriptor descriptor, object value)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException("descriptor");
            }

            var jToken = value as JToken;
            if (jToken != null)
            {
                var serializer = GlobalHost.DependencyResolver.Resolve<JsonSerializer>();
                return jToken.ToObject(descriptor.ParameterType, serializer);
            }
            
            return value;
        }

        /// <summary>
        /// Resolves method parameter values based on provided objects.
        /// </summary>
        /// <param name="method">Method descriptor.</param>
        /// <param name="values">List of values to resolve parameter values from.</param>
        /// <returns>Array of parameter values.</returns>
        public virtual IList<object> ResolveMethodParameters(MethodDescriptor method, IList<object> values)
        {
            if (method == null)
            {
                throw new ArgumentNullException("method");
            }

            return method.Parameters.Zip(values, ResolveParameter).ToArray();
        }
    }
}
