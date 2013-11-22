// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.md in the project root for license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNet.SignalR.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Microsoft.AspNet.SignalR.Hubs
{
    internal class HubRequestParser : IHubRequestParser
    {
        public HubRequest Parse(string data, JsonSerializer serializer)
        {
            var request = serializer.Parse<HubRequest>(data);
            return request;
        }

        private static IDictionary<string, object> GetState(HubRequest deserializedData)
        {
            if (deserializedData.State == null)
            {
                return new Dictionary<string, object>();
            }

            // Get the raw JSON string and check if it's over 4K
            string json = deserializedData.State.ToString();

            if (json.Length > 4096)
            {
                throw new InvalidOperationException(Resources.Error_StateExceededMaximumLength);
            }

            var settings = JsonUtility.CreateDefaultSerializerSettings();
            settings.Converters.Add(new SipHashBasedDictionaryConverter());
            var serializer = JsonSerializer.Create(settings);
            return serializer.Parse<IDictionary<string, object>>(json);
        }
    }
}
